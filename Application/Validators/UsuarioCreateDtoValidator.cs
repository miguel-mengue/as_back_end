using FluentValidation;
using APIUsuarios.Application.DTOs;
using APIUsuarios.Application.Interfaces;

namespace APIUsuarios.Application.Validators
{
    public class UsuarioCreateDtoValidator : AbstractValidator<UsuarioCreateDto>
    {
        private readonly IUsuarioRepository _usuarioRepository;

        public UsuarioCreateDtoValidator(IUsuarioRepository usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;

            // Nome: obrigatório, entre 3 e 100 caracteres
            RuleFor(x => x.Nome)
                .NotEmpty().WithMessage("Nome é obrigatório")
                .Length(3, 100).WithMessage("Nome deve conter entre 3 e 100 caracteres");

            // Email: obrigatório, formato válido, único
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email é obrigatório")
                .EmailAddress().WithMessage("Email deve estar em um formato válido")
                .MustAsync(async (email, ct) => !await usuarioRepository.EmailExistsAsync(email.ToLower(), ct))
                .WithMessage("Este email já está cadastrado");

            // Senha: obrigatória, mínimo 6 caracteres
            RuleFor(x => x.Senha)
                .NotEmpty().WithMessage("Senha é obrigatória")
                .MinimumLength(6).WithMessage("Senha deve ter no mínimo 6 caracteres");

            // DataNascimento: obrigatória, idade >= 18 anos
            RuleFor(x => x.DataNascimento)
                .NotEmpty().WithMessage("Data de nascimento é obrigatória")
                .Must(BeAtLeast18YearsOld).WithMessage("O usuário deve ter pelo menos 18 anos");

            // Telefone: opcional.
            RuleFor(x => x.Telefone)
                .Matches(@"^$$\d{2}$$\s\d{4,5}-\d{4}$")
                .When(x => !string.IsNullOrEmpty(x.Telefone))
                .WithMessage("Telefone deve estar no formato (XX) XXXXX-XXXX");
        }

        private static bool BeAtLeast18YearsOld(DateTime dataNascimento)
        {
            var today = DateTime.Today;
            var age = today.Year - dataNascimento.Year;
            
            if (dataNascimento.Date > today.AddYears(-age))
                age--;
            
            return age >= 18;
        }
    }
}