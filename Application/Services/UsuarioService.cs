using APIUsuarios.Application.DTOs;
using APIUsuarios.Application.Interfaces;
using APIUsuarios.Domain.Entities;

namespace APIUsuarios.Application.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IUsuarioRepository _usuarioRepository;

        public UsuarioService(IUsuarioRepository usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
        }

        public async Task<IEnumerable<UsuarioReadDto>> ListarAsync(CancellationToken ct)
        {
            var usuarios = await _usuarioRepository.GetAllAsync(ct);
            return usuarios.Select(MapToReadDto);
        }

        public async Task<UsuarioReadDto?> ObterAsync(int id, CancellationToken ct)
        {
            var usuario = await _usuarioRepository.GetByIdAsync(id, ct);
            return usuario is null ? null : MapToReadDto(usuario);
        }

        public async Task<UsuarioReadDto> CriarAsync(UsuarioCreateDto dto, CancellationToken ct)
        {
            var usuario = new Usuario
            {
                Nome = dto.Nome,
                Email = dto.Email.ToLower(),
                Senha = BCrypt.Net.BCrypt.HashPassword(dto.Senha),
                DataNascimento = dto.DataNascimento,
                Telefone = dto.Telefone,
                Ativo = true,
                DataCriacao = DateTime.UtcNow
            };

            await _usuarioRepository.AddAsync(usuario, ct);
            await _usuarioRepository.SaveChangesAsync(ct);

            return MapToReadDto(usuario);
        }

        public async Task<UsuarioReadDto> AtualizarAsync(int id, UsuarioUpdateDto dto, CancellationToken ct)
        {
            var usuario = await _usuarioRepository.GetByIdAsync(id, ct);
            
            if (usuario is null)
                throw new KeyNotFoundException($"Usuário com ID {id} não encontrado");

            usuario.Nome = dto.Nome;
            usuario.Email = dto.Email.ToLower();
            usuario.DataNascimento = dto.DataNascimento;
            usuario.Telefone = dto.Telefone;
            usuario.Ativo = dto.Ativo;
            usuario.DataAtualizacao = DateTime.UtcNow;

            await _usuarioRepository.UpdateAsync(usuario, ct);
            await _usuarioRepository.SaveChangesAsync(ct);

            return MapToReadDto(usuario);
        }

        public async Task<bool> RemoverAsync(int id, CancellationToken ct)
        {
            var usuario = await _usuarioRepository.GetByIdAsync(id, ct);
            
            if (usuario is null)
                throw new KeyNotFoundException($"Usuário com ID {id} não encontrado");

            usuario.Ativo = false;
            usuario.DataAtualizacao = DateTime.UtcNow;

            await _usuarioRepository.UpdateAsync(usuario, ct);
            await _usuarioRepository.SaveChangesAsync(ct);

            return true;
        }

        public async Task<bool> EmailJaCadastradoAsync(string email, CancellationToken ct)
        {
            return await _usuarioRepository.EmailExistsAsync(email.ToLower(), ct);
        }

        private static UsuarioReadDto MapToReadDto(Usuario usuario)
        {
            return new UsuarioReadDto(
                usuario.Id,
                usuario.Nome,
                usuario.Email,
                usuario.DataNascimento,
                usuario.Telefone,
                usuario.Ativo,
                usuario.DataCriacao
            );
        }
    }
}
