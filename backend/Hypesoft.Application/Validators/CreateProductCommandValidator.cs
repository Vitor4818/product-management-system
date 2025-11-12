using FluentValidation;
using Hypesoft.Application.Commands;

namespace Hypesoft.Application.Validators
{
    /// <summary>
    /// Define as regras de validação para o comando CreateProductCommand.
    /// </summary>
    public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
    {
        public CreateProductCommandValidator()
        {
            // Regra para 'Name':
            RuleFor(x => x.Name)
                .NotEmpty() 
                .WithMessage("O nome do produto é obrigatório.")
                .MaximumLength(100)
                .WithMessage("O nome do produto não pode exceder 100 caracteres.");

            // Regra para 'Description':
            RuleFor(x => x.Description)
                .NotEmpty()
                .WithMessage("A descrição do produto é obrigatória")
                .MaximumLength(500) 
                .WithMessage("A descrição não pode exceder 500 caracteres.");

            // Regra para 'Price':
            RuleFor(x => x.Price)
                .GreaterThan(0) 
                .WithMessage("O preço deve ser maior que zero.");

            // Regra para 'StockQuantity':
            RuleFor(x => x.StockQuantity)
                .GreaterThanOrEqualTo(0)
                .WithMessage("A quantidade em estoque não pode ser negativa.");

            // Regra para 'CategoryId':
            RuleFor(x => x.CategoryId)
                .NotEmpty() 
                .WithMessage("A categoria do produto é obrigatória.");
        }
    }
}