# API de Gerenciamento de Usuários

## 📺 Vídeo Demonstrativo

**Assista à demonstração completa da API:**
[API de Gerenciamento de Usuários - Vídeo Demonstrativo](https://youtu.be/5xLaYenBfEo)

---

## 📋 Descrição do Projeto

Esta é uma **API REST completa de Gerenciamento de Usuários** desenvolvida em **.NET 9** seguindo os princípios de **Clean Architecture** e padrões de design profissionais.

A aplicação implementa operações CRUD (Create, Read, Update, Delete) com validações robustas, segurança na armazenagem de senhas através de hash BCrypt, e adota padrões de projeto reconhecidos como Repository Pattern, Service Pattern e DTO Pattern.

O projeto foi desenvolvido como Avaliação Semestral (AS) da disciplina de Desenvolvimento Backend e demonstra a aplicação prática de conceitos acadêmicos em um cenário real de desenvolvimento de software.

---

## 🎯 Objetivos

- Implementar uma API REST completa utilizando ASP.NET Core com Minimal APIs
- Aplicar padrões de projeto em um contexto real
- Estruturar código seguindo princípios de Clean Architecture
- Persistir dados utilizando Entity Framework Core
- Validar entrada de dados com FluentValidation
- Documentar decisões técnicas de forma acadêmica
- Apresentar soluções técnicas de forma clara e objetiva

---

## 🛠 Tecnologias Utilizadas

| Tecnologia | Versão | Descrição |
|------------|--------|-----------|
| **.NET** | 9.0 | Plataforma de desenvolvimento |
| **C#** | 12.0 | Linguagem de programação |
| **ASP.NET Core** | 9.0 | Framework web |
| **Entity Framework Core** | 9.0+ | ORM para acesso a dados |
| **SQLite** | Integrado | Banco de dados relacional |
| **FluentValidation.AspNetCore** | 11.3+ | Validação de dados |
| **BCrypt.Net-Next** | 4.0+ | Hash seguro de senhas |

---

## 🏗 Padrões de Projeto Implementados

### 1. Repository Pattern
- **Interface**: `IUsuarioRepository`
- **Implementação**: `UsuarioRepository`
- **Responsabilidade**: Abstração da camada de persistência, isolando a lógica de acesso a dados

### 2. Service Pattern
- **Interface**: `IUsuarioService`
- **Implementação**: `UsuarioService`
- **Responsabilidade**: Orquestração da lógica de negócio e aplicação de regras

### 3. DTO Pattern
- **UsuarioCreateDto**: Modelo para criação de usuários
- **UsuarioReadDto**: Modelo para leitura de dados (sem senha)
- **UsuarioUpdateDto**: Modelo para atualização de usuários

### 4. Dependency Injection
- Configuração no `Program.cs` para injetar dependências automaticamente
- Ciclo de vida: `AddScoped` para Repository e Service

### 5. FluentValidation
- Validadores específicos para cada operação
- Regras de negócio encapsuladas em classes dedicadas

---

## 📁 Estrutura do Projeto

\`\`\`
APIUsuarios/
├── Domain/
│   └── Entities/
│       └── Usuario.cs                          # Entidade de domínio
│
├── Application/
│   ├── DTOs/
│   │   ├── UsuarioCreateDto.cs                # DTO para criação
│   │   ├── UsuarioReadDto.cs                  # DTO para leitura
│   │   └── UsuarioUpdateDto.cs                # DTO para atualização
│   │
│   ├── Interfaces/
│   │   ├── IUsuarioRepository.cs               # Contrato do repository
│   │   └── IUsuarioService.cs                  # Contrato do service
│   │
│   ├── Services/
│   │   └── UsuarioService.cs                   # Lógica de negócio
│   │
│   └── Validators/
│       ├── UsuarioCreateDtoValidator.cs        # Validação para create
│       └── UsuarioUpdateDtoValidator.cs        # Validação para update
│
├── Infrastructure/
│   ├── Persistence/
│   │   └── AppDbContext.cs                     # Configuração EF Core
│   │
│   └── Repositories/
│       └── UsuarioRepository.cs                # Implementação repository
│
├── Migrations/                                 # Migrations do EF Core (geradas automaticamente)
├── Program.cs                                  # Configuração da API e endpoints
├── appsettings.json                            # Configurações
├── APIUsuarios.csproj                          # Arquivo de projeto
└── usuarios.db                                 # Banco de dados SQLite
\`\`\`

---

## 📦 Entidade Usuario

```csharp
public class Usuario
{
    public int Id { get; set; }                    // PK, Auto-increment
    public string Nome { get; set; }               // Obrigatório, 3-100 caracteres
    public string Email { get; set; }              // Obrigatório, único
    public string Senha { get; set; }              // Obrigatório, hash BCrypt
    public DateTime DataNascimento { get; set; }   // Obrigatório, >= 18 anos
    public string? Telefone { get; set; }          // Opcional, formato (XX) XXXXX-XXXX
    public bool Ativo { get; set; }                // Padrão: true
    public DateTime DataCriacao { get; set; }      // Auto-preenchido
    public DateTime? DataAtualizacao { get; set; } // Auto-atualizado
}