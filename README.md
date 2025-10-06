# People Registry — .NET + React (Desafio Técnico Stefanini)

API e (opcional) front-end para **cadastro de pessoas** com autenticação **JWT**, versionamento de API (**v1/v2**), validações e documentação **Swagger**.
Projeto focado em **back-end .NET** (6+). Implementado e testado em **.NET 9 / EF Core 9**.

---

## Sumário

* [Arquitetura](#arquitetura)
* [Tecnologias](#tecnologias)
* [Requisitos](#requisitos)
* [Como rodar (Back-end)](#como-rodar-back-end)
* [Seed de usuário Admin](#seed-de-usuário-admin)
* [Autenticação & Autorização](#autenticação--autorização)
* [Versionamento (v1 e v2)](#versionamento-v1-e-v2)
* [Modelos (v1 vs v2)](#modelos-v1-vs-v2)
* [Endpoints principais](#endpoints-principais)
* [Swagger](#swagger)
* [Variáveis de ambiente](#variáveis-de-ambiente)
* [Banco de Dados](#banco-de-dados)
* [Testes](#testes)
* [Estrutura do projeto](#estrutura-do-projeto)
* [Troubleshooting](#troubleshooting)
* [Roadmap curto](#roadmap-curto)

---

## Arquitetura

Camadas inspiradas em **Clean Architecture**:

* **Api**: Controllers, Swagger, versão da API, autenticação/autorizações.
* **Application**: *Use cases*, validações, mapeamentos.
* **Domain**: Entidades, enums, contratos (interfaces), exceções.
* **Infrastructure**: EF Core (DbContext/Migrations/Repos), segurança (hash/JWT).

> Dependências “de fora para dentro”: Api → Application → Domain.
> Infrastructure implementa interfaces do Domain e é injetada na composição.

---

## Tecnologias

* **.NET** 6+ (testado em **.NET 9**)
* **ASP.NET Core** Web API
* **Entity Framework Core** 9 (SQLite por padrão)
* **FluentValidation**
* **JWT** (Bearer) com **roles** (Admin/User)
* **Swagger / Swashbuckle**
* **AutoMapper**

> **Observação H2**: o enunciado cita H2 (muito comum em Java). Em .NET não há provider oficial EF Core para H2; usamos **SQLite** (equivalente leve para dev). Alternativa: PostgreSQL/SQL Server (ver seção “Banco de Dados”).

---

## Requisitos

* **SDK .NET** 8 ou 9 (recomendado)
* **Node.js** 18+ (apenas se for rodar o front-end)
* **SQLite** (instalado ou via binários do EF)

---

## Como rodar (Back-end)

```bash
# na raiz do repositório
dotnet restore
dotnet build
dotnet run --project src/PeopleRegistry.Api/PeopleRegistry.Api.csproj
```

Por padrão a API sobe em:

```
https://localhost:7004
http://localhost:5234
```

> Na primeira execução as **migrations** são aplicadas e um **usuário Admin é criado automaticamente** (veja [Seed](#seed-de-usuário-admin)).

---

## Seed de usuário Admin

Na inicialização, se não houver usuários, é criado:

* **Email**: `admin@company.com`
* **Senha**: `Admin@123`
* **Role**: `Admin`

Estes valores podem ser alterados por **variáveis de ambiente** (ver [Variáveis de ambiente](#variáveis-de-ambiente)).

---

## Autenticação & Autorização

* **Login** via `/api/auth/login` retorna **accessToken** (JWT) e, opcionalmente, **refreshToken**.
* **Todos os endpoints de Pessoas exigem JWT**.
* **Cadastro de usuários** está **restrito a Admin** (`[Authorize(Roles = "Admin")]`).

Fluxo básico (via cURL):

```bash
# 1) Login como Admin
curl -X POST https://localhost:7004/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{ "email":"admin@company.com", "password":"Admin@123" }'

# 2) Use o token no Authorization (Bearer <token>)
curl https://localhost:7004/api/v1/people \
  -H "Authorization: Bearer <accessToken>"
```

---

## Versionamento (v1 e v2)

* **v1**: campos básicos de Pessoa (CPF único, etc.).
* **v2**: **endereço obrigatório** (Street, Number, City, State, ZipCode) — mantendo a v1 funcional.

Rotas base:

```
v1: /api/v1/people
v2: /api/v2/people
```

---

## Modelos (v1 vs v2)

### v1 — `Person`

* **Name** (obrigatório)
* **Cpf** (obrigatório, único, 11 dígitos)
* **DateOfBirth** (obrigatório, < hoje)
* **Email** (opcional, válido se informado)
* **Gender** (opcional: Male/Female/Other)
* **PlaceOfBirth** (opcional)
* **Nationality** (opcional)
* **CreatedAt / UpdatedAt** (automáticos)

### v2 — `Person` + `Address` (obrigatório)

Além dos campos v1, endereço obrigatório:

* **Street**, **Number**, **City**, **State**, **ZipCode**

---

## Endpoints principais

### Auth

* `POST /api/auth/login` → `{ accessToken, refreshToken? }`
* `POST /api/auth/register` *(**apenas Admin**)* → cria usuário **Role=User**

### Pessoas (v1)

* `GET /api/v1/people` → lista (pode-se estender com `page`, `size`, `q`)
* `GET /api/v1/people/{id}`
* `POST /api/v1/people` → cria pessoa v1
* `PUT /api/v1/people/{id}` → atualiza
* `DELETE /api/v1/people/{id}` → remove

### Pessoas (v2)

* `POST /api/v2/people` → cria pessoa **com Address obrigatório**
* Demais operações idem v1 (podem compartilhar implementação)

**Exemplo `POST /api/v2/people`**:

```json
{
  "name": "Maria Silva",
  "cpf": "12345678901",
  "dateOfBirth": "1990-05-10",
  "email": "maria@exemplo.com",
  "gender": "Female",
  "placeOfBirth": "São Paulo",
  "nationality": "Brasileira",
  "address": {
    "street": "Av. Paulista",
    "number": "1000",
    "city": "São Paulo",
    "state": "SP",
    "zipCode": "01310-100"
  }
}
```

---

## Swagger

Disponível em:

```
https://localhost:7004/swagger
```

* Documentação separada por **versão** (v1 e v2).
* **Authorize** (canto superior direito) para testar endpoints com JWT.

---

## Variáveis de ambiente

Exemplo `appsettings.json` (parcial):

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=PeopleRegistry.db"
  },
  "Jwt": {
    "SigningKey": "coloque-uma-chave-com-32+ bytes",
    "ExpirationTimeMinutes": "60"
  },
  "Seed": {
    "AdminEmail": "admin@company.com",
    "AdminPassword": "Admin@123"
  }
}
```

Override por **env vars** (Windows PowerShell):

```powershell
$env:Jwt__SigningKey="sua-chave-bem-grande-aqui-...-32+bytes"
$env:Seed__AdminEmail="admin@company.com"
$env:Seed__AdminPassword="Admin@123"
```

> **Importante**: `Jwt:SigningKey` deve ter **32+ bytes** para HS256.

---

## Banco de Dados

* **Padrão: SQLite** (`PeopleRegistry.db` na raiz do projeto API).
* Migrations automáticas na inicialização.

Comandos úteis:

```bash
# criar nova migration
dotnet ef migrations add NomeDaMigration \
  --project src/PeopleRegistry.Infrastructure/PeopleRegistry.Infrastructure.csproj \
  --startup-project src/PeopleRegistry.Api/PeopleRegistry.Api.csproj

# aplicar
dotnet ef database update \
  --project src/PeopleRegistry.Infrastructure/PeopleRegistry.Infrastructure.csproj \
  --startup-project src/PeopleRegistry.Api/PeopleRegistry.Api.csproj
```

> **Sobre H2**: caso o avaliador exija estritamente H2, alinhar alternativa (ex.: PostgreSQL). Para Postgres, basta trocar o provider no `Program.cs` e a **connection string**.

---

## Testes

> Meta do desafio: **XUnit** com **≥80%**.
> Sugeridos (unit/integration):

* **Unit**: `PersonValidator`, `RegisterPersonUseCase`, `LoginUseCase` (mocks de repositórios/cripto/token).
* **Integração**: `WebApplicationFactory`, banco em memória/SQLite temporário, cenários:

  * CRUD v1/v2 feliz/triste
  * Login inválido / válido
  * Endpoint `/auth/register` retorna **403** para não-admin
  * CPF duplicado → 400
  * Data de nascimento futura → 400

Execução:

```bash
dotnet test
```

---

## Estrutura do projeto

```
src/
  PeopleRegistry.Api/                 # camada Web (controllers, Program, Swagger, versioning)
  PeopleRegistry.Application/         # use cases, validators, AutoMapper
  PeopleRegistry.Domain/              # entidades, enums, interfaces, exceções
  PeopleRegistry.Infrastructure/      # EF Core, DbContext, migrations, repos, security (JWT/BCrypt)
  PeopleRegistry.Communication/       # DTOs (Requests/Responses)
```

---

## Troubleshooting

* **500 ao gerar JWT / IDX10720**
  A chave `Jwt:SigningKey` é curta. Use **32+ bytes**.

* **401 em endpoints protegidos**
  Envie `Authorization: Bearer <accessToken>` (sem aspas) e confira se o token não expirou.

* **403 no `/api/auth/register`**
  Apenas **Admin** pode registrar. Faça login com o usuário seed.

* **Migrations não aplicam**
  Garanta que rodou a API ao menos uma vez; o `Program.cs` aplica `db.Database.Migrate()` na inicialização.

---

## Roadmap curto

* Paginação/ordenação/filtro em `GET /people`.
* ETags (concurrency) e idempotência de POST.
* Fluxo completo de **refresh token** (rotação, revogação, detecção de reuso).
* Testes automatizados (xUnit + cobertura).
* Dockerfile/Compose e deploy (Render/Azure/Fly.io) + front em Vercel/Netlify.

---

**Autor:** *Gabriel André* — *Desafio Técnico Stefanini (.NET + React).*
Sinta-se à vontade para abrir *issues/PRs* com sugestões ou melhorias.
