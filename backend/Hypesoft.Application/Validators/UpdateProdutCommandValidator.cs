using FluentValidation;
using Hypesoft.Application.Commands;

namespace Hypesoft.Application.Validators
{
    /// <summary>
    /// Define as regras de validação para o comando de update.
    /// </summary>
    public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
    {
        public UpdateProductCommandValidator()
        {
            // Regra para 'Id':
            RuleFor(x => x.Id)
                .NotEmpty()
                .WithMessage("O ID do produto é obrigatório para a atualização.");

            // Regra para 'Name':
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("O nome do produto não pode ser vazio.")
                .MaximumLength(100)
                .WithMessage("O nome do produto não pode exceder 100 caracteres.")
                .When(x => x.Name != null); 

            // Regra para 'Description':
            RuleFor(x => x.Description)
                .MaximumLength(500)
                .WithMessage("A descrição não pode exceder 500 caracteres.")
                .When(x => x.Description != null);

            // Regra para 'Price':
            RuleFor(x => x.Price)
                .GreaterThan(0)
                .WithMessage("O preço deve ser maior que zero.")
                .When(x => x.Price.HasValue);

            // Regra para 'StockQuantity':
            RuleFor(x => x.StockQuantity)
                .GreaterThanOrEqualTo(0)
                .WithMessage("A quantidade em estoque não pode ser negativa.")
                .When(x => x.StockQuantity.HasValue); 

            // Regra para 'CategoryId':
            RuleFor(x => x.CategoryId)
                .NotEmpty()
                .WithMessage("A categoria do produto não pode ser vazia.")
                .When(x => x.CategoryId != null); 
        }
    }
}