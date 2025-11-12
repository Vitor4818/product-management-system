using FluentValidation;
using Hypesoft.Application.Commands;
using Hypesoft.Domain.Repositories;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Bson; 

namespace Hypesoft.Application.Validators
{
    public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
    {
        private readonly ICategoryRepository _categoryRepository;

        public CreateProductCommandValidator(ICategoryRepository categoryRepository) 
        {
            _categoryRepository = categoryRepository; 

            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("O nome do produto é obrigatório.");

            RuleFor(x => x.Price)
                .GreaterThan(0)
                .WithMessage("O preço deve ser maior que zero.");

            RuleFor(x => x.StockQuantity)
                .GreaterThanOrEqualTo(0)
                .WithMessage("A quantidade em estoque não pode ser negativa.");

            
            RuleFor(x => x.CategoryId)
                .Cascade(CascadeMode.Stop) 
                .NotEmpty().WithMessage("O ID da Categoria é obrigatório.")
                .MustAsync(CategoryMustExist)
                .WithMessage("A Categoria informada não existe ou é inválida.");
        }
        private async Task<bool> CategoryMustExist(string id, CancellationToken cancellationToken)
        {
            if (!ObjectId.TryParse(id, out _))
            {
                return false; 
            }
            var category = await _categoryRepository.GetByIdAsync(id);
            return category != null;
        }
    }
}