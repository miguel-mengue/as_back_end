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

builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<IUsuarioService, UsuarioService>();

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

// ENDPOINTS

// GET: Listar todos os usuários
app.MapGet("/usuarios", async (IUsuarioService service, CancellationToken ct) =>
{
    var usuarios = await service.ListarAsync(ct);
    return Results.Ok(usuarios);
})
.WithName("ListarUsuarios")
.Produces(StatusCodes.Status200OK);

// GET: Buscar usuário por ID
app.MapGet("/usuarios/{id}", async (int id, IUsuarioService service, CancellationToken ct) =>
{
    var usuario = await service.ObterAsync(id, ct);
    if (usuario is null)
        return Results.NotFound(new { mensagem = "Usuário não encontrado" });
    
    return Results.Ok(usuario);
})
.WithName("ObterUsuario")
.Produces(StatusCodes.Status200OK)
.Produces(StatusCodes.Status404NotFound);

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
        return Results.BadRequest(new { mensagem = ex.Message });
    }
})
.WithName("CriarUsuario")
.Accepts<UsuarioCreateDto>("application/json")
.Produces(StatusCodes.Status201Created)
.Produces(StatusCodes.Status400BadRequest)
.Produces(StatusCodes.Status409Conflict);

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
        return Results.BadRequest(new { mensagem = ex.Message });
    }
})
.WithName("AtualizarUsuario")
.Accepts<UsuarioUpdateDto>("application/json")
.Produces(StatusCodes.Status200OK)
.Produces(StatusCodes.Status400BadRequest)
.Produces(StatusCodes.Status404NotFound);

// DELETE: Remover usuário
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
})
.WithName("RemoverUsuario")
.Produces(StatusCodes.Status204NoContent)
.Produces(StatusCodes.Status404NotFound);

app.Run();
