# 📚 API de Gerenciamento de Usuários - .NET 9

Uma API RESTful completa desenvolvida em **.NET 9** com **Clean Architecture**, implementando operações CRUD para gerenciamento de usuários com validações robustas, segurança e padrões de design profissionais.

## 🎬 Vídeo Demonstrativo

**[Assista a demonstração completa clicando aqui](https://youtu.be/5xLaYenBfEo)**

O vídeo contém:
- ✅ Apresentação e estrutura do projeto
- ✅ Explicação detalhada do código
- ✅ Demonstração prática de todos os 5 endpoints
- ✅ Testes de validação e tratamento de erros
- ✅ Verificação do banco de dados

---

## 📁 Estrutura do Projeto

APIUsuarios/

   Domain/
      Entities/
         Usuario.cs

   Application/
      DTOs/
         UsuarioCreateDto.cs
         UsuarioReadDto.cs
         UsuarioUpdateDto.cs
      Interfaces/
         IUsuarioRepository.cs
         IUsuarioService.cs
      Services/
         UsuarioService.cs
      Validators/
         UsuarioCreateDtoValidator.cs
         UsuarioUpdateDtoValidator.cs

   Infrastructure/
      Persistence/
         AppDbContext.cs
      Repositories/
=        UsuarioRepository.cs

   Migrations/
      [Migration files]

   Program.cs
   appsettings.json
   APIUsuarios.csproj
   usuarios.db
   README.md
   .gitignore

---

## 📋 Descrição das Camadas

### Domain/
Camada de domínio contendo a **entidade Usuario** com atributos de negócio:
- Id, Nome, Email, Senha, DataNascimento, Telefone, Ativo, DataCriacao, DataAtualizacao

### Application/
Camada de aplicação com:
- **DTOs**: Modelos de transferência de dados (Create, Read, Update)
- **Interfaces**: Contratos do Repository e Service
- **Services**: Lógica de negócio (orquestração)
- **Validators**: Regras de validação com FluentValidation

### Infrastructure/
Camada de infraestrutura com:
- **Persistence**: AppDbContext configurado para SQLite
- **Repositories**: Implementação de acesso a dados

---

## 🛠 Tecnologias Utilizadas

- **.NET SDK 9.0**
- **ASP.NET Core 9.0**
- **Entity Framework Core 9.0**
- **SQLite**
- **FluentValidation**
- **BCrypt.Net-Next**
- **Swagger/OpenAPI**

---

## ✅ Validações Implementadas

| Campo | Regras |
|-------|--------|
| Nome | Obrigatório, 3-100 caracteres |
| Email | Obrigatório, formato válido, ÚNICO |
| Senha | Obrigatório, mínimo 6 caracteres |
| DataNascimento | Obrigatório, idade >= 18 anos |
| Telefone | Opcional, formato (XX) XXXXX-XXXX |

---

## 🚀 Como Executar

### Pré-requisitos

- .NET SDK 9.0 ou superior
- Visual Studio Code ou Visual Studio 2022

### Passos

1. **Clonar o repositório**

   git clone https://github.com/miguel-mengue/api-usuarios.git
   cd APIUsuarios

3. **Restaurar pacotes**

   dotnet restore


4. **Criar banco de dados**

   dotnet ef migrations add InitialCreate
   dotnet ef database update


5. **Executar a API**

   dotnet run


A API estará disponível em:
- **HTTP**: `http://localhost:5150`
- **Swagger**: `http://localhost:5150/swagger`

---

## 📡 Endpoints da API

### 1. GET /usuarios
Retorna todos os usuários cadastrados.

**URL:** `http://localhost:5150/usuarios`

**Resposta esperada (200 OK):**

[
  {
    "id": 1,
    "nome": "João Silva",
    "email": "joao@email.com",
    "dataNascimento": "2000-01-15",
    "telefone": "(11) 98765-4321",
    "ativo": true,
    "dataCriacao": "2024-12-02T20:30:00Z"
  }
]


---

### 2. GET /usuarios/{id}
Retorna um usuário específico por ID.

**URL:** `http://localhost:5150/usuarios/1`

**Resposta esperada (200 OK):**

{
  "id": 1,
  "nome": "João Silva",
  "email": "joao@email.com",
  "dataNascimento": "2000-01-15",
  "telefone": "(11) 98765-4321",
  "ativo": true,
  "dataCriacao": "2024-12-02T20:30:00Z"
}


**Resposta (404 Not Found):**

{
  "mensagem": "Usuário não encontrado"
}


---

### 3. POST /usuarios
Cria um novo usuário.

**URL:** `http://localhost:5150/usuarios`

**Corpo da requisição:**

{
  "nome": "João Silva",
  "email": "joao@email.com",
  "senha": "senha123",
  "dataNascimento": "2000-01-15",
  "telefone": "(11) 98765-4321"
}


**Resposta esperada (201 Created):**

{
  "id": 1,
  "nome": "João Silva",
  "email": "joao@email.com",
  "dataNascimento": "2000-01-15",
  "telefone": "(11) 98765-4321",
  "ativo": true,
  "dataCriacao": "2024-12-02T20:30:00Z"
}


**Resposta (400 Bad Request) - Email Duplicado:**
\`\`\`json
{
  "mensagem": "Este email já está cadastrado"
}
\`\`\`

---

### 4. PUT /usuarios/{id}
Atualiza um usuário existente.

**URL:** `http://localhost:5150/usuarios/1`

**Corpo da requisição:**
\`\`\`json
{
  "nome": "João Silva Atualizado",
  "email": "joao.novo@email.com",
  "dataNascimento": "2000-01-15",
  "telefone": "(11) 99999-8888",
  "ativo": true
}
\`\`\`

**Resposta esperada (200 OK):**
\`\`\`json
{
  "id": 1,
  "nome": "João Silva Atualizado",
  "email": "joao.novo@email.com",
  "dataNascimento": "2000-01-15",
  "telefone": "(11) 99999-8888",
  "ativo": true,
  "dataCriacao": "2024-12-02T20:30:00Z"
}
\`\`\`

---

### 5. DELETE /usuarios/{id}
Marca um usuário como inativo (Soft Delete).

**URL:** `http://localhost:5150/usuarios/1`

**Resposta esperada (204 No Content):**

(sem corpo de resposta)


---

## 📊 Tabela de Respostas HTTP

| Endpoint | Sucesso | Não Encontrado | Erro Validação |
|----------|---------|----------------|-----------------|
| GET /usuarios | 200 | - | - |
| GET /usuarios/{id} | 200 | 404 | - |
| POST /usuarios | 201 | - | 400 |
| PUT /usuarios/{id} | 200 | 404 | 400 |
| DELETE /usuarios/{id} | 204 | 404 | - |

---

## 🏗 Padrões de Design

- **Repository Pattern**: Abstração de acesso a dados
- **Service Pattern**: Orquestração de lógica de negócio
- **DTO Pattern**: Separação entre dados internos e expostos
- **Dependency Injection**: Injeção de dependências via IoC Container
- **Soft Delete**: Marca como inativo ao invés de remover

---

## 🔐 Segurança Implementada

- **Hash de Senha**: BCrypt com salt automático
- **Email Único**: Validação e índice no banco
- **Validações Robustas**: Regras de negócio encapsuladas
- **Soft Delete**: Histórico preservado
- **DTOs**: Dados sensíveis não expostos

---

## 🐛 Troubleshooting

**Erro: "UNIQUE constraint failed: Usuarios.Email"**
- Use um email único: `novo@email.com`

**Erro: "An error occurred while saving the entity changes"**
- Verifique se os dados cumprem as restrições

**API não inicia**

taskkill /F /IM dotnet.exe
dotnet run


---

## 📚 Referências

- [ASP.NET Core Documentation](https://docs.microsoft.com/aspnet/core)
- [Entity Framework Core](https://docs.microsoft.com/ef/core)
- [FluentValidation](https://docs.fluentvalidation.net)
- Clean Architecture - Robert C. Martin

---

## 👨‍💼 Autor

**Nome**: Miguel Mengue

**Curso**: Analise e Desenvolvimento de sistemas
**Instituição**: ULBRA 
**Período**: 2025/2

---

*Desenvolvido como trabalho final de Desenvolvimento Backend*
