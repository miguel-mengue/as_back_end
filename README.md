# API de Gerenciamento de Usuários - .NET 9

> 🎬 **[Assista à Demonstração Completa no YouTube](https://youtu.be/5xLaYenBfEo)**

---

## Visão Geral

API REST para gerenciamento de usuários desenvolvida em **C# com .NET 9**, seguindo a arquitetura em camadas (Clean Architecture). O projeto implementa operações CRUD completas com validações robustas, segurança, e padrões de design profissionais.

**Características:**
- ✅ 5 Endpoints RESTful (GET, POST, PUT, DELETE)
- ✅ Validações robustas com FluentValidation
- ✅ Hash seguro de senhas com BCrypt
- ✅ Email único no banco de dados
- ✅ Soft Delete (usuários não são removidos)
- ✅ Clean Architecture em 3 camadas
- ✅ Repository Pattern + Service Pattern
- ✅ DTOs para transferência de dados
- ✅ Banco de dados SQLite com Entity Framework Core

---

## 📁 Estrutura do Projeto

\`\`\`
APIUsuarios/
│
├── 📁 Domain/
│   └── 📁 Entities/
│       └── Usuario.cs                 # Entidade do domínio
│
├── 📁 Application/
│   ├── 📁 DTOs/
│   │   ├── UsuarioCreateDto.cs        # DTO para criação
│   │   ├── UsuarioReadDto.cs          # DTO para leitura
│   │   └── UsuarioUpdateDto.cs        # DTO para atualização
│   │
│   ├── 📁 Interfaces/
│   │   ├── IUsuarioRepository.cs       # Contrato do repositório
│   │   └── IUsuarioService.cs          # Contrato do serviço
│   │
│   ├── 📁 Services/
│   │   └── UsuarioService.cs           # Lógica de negócio
│   │
│   └── 📁 Validators/
│       ├── UsuarioCreateDtoValidator.cs    # Validações para criar
│       └── UsuarioUpdateDtoValidator.cs    # Validações para atualizar
│
├── 📁 Infrastructure/
│   ├── 📁 Persistence/
│   │   └── AppDbContext.cs             # Configuração do Entity Framework
│   │
│   └── 📁 Repositories/
│       └── UsuarioRepository.cs        # Implementação do repositório
│
├── 📁 Migrations/
│   ├── [timestamp]_InitialCreate.cs
│   └── UsuariosContextModelSnapshot.cs
│
├── 📄 Program.cs                       # Configuração da API e endpoints
├── 📄 appsettings.json                 # Configurações da aplicação
├── 📄 APIUsuarios.csproj               # Arquivo do projeto
├── 💾 usuarios.db                      # Banco de dados SQLite
├── 📄 README.md                        # Este arquivo
└── 📄 .gitignore                       # Arquivos ignorados pelo Git

\`\`\`

---

## 🛠 Tecnologias Utilizadas

| Tecnologia | Versão | Propósito |
|-----------|--------|----------|
| **.NET SDK** | 9.0+ | Runtime e SDK |
| **C#** | 12+ | Linguagem de programação |
| **ASP.NET Core** | 9.0 | Framework web |
| **Entity Framework Core** | 9.0 | ORM para acesso a dados |
| **SQLite** | Latest | Banco de dados relacional |
| **FluentValidation** | 11.x | Validação de dados |
| **BCrypt.Net-Next** | 4.x | Hash de senhas |

---

## 👤 Entidade Usuario

```csharp
public class Usuario
{
    public int Id { get; set; }                           // Identificador único
    public string Nome { get; set; }                      // Nome (3-100 caracteres)
    public string Email { get; set; }                     // Email único
    public string Senha { get; set; }                     // Hash da senha (BCrypt)
    public DateTime DataNascimento { get; set; }          // Data de nascimento
    public string Telefone { get; set; }                  // Telefone (opcional)
    public bool Ativo { get; set; } = true;              // Flag de ativo/deletado
    public DateTime DataCriacao { get; set; }             // Data de criação
    public DateTime? DataAtualizacao { get; set; }        // Data de última atualização
}