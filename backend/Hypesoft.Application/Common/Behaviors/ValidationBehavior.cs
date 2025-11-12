using FluentValidation;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Hypesoft.Application.Common.Exceptions;

namespace Hypesoft.Application.Common.Behaviors
{
    /// <summary>
    /// O "Segurança" (Pipeline Behavior) que intercepta TODOS os pedidos do MediatR.
    /// </summary>
    public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse> 
    {
        
        private readonly IEnumerable<IValidator<TRequest>> _validators;

        public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators;
        }

        /// <summary>
        /// O método Handle que intercepta o pedido.
        /// </summary>
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            if (_validators.Any())
            {
                // Cria o "contexto" de validação.
                var context = new ValidationContext<TRequest>(request);

                // Executa todos os validadores e coleta os resultados.
                var validationResults = await Task.WhenAll(
                    _validators.Select(v => v.ValidateAsync(context, cancellationToken))
                );

                // Pega todas as failures de todos os resultados.
                var failures = validationResults
                    .SelectMany(r => r.Errors)
                    .Where(f => f != null)
                    .ToList();

                // Se houver erros
                if (failures.Count != 0)
                {
                    // Ele lança a nossa exceção customizada, que contém todos os erros.
                throw new Hypesoft.Application.Common.Exceptions.ValidationException(failures);              
                }
            }
            
            // Se não houver validadores OU se não houver erros,
            return await next();
        }
    }
}