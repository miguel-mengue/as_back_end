using Microsoft.EntityFrameworkCore;
using FluentValidation;
using APIUsuarios.Application.Interfaces;
using APIUsuarios.Application.Services;
using APIUsuarios.Application.DTOs;
using APIUsuarios.Infrastructure.Persistence;
using APIUsuarios.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Configurar banco de dados SQLite
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") 
    ?? "Data Source=usuarios.db";

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(connectionString)
);

// Registrar Repository e Service (Dependency Injection)
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<IUsuarioService, UsuarioService>();

// CORS (permitir requisições do frontend)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowAll");

// ============ ENDPOINTS ============

// GET: Listar todos os usuários
app.MapGet("/usuarios", async (IUsuarioService service, CancellationToken ct) =>
{
    try
    {
        var usuarios = await service.ListarAsync(ct);
        return Results.Ok(usuarios);
    }
    catch (Exception ex)
    {
        Console.WriteLine($"[ERROR] GET /usuarios: {ex.Message}");
        Console.WriteLine($"[ERROR] Stack Trace: {ex.StackTrace}");
        if (ex.InnerException != null)
            Console.WriteLine($"[ERROR] Inner Exception: {ex.InnerException.Message}");
        return Results.BadRequest(new { mensagem = $"Erro ao listar usuários: {ex.Message}" });
    }
});

// GET: Buscar usuário por ID
app.MapGet("/usuarios/{id}", async (int id, IUsuarioService service, CancellationToken ct) =>
{
    try
    {
        var usuario = await service.ObterAsync(id, ct);
        if (usuario is null)
            return Results.NotFound(new { mensagem = "Usuário não encontrado" });
        
        return Results.Ok(usuario);
    }
    catch (Exception ex)
    {
        Console.WriteLine($"[ERROR] GET /usuarios/{id}: {ex.Message}");
        return Results.BadRequest(new { mensagem = ex.Message });
    }
});

// POST: Criar novo usuário
app.MapPost("/usuarios", async (UsuarioCreateDto dto, IUsuarioService service, CancellationToken ct) =>
{
    try
    {
        var usuario = await service.CriarAsync(dto, ct);
        return Results.Created($"/usuarios/{usuario.Id}", usuario);
    }
    catch (Exception ex)
    {
        Console.WriteLine($"[ERROR] POST /usuarios: {ex.Message}");
        Console.WriteLine($"[ERROR] Stack Trace: {ex.StackTrace}");
        if (ex.InnerException != null)
            Console.WriteLine($"[ERROR] Inner Exception: {ex.InnerException.Message}");
        return Results.BadRequest(new { mensagem = $"Erro ao criar usuário: {ex.Message}" });
    }
});

// PUT: Atualizar usuário
app.MapPut("/usuarios/{id}", async (int id, UsuarioUpdateDto dto, IUsuarioService service, CancellationToken ct) =>
{
    try
    {
        var usuario = await service.AtualizarAsync(id, dto, ct);
        return Results.Ok(usuario);
    }
    catch (KeyNotFoundException)
    {
        return Results.NotFound(new { mensagem = "Usuário não encontrado" });
    }
    catch (Exception ex)
    {
        Console.WriteLine($"[ERROR] PUT /usuarios/{id}: {ex.Message}");
        Console.WriteLine($"[ERROR] Stack Trace: {ex.StackTrace}");
        if (ex.InnerException != null)
            Console.WriteLine($"[ERROR] Inner Exception: {ex.InnerException.Message}");
        return Results.BadRequest(new { mensagem = $"Erro ao atualizar usuário: {ex.Message}" });
    }
});

// DELETE: Remover usuário (Soft Delete)
app.MapDelete("/usuarios/{id}", async (int id, IUsuarioService service, CancellationToken ct) =>
{
    try
    {
        await service.RemoverAsync(id, ct);
        return Results.NoContent();
    }
    catch (KeyNotFoundException)
    {
        return Results.NotFound(new { mensagem = "Usuário não encontrado" });
    }
    catch (Exception ex)
    {
        Console.WriteLine($"[ERROR] DELETE /usuarios/{id}: {ex.Message}");
        if (ex.InnerException != null)
            Console.WriteLine($"[ERROR] Inner Exception: {ex.InnerException.Message}");
        return Results.BadRequest(new { mensagem = ex.Message });
    }
});

app.Run();