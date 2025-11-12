using FluentValidation;
using Hypesoft.Application.Commands;

namespace Hypesoft.Application.Validators
{
    /// <summary>
    /// Define as regras de validação para o comando <see cref="CreateCategoryCommand"/>.
    /// </summary>
    public class CreateCategoryCommandValidator : AbstractValidator<CreateCategoryCommand>
    {
        public CreateCategoryCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty() 
                .WithMessage("O nome da categoria é obrigatório.")
                .MaximumLength(50) 
                .WithMessage("O nome da categoria não pode exceder 50 caracteres.");
            
            // (Em um projeto real, adicionaríamos uma regra 'MustAsync'
            // para verificar se o nome da categoria já existe no banco,
            // injetando o ICategoryRepository aqui. Para uma "lista simples",
            // vamos manter assim.)
        }
    }
}