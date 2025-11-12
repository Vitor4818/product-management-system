using FluentValidation;
using Hypesoft.Application.Commands;
using Hypesoft.Domain.Repositories;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Bson; 

namespace Hypesoft.Application.Validators
{
    public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
    {
        private readonly ICategoryRepository _categoryRepository;

        public UpdateProductCommandValidator(ICategoryRepository categoryRepository) 
        {
            _categoryRepository = categoryRepository; 
            
            RuleFor(x => x.Id)
                .NotEmpty()
                .WithMessage("O ID do produto é obrigatório para a atualização.");

            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("O nome do produto não pode ser vazio.")
                .When(x => x.Name != null); 

            RuleFor(x => x.Price)
                .GreaterThan(0)
                .WithMessage("O preço deve ser maior que zero.")
                .When(x => x.Price.HasValue);

            RuleFor(x => x.StockQuantity)
                .GreaterThanOrEqualTo(0)
                .WithMessage("A quantidade em estoque não pode ser negativa.")
                .When(x => x.StockQuantity.HasValue);

            
            RuleFor(x => x.CategoryId)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("O ID da Categoria não pode ser vazio.")
                .MustAsync(CategoryMustExist) 
                .WithMessage("A Categoria informada não existe ou é inválida.")
                .When(x => x.CategoryId != null); 
            
        }
        private async Task<bool> CategoryMustExist(string id, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(id) || !ObjectId.TryParse(id, out _))
            {
                return false; 
            }
            var category = await _categoryRepository.GetByIdAsync(id);
            return category != null;
        }
    }
}