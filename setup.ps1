# Script de setup para WorkBalance Hub API
# Execute este script no PowerShell

Write-Host "=== WorkBalance Hub - Setup ===" -ForegroundColor Cyan
Write-Host ""

# Verificar se o .NET SDK está instalado
Write-Host "Verificando .NET SDK..." -ForegroundColor Yellow
$dotnetVersion = dotnet --version
if ($LASTEXITCODE -ne 0) {
    Write-Host "ERRO: .NET SDK não encontrado. Instale o .NET 8.0 SDK." -ForegroundColor Red
    exit 1
}
Write-Host "✓ .NET SDK $dotnetVersion encontrado" -ForegroundColor Green

# Verificar se o EF Core Tools está instalado
Write-Host "Verificando EF Core Tools..." -ForegroundColor Yellow
$efVersion = dotnet ef --version 2>&1
if ($LASTEXITCODE -ne 0) {
    Write-Host "Instalando EF Core Tools..." -ForegroundColor Yellow
    dotnet tool install --global dotnet-ef
    if ($LASTEXITCODE -ne 0) {
        Write-Host "ERRO: Falha ao instalar EF Core Tools." -ForegroundColor Red
        exit 1
    }
}
Write-Host "✓ EF Core Tools instalado" -ForegroundColor Green

# Restaurar pacotes NuGet
Write-Host "Restaurando pacotes NuGet..." -ForegroundColor Yellow
dotnet restore
if ($LASTEXITCODE -ne 0) {
    Write-Host "ERRO: Falha ao restaurar pacotes." -ForegroundColor Red
    exit 1
}
Write-Host "✓ Pacotes restaurados" -ForegroundColor Green

# Compilar o projeto
Write-Host "Compilando projeto..." -ForegroundColor Yellow
dotnet build
if ($LASTEXITCODE -ne 0) {
    Write-Host "ERRO: Falha na compilação." -ForegroundColor Red
    exit 1
}
Write-Host "✓ Projeto compilado com sucesso" -ForegroundColor Green

# Criar migration inicial
Write-Host "Criando migration inicial..." -ForegroundColor Yellow
Set-Location "src\WorkBalanceHub.API"
dotnet ef migrations add InitialCreate --project ..\WorkBalanceHub.Infrastructure --startup-project .
if ($LASTEXITCODE -ne 0) {
    Write-Host "AVISO: Migration pode já existir. Continuando..." -ForegroundColor Yellow
}
Set-Location ..\..
Write-Host "✓ Migration criada" -ForegroundColor Green

# Aplicar migrations
Write-Host "Aplicando migrations ao banco de dados..." -ForegroundColor Yellow
Set-Location "src\WorkBalanceHub.API"
dotnet ef database update --project ..\WorkBalanceHub.Infrastructure --startup-project .
if ($LASTEXITCODE -ne 0) {
    Write-Host "ERRO: Falha ao aplicar migrations. Verifique a connection string." -ForegroundColor Red
    Set-Location ..\..
    exit 1
}
Set-Location ..\..
Write-Host "✓ Migrations aplicadas" -ForegroundColor Green

Write-Host ""
Write-Host "=== Setup concluído com sucesso! ===" -ForegroundColor Green
Write-Host ""
Write-Host "Para executar a API:" -ForegroundColor Cyan
Write-Host "  cd src\WorkBalanceHub.API" -ForegroundColor White
Write-Host "  dotnet run" -ForegroundColor White
Write-Host ""
Write-Host "Acesse o Swagger em: https://localhost:5001/swagger" -ForegroundColor Cyan

