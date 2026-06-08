# SatAlert API (.NET) — Global Solution FIAP 2026

API REST de gerenciamento de usuários e notificações de alerta do sistema SatAlert.

## Integrantes do Grupo

| Nome | RM | Turma |
|---|---|---|
| Andrei de Paiva Gibbini | 563061 | 2TDSPF |
| Arthur Câmara | 562310 | 2TDSPG |
| Diogo Cunha | 563654 | 2TDSPF |
| Pedro Santos Pequini | 561842 | 2TDSPF |
| Pedro Sakai Silva Zambaca | 565956 | 2TDSPF |

## Links

- **Vídeo demonstração:** *(em breve)*
- **Vídeo pitch:** *(em breve)*

## Sobre o Projeto

O SatAlert é um sistema de alertas baseado em dados satelitais. Esta API (.NET) é responsável pela gestão de usuários e pelo registro de notificações enviadas a eles. Quando a API Java detecta um novo alerta crítico, ela chama esta API para registrar a notificação e marcar quais usuários foram avisados.

## Arquitetura

Clean Architecture com 4 projetos:

```
SatAlert.sln
├── SatAlert.Domain         → Entidades (sem dependência externa)
├── SatAlert.Application    → DTOs e contratos
├── SatAlert.Infrastructure → DbContext (EF Core), Repositórios, Migrations
└── SatAlert.API            → Controllers, Program.cs
```

## Diagrama de Entidades

```
┌─────────────────────────────────┐       ┌───────────────────────────────────┐
│            Usuario               │       │           Notificacao              │
├─────────────────────────────────┤       ├───────────────────────────────────┤
│ Id              : Guid (PK)     │       │ Id              : Guid (PK)       │
│ Nome            : string        │  1:N  │ UsuarioId       : Guid (FK)       │
│ Email           : string        │──────►│ Mensagem        : string          │
│ Telefone        : string        │       │ DataEnvio       : DateTime        │
│ RegiaoInteresse : string        │       │ Lida            : bool            │
│ Ativo           : bool          │       └───────────────────────────────────┘
│ CriadoEm        : DateTime      │
└─────────────────────────────────┘
```

Relacionamento: um `Usuario` pode ter várias `Notificacoes` (1:N). Ao deletar um usuário, todas as suas notificações são removidas em cascade.

## Tecnologias

| Camada | Tecnologia |
|---|---|
| Framework | ASP.NET Core 9 |
| ORM | Entity Framework Core 9 |
| Banco | Oracle XE — Oracle.EntityFrameworkCore 9.23.60 |
| Documentação | Swagger / OpenAPI |

## Como Executar

### Pré-requisitos

- .NET 9 SDK + ASP.NET Core Runtime 9
- `dotnet tool install --global dotnet-ef --version 9.*`

### Configuração do banco

Edite `SatAlert.API/appsettings.json` com suas credenciais:

```json
{
  "ConnectionStrings": {
    "Oracle": "Data Source=oracle.fiap.com.br:1521/orcl;User ID=<SEU_RM>;Password=<SUA_SENHA>;"
  }
}
```

### Aplicar migrations

```powershell
$env:PATH = "C:\Program Files\dotnet;" + $env:PATH + ";$env:USERPROFILE\.dotnet\tools"
dotnet ef database update --project SatAlert.Infrastructure --startup-project SatAlert.API
```

### Rodar a API

```bash
cd SatAlert.API
dotnet run
```

Swagger disponível em: `http://localhost:5000`

## Endpoints

### Usuários `/api/Usuarios`

| Método | Rota | Descrição | Status |
|---|---|---|---|
| GET | /api/Usuarios | Lista todos | 200 |
| GET | /api/Usuarios/{id} | Busca por Id | 200 / 404 |
| POST | /api/Usuarios | Cria usuário | 201 / 400 |
| PUT | /api/Usuarios/{id} | Atualiza usuário | 200 / 400 / 404 |
| DELETE | /api/Usuarios/{id} | Remove usuário (cascade) | 204 / 404 |

### Notificações `/api/Notificacoes`

| Método | Rota | Descrição | Status |
|---|---|---|---|
| GET | /api/Notificacoes/usuario/{usuarioId} | Notificações do usuário | 200 / 404 |
| POST | /api/Notificacoes | Registra notificação | 201 / 400 / 404 |
| PUT | /api/Notificacoes/{id}/marcar-lida | Marca como lida | 200 / 404 |

## Exemplos de Request/Response

### POST /api/Usuarios

**Request:**
```json
{
  "nome": "João Silva",
  "email": "joao@email.com",
  "telefone": "11999999999",
  "regiaoInteresse": "São Paulo - SP"
}
```

**Response 201:**
```json
{
  "id": "903b8a95-2f26-4b3f-88bd-c88efe713f0e",
  "nome": "João Silva",
  "email": "joao@email.com",
  "telefone": "11999999999",
  "regiaoInteresse": "São Paulo - SP",
  "ativo": true,
  "criadoEm": "2026-05-29T17:36:14.578039"
}
```

### POST /api/Notificacoes

**Request:**
```json
{
  "usuarioId": "903b8a95-2f26-4b3f-88bd-c88efe713f0e",
  "mensagem": "Alerta crítico detectado na região São Paulo - SP: risco de alagamento."
}
```

**Response 201:**
```json
{
  "id": "f0c28820-5ad6-485a-b71a-2cf906356e4d",
  "usuarioId": "903b8a95-2f26-4b3f-88bd-c88efe713f0e",
  "mensagem": "Alerta crítico detectado na região São Paulo - SP: risco de alagamento.",
  "dataEnvio": "2026-05-30T00:00:01.352241",
  "lida": false
}
```

### PUT /api/Notificacoes/{id}/marcar-lida

Sem body. Retorna a notificação com `"lida": true`:

```json
{
  "id": "f0c28820-5ad6-485a-b71a-2cf906356e4d",
  "usuarioId": "903b8a95-2f26-4b3f-88bd-c88efe713f0e",
  "mensagem": "Alerta crítico detectado na região São Paulo - SP: risco de alagamento.",
  "dataEnvio": "2026-05-30T00:00:01.352241",
  "lida": true
}
```

### Tratamento de erros

Entrada inválida retorna 400:
```json
{ "erro": "E-mail já cadastrado." }
```

Recurso não encontrado retorna 404:
```json
{ "erro": "Usuário não encontrado." }
```