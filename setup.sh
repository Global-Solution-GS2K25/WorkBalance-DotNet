#!/bin/bash
# Script de setup para WorkBalance Hub API
# Execute este script no Linux/Mac

echo "=== WorkBalance Hub - Setup ==="
echo ""

# Verificar se o .NET SDK está instalado
echo "Verificando .NET SDK..."
if ! command -v dotnet &> /dev/null; then
    echo "ERRO: .NET SDK não encontrado. Instale o .NET 8.0 SDK."
    exit 1
fi
DOTNET_VERSION=$(dotnet --version)
echo "✓ .NET SDK $DOTNET_VERSION encontrado"

# Verificar se o EF Core Tools está instalado
echo "Verificando EF Core Tools..."
if ! dotnet ef --version &> /dev/null; then
    echo "Instalando EF Core Tools..."
    dotnet tool install --global dotnet-ef
    if [ $? -ne 0 ]; then
        echo "ERRO: Falha ao instalar EF Core Tools."
        exit 1
    fi
fi
echo "✓ EF Core Tools instalado"

# Restaurar pacotes NuGet
echo "Restaurando pacotes NuGet..."
dotnet restore
if [ $? -ne 0 ]; then
    echo "ERRO: Falha ao restaurar pacotes."
    exit 1
fi
echo "✓ Pacotes restaurados"

# Compilar o projeto
echo "Compilando projeto..."
dotnet build
if [ $? -ne 0 ]; then
    echo "ERRO: Falha na compilação."
    exit 1
fi
echo "✓ Projeto compilado com sucesso"

# Criar migration inicial
echo "Criando migration inicial..."
cd src/WorkBalanceHub.API
dotnet ef migrations add InitialCreate --project ../WorkBalanceHub.Infrastructure --startup-project . 2>/dev/null
if [ $? -ne 0 ]; then
    echo "AVISO: Migration pode já existir. Continuando..."
fi
cd ../..
echo "✓ Migration criada"

# Aplicar migrations
echo "Aplicando migrations ao banco de dados..."
cd src/WorkBalanceHub.API
dotnet ef database update --project ../WorkBalanceHub.Infrastructure --startup-project .
if [ $? -ne 0 ]; then
    echo "ERRO: Falha ao aplicar migrations. Verifique a connection string."
    cd ../..
    exit 1
fi
cd ../..
echo "✓ Migrations aplicadas"

echo ""
echo "=== Setup concluído com sucesso! ==="
echo ""
echo "Para executar a API:"
echo "  cd src/WorkBalanceHub.API"
echo "  dotnet run"
echo ""
echo "Acesse o Swagger em: https://localhost:5001/swagger"

