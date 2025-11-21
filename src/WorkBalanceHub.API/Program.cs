using Microsoft.EntityFrameworkCore;
using FluentValidation;
using WorkBalanceHub.Infrastructure.Data;
using WorkBalanceHub.Domain.Repositories;
using WorkBalanceHub.Infrastructure.Repositories;
using WorkBalanceHub.Application.Services;
using WorkBalanceHub.Application.Validators;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers()
    .ConfigureApiBehaviorOptions(options =>
    {
        options.InvalidModelStateResponseFactory = context =>
        {
            var errors = context.ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
            var problemDetails = new Microsoft.AspNetCore.Mvc.ProblemDetails
            {
                Title = "Erro de validação",
                Detail = string.Join("; ", errors),
                Status = 400,
                Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1"
            };
            return new Microsoft.AspNetCore.Mvc.BadRequestObjectResult(problemDetails);
        };
    });
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "WorkBalance Hub API",
        Version = "v1",
        Description = "API para monitoramento de bem-estar dos colaboradores no ambiente de trabalho"
    });
});

// Configurar ProblemDetails
builder.Services.AddProblemDetails();

// Configurar Entity Framework
// Choose DB provider: if env var USE_SQLITE=true then use SQLite file for easy local testing.
var useSqlite = Environment.GetEnvironmentVariable("USE_SQLITE")?.ToLower() == "true";
if (useSqlite)
{
    var sqliteFile = builder.Configuration.GetValue<string>("Sqlite:File") ?? "workbalancehub.db";
    builder.Services.AddDbContext<WorkBalanceHubDbContext>(options =>
        options.UseSqlite($"Data Source={sqliteFile}"));
}
else
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") 
        ?? "Server=(localdb)\\mssqllocaldb;Database=WorkBalanceHub;Trusted_Connection=True;MultipleActiveResultSets=true";

    builder.Services.AddDbContext<WorkBalanceHubDbContext>(options =>
        options.UseSqlServer(connectionString));
}

// Registrar Repositórios
builder.Services.AddScoped<IRepository<WorkBalanceHub.Domain.Entities.Equipe>, EquipeRepository>();
builder.Services.AddScoped<IRepository<WorkBalanceHub.Domain.Entities.EstacaoTrabalho>, EstacaoTrabalhoRepository>();
builder.Services.AddScoped<IColaboradorRepository, ColaboradorRepository>();
builder.Services.AddScoped<ICheckInRepository, CheckInRepository>();
builder.Services.AddScoped<ILeituraAmbienteRepository, LeituraAmbienteRepository>();
builder.Services.AddScoped<IPlanoAcaoRepository, PlanoAcaoRepository>();

// Registrar Serviços de Aplicação
builder.Services.AddScoped<IColaboradorService, ColaboradorService>();
builder.Services.AddScoped<IEquipeService, EquipeService>();
builder.Services.AddScoped<ICheckInService, CheckInService>();
builder.Services.AddScoped<ILeituraAmbienteService, LeituraAmbienteService>();
builder.Services.AddScoped<IEstacaoTrabalhoService, EstacaoTrabalhoService>();
builder.Services.AddScoped<IPlanoAcaoService, PlanoAcaoService>();

// Registrar Validadores FluentValidation
builder.Services.AddValidatorsFromAssemblyContaining<CriarColaboradorDtoValidator>();

var app = builder.Build();

// Configure the HTTP request pipeline.
// Swagger sempre habilitado para facilitar testes (em produção, remover ou proteger)
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "WorkBalance Hub API v1");
    c.RoutePrefix = string.Empty; // Swagger na raiz
});

app.UseHttpsRedirection();
app.UseStatusCodePages();

app.UseAuthorization();

app.MapControllers();

// If using SQLite for demo, ensure DB is created and seed minimal data
using (var scope = app.Services.CreateScope())
{
    var envUseSqlite = Environment.GetEnvironmentVariable("USE_SQLITE")?.ToLower() == "true";
    if (envUseSqlite)
    {
        var db = scope.ServiceProvider.GetRequiredService<WorkBalanceHub.Infrastructure.Data.WorkBalanceHubDbContext>();
        db.Database.EnsureCreated();

        // minimal seed if no equipes
        if (!db.Equipes.Any())
        {
            var equipe = new WorkBalanceHub.Domain.Entities.Equipe("Equipe Alpha", "Equipe padrão para testes");
            db.Equipes.Add(equipe);
            db.SaveChanges();

            var colaborador = new WorkBalanceHub.Domain.Entities.Colaborador("João Silva", "joao@exemplo.com", "Desenvolvedor", equipe.Id);
            db.Colaboradores.Add(colaborador);
            db.SaveChanges();
        }
    }
}

app.Run();

