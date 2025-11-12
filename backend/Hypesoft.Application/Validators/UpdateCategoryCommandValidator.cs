using FluentValidation;
using Hypesoft.Application.Commands;

namespace Hypesoft.Application.Validators
{
    /// <summary>
    /// Define as regras de validação para o comando <see cref="UpdateCategoryCommand"/>.
    /// </summary>
    public class UpdateCategoryCommandValidator : AbstractValidator<UpdateCategoryCommand>
    {
        public UpdateCategoryCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .WithMessage("O ID da categoria é obrigatório para a atualização.");

            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("O nome da categoria não pode ser vazio.")
                .MaximumLength(50)
                .WithMessage("O nome da categoria não pode exceder 50 caracteres.")
                .When(x => x.Name != null); 
        }
    }
}