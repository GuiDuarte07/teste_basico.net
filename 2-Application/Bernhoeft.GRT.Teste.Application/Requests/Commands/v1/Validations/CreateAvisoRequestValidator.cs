using FluentValidation;

namespace Bernhoeft.GRT.Teste.Application.Requests.Commands.v1.Validations
{
    public class CreateAvisoRequestValidator : AbstractValidator<CreateAvisoRequest>
    {
        public CreateAvisoRequestValidator()
        {
            RuleFor(x => x.Titulo)
                .NotEmpty()
                .WithMessage("O Título é obrigatório.")
                .MaximumLength(50)
                .WithMessage("O Título deve ter no máximo 50 caracteres.");

            RuleFor(x => x.Mensagem)
                .NotEmpty()
                .WithMessage("A Mensagem é obrigatória.");
        }
    }
}