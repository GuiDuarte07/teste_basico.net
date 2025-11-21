using FluentValidation;

namespace Bernhoeft.GRT.Teste.Application.Requests.Commands.v1.Validations
{
    public class UpdateAvisoRequestValidator : AbstractValidator<UpdateAvisoRequest>
    {
        public UpdateAvisoRequestValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithMessage("O ID deve ser maior que zero.");

            // Quando informado, validar mensagem
            RuleFor(x => x.Mensagem)
                .NotEmpty()
                .When(x => x.Mensagem != null)
                .WithMessage("A Mensagem não pode ser vazia quando informada.");
        }
    }
}