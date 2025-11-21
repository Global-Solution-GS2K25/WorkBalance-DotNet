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
            var problemDetails = new Microsoft.AspNetCore.Mvc.ProblemDetails
            {
                Title = "Erro de validação",
                Detail = "Um ou mais erros de validação ocorreram.",
                Status = 400,
                Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1"
            };
            return new BadRequestObjectResult(problemDetails);
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
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") 
    ?? "Server=(localdb)\\mssqllocaldb;Database=WorkBalanceHub;Trusted_Connection=True;MultipleActiveResultSets=true";

builder.Services.AddDbContext<WorkBalanceHubDbContext>(options =>
    options.UseSqlServer(connectionString));

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
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseStatusCodePages();

app.UseAuthorization();

app.MapControllers();

app.Run();

