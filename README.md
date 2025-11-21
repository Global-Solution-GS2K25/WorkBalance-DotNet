# WorkBalance Hub API

## Visão Geral

O **WorkBalance Hub** é uma plataforma integrada desenvolvida para monitorar, analisar e apoiar o bem-estar dos colaboradores no ambiente de trabalho. A solução combina dados subjetivos (check-ins emocionais) e objetivos (condições do ambiente físico), com o objetivo de prevenir o adoecimento mental, melhorar o clima organizacional e apoiar a tomada de decisão do RH e da gestão.

Este projeto implementa a API REST do WorkBalance Hub utilizando **.NET 8**, seguindo os princípios de **Clean Architecture** e **Domain-Driven Design (DDD)**.

## Arquitetura

O projeto está organizado em camadas seguindo os princípios de Clean Architecture:

### 1. **Domain** (Domínio)
- **Entidades**: `Colaborador`, `Equipe`, `CheckIn`, `EstacaoTrabalho`, `LeituraAmbiente`, `PlanoAcao`
- **Repositórios (Interfaces)**: Definições de contratos para acesso a dados
- **Regras de Negócio**: Invariantes e validações implementadas diretamente nas entidades

### 2. **Application** (Aplicação)
- **DTOs**: Objetos de transferência de dados para comunicação entre camadas
- **Serviços**: Casos de uso da aplicação (ex: `ColaboradorService`, `CheckInService`)
- **Validadores**: Validações usando FluentValidation

### 3. **Infrastructure** (Infraestrutura)
- **EF Core**: Configuração do `DbContext` e mapeamentos de entidades
- **Repositórios Concretos**: Implementações dos repositórios usando Entity Framework Core
- **Migrations**: Controle de versão do banco de dados

### 4. **API** (Apresentação)
- **Controllers**: Endpoints REST com CRUD completo
- **Swagger**: Documentação interativa da API
- **ProblemDetails**: Tratamento padronizado de erros

## Tecnologias Utilizadas

- **.NET 8.0**
- **Entity Framework Core 8.0** (SQL Server)
- **FluentValidation 11.9.0**
- **Swashbuckle.AspNetCore 6.5.0** (Swagger/OpenAPI)
- **Microsoft.AspNetCore.OpenApi 8.0.0**

## Pré-requisitos

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- SQL Server (LocalDB, Express ou Full) ou SQL Server em nuvem
- Visual Studio 2022, VS Code ou Rider (opcional)

## Como Executar

### 1. Clonar o Repositório

```bash
git clone <url-do-repositorio>
cd WorkBalanceHub
```

### 2. Configurar a String de Conexão

Edite o arquivo `src/WorkBalanceHub.API/appsettings.json` e ajuste a connection string conforme seu ambiente:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=WorkBalanceHub;Trusted_Connection=True;MultipleActiveResultSets=true"
  }
}
```

### 3. Aplicar Migrations

Execute os seguintes comandos na raiz do projeto:

```bash
# Navegar para o diretório da API
cd src/WorkBalanceHub.API

# Criar a migration inicial (se ainda não existir)
dotnet ef migrations add InitialCreate --project ../WorkBalanceHub.Infrastructure --startup-project .

# Aplicar as migrations ao banco de dados
dotnet ef database update --project ../WorkBalanceHub.Infrastructure --startup-project .
```

**Nota**: Se você ainda não tem o EF Core Tools instalado globalmente, instale com:

```bash
dotnet tool install --global dotnet-ef
```

### 4. Executar a Aplicação

```bash
# Ainda no diretório src/WorkBalanceHub.API
dotnet run
```

A API estará disponível em:
- **HTTP**: `http://localhost:5000`
- **HTTPS**: `https://localhost:5001`

### 5. Acessar o Swagger

Com a aplicação rodando, acesse:
- **Swagger UI**: `https://localhost:5001/swagger` (ou `http://localhost:5000/swagger`)

## Estrutura de Endpoints

### Equipes (`/api/equipes`)
- `POST /api/equipes` - Criar equipe
- `GET /api/equipes/{id}` - Obter equipe por ID
- `GET /api/equipes/search` - Buscar equipes (paginação, ordenação, filtros)
- `PUT /api/equipes/{id}` - Atualizar equipe
- `DELETE /api/equipes/{id}` - Desativar equipe

### Colaboradores (`/api/colaboradores`)
- `POST /api/colaboradores` - Criar colaborador
- `GET /api/colaboradores/{id}` - Obter colaborador por ID
- `GET /api/colaboradores/search` - Buscar colaboradores (paginação, ordenação, filtros)
- `GET /api/colaboradores/equipe/{equipeId}` - Obter colaboradores por equipe
- `PUT /api/colaboradores/{id}` - Atualizar colaborador
- `DELETE /api/colaboradores/{id}` - Desativar colaborador

### Check-ins (`/api/checkins`)
- `POST /api/checkins` - Criar check-in
- `GET /api/checkins/{id}` - Obter check-in por ID
- `GET /api/checkins/search` - Buscar check-ins (paginação, ordenação, filtros)
- `GET /api/checkins/colaborador/{colaboradorId}` - Obter check-ins por colaborador
- `GET /api/checkins/indicadores/equipe/{equipeId}` - Obter indicadores de bem-estar por equipe
- `PUT /api/checkins/{id}` - Atualizar check-in
- `DELETE /api/checkins/{id}` - Remover check-in

### Estações de Trabalho (`/api/estacoestrabalho`)
- `POST /api/estacoestrabalho` - Criar estação de trabalho
- `GET /api/estacoestrabalho/{id}` - Obter estação por ID
- `GET /api/estacoestrabalho/search` - Buscar estações (paginação, ordenação, filtros)
- `PUT /api/estacoestrabalho/{id}` - Atualizar estação
- `DELETE /api/estacoestrabalho/{id}` - Desativar estação

### Leituras de Ambiente (`/api/leiturasambiente`)
- `POST /api/leiturasambiente` - Criar leitura de ambiente
- `GET /api/leiturasambiente/{id}` - Obter leitura por ID
- `GET /api/leiturasambiente/search` - Buscar leituras (paginação, ordenação, filtros)
- `GET /api/leiturasambiente/estacao/{estacaoTrabalhoId}` - Obter leituras por estação
- `DELETE /api/leiturasambiente/{id}` - Remover leitura

### Planos de Ação (`/api/planosacao`)
- `POST /api/planosacao` - Criar plano de ação
- `GET /api/planosacao/{id}` - Obter plano por ID
- `GET /api/planosacao/search` - Buscar planos (paginação, ordenação, filtros)
- `GET /api/planosacao/equipe/{equipeId}` - Obter planos por equipe
- `PUT /api/planosacao/{id}` - Atualizar plano
- `PATCH /api/planosacao/{id}/status` - Alterar status do plano
- `DELETE /api/planosacao/{id}` - Remover plano

## Exemplos de Uso

### Criar uma Equipe

```bash
curl -X POST "https://localhost:5001/api/equipes" \
  -H "Content-Type: application/json" \
  -d '{
    "nome": "Desenvolvimento",
    "descricao": "Equipe de desenvolvimento de software"
  }'
```

### Criar um Colaborador

```bash
curl -X POST "https://localhost:5001/api/colaboradores" \
  -H "Content-Type: application/json" \
  -d '{
    "nome": "João Silva",
    "email": "joao.silva@empresa.com",
    "cargo": "Desenvolvedor",
    "equipeId": 1
  }'
```

### Criar um Check-in

```bash
curl -X POST "https://localhost:5001/api/checkins" \
  -H "Content-Type: application/json" \
  -d '{
    "colaboradorId": 1,
    "humor": 4,
    "nivelEstresse": 2,
    "qualidadeSono": 3,
    "sintomasFisicos": "Nenhum",
    "observacoes": "Dia produtivo"
  }'
```

### Buscar Colaboradores com Paginação e Filtros

```bash
curl "https://localhost:5001/api/colaboradores/search?pageNumber=1&pageSize=10&searchTerm=joao&sortBy=nome&sortDirection=asc"
```

### Obter Indicadores de Bem-estar

```bash
curl "https://localhost:5001/api/checkins/indicadores/equipe/1?dataInicio=2025-01-01&dataFim=2025-01-31"
```

## Decisões Arquiteturais

### 1. Clean Architecture
- Separação clara de responsabilidades entre camadas
- Dependências apontam para dentro (Domain não depende de nada)
- Facilita testes e manutenção

### 2. Domain-Driven Design (DDD)
- Entidades ricas com regras de negócio
- Invariantes protegidas por validações nas entidades
- Agregados bem definidos

### 3. Repository Pattern
- Abstração do acesso a dados
- Facilita testes unitários com mocks
- Permite trocar a implementação de persistência

### 4. DTOs (Data Transfer Objects)
- Separação entre modelo de domínio e modelo de apresentação
- Previne exposição acidental de dados sensíveis
- Facilita versionamento da API

### 5. FluentValidation
- Validações declarativas e reutilizáveis
- Mensagens de erro consistentes
- Validação no nível de aplicação

### 6. ProblemDetails
- Respostas de erro padronizadas (RFC 7807)
- Melhor experiência para consumidores da API
- Facilita debugging

### 7. Paginação e Ordenação
- Endpoints `/search` com suporte a:
  - Paginação (`pageNumber`, `pageSize`)
  - Ordenação (`sortBy`, `sortDirection`)
  - Filtros (`searchTerm`)

## Validações Implementadas

- **Colaborador**: Nome, e-mail válido, cargo obrigatório, equipe existente
- **Check-in**: Humor, estresse e sono entre 1-5, um check-in por dia por colaborador
- **Leitura Ambiente**: Temperatura (-50 a 60°C), ruído (0 a 150 dB), luminosidade (0 a 100.000 lux)
- **Plano de Ação**: Título, descrição, datas válidas, equipe existente

## Tratamento de Erros

A API utiliza `ProblemDetails` para retornar erros de forma padronizada:

```json
{
  "type": "https://tools.ietf.org/html/rfc7231#section-6.5.1",
  "title": "Dados inválidos",
  "status": 400,
  "detail": "O nome do colaborador é obrigatório."
}
```

## Migrations

### Criar uma Nova Migration

```bash
cd src/WorkBalanceHub.API
dotnet ef migrations add NomeDaMigration --project ../WorkBalanceHub.Infrastructure --startup-project .
```

### Aplicar Migrations

```bash
dotnet ef database update --project ../WorkBalanceHub.Infrastructure --startup-project .
```

### Reverter Migration

```bash
dotnet ef database update NomeDaMigrationAnterior --project ../WorkBalanceHub.Infrastructure --startup-project .
```

## Seed de Dados (Opcional)

Para popular o banco com dados iniciais, você pode criar um script ou usar o Swagger para criar registros manualmente. Exemplo de dados iniciais:

1. Criar equipes
2. Criar colaboradores
3. Criar estações de trabalho
4. Criar check-ins e leituras de ambiente

## Variáveis de Ambiente

Você pode configurar a connection string via variável de ambiente:

```bash
# Windows PowerShell
$env:ConnectionStrings__DefaultConnection="Server=...;Database=...;..."

# Linux/Mac
export ConnectionStrings__DefaultConnection="Server=...;Database=...;..."
```

Ou criar um arquivo `appsettings.Development.json` local (não versionado).

## Testes

Para executar testes (quando implementados):

```bash
dotnet test
```

## Próximos Passos

- [ ] Implementar autenticação e autorização (JWT)
- [ ] Adicionar testes unitários e de integração
- [ ] Implementar cache (Redis)
- [ ] Adicionar logging estruturado (Serilog)
- [ ] Implementar HATEOAS nos endpoints
- [ ] Adicionar suporte a CORS configurável
- [ ] Implementar rate limiting
- [ ] Adicionar health checks

## Contribuindo

1. Faça um fork do projeto
2. Crie uma branch para sua feature (`git checkout -b feature/MinhaFeature`)
3. Commit suas mudanças (`git commit -m 'Adiciona MinhaFeature'`)
4. Push para a branch (`git push origin feature/MinhaFeature`)
5. Abra um Pull Request

## Licença

Este projeto foi desenvolvido como parte do trabalho acadêmico da FIAP - Global Solution 2025.

## Autores

- **Juan Pablo Rebelo Coelho** - RM: 560445
- **Maria Eduarda Fernandes Rocha** - RM: 560657
- **Victor de Carvalho Alves** - RM: 560395

---

**FIAP - Análise e Desenvolvimento de Sistemas**  
**Global Solution 2025**

