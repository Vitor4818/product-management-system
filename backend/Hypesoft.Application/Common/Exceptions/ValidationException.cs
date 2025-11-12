using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Hypesoft.Application.Common.Exceptions
{
    /// <summary>
    /// Exceção customizada que será lançada pelo ValidationBehavior quando uma ou mais regras do FluentValidation falharem.
    /// </summary>
    public class ValidationException : Exception
    {
        // Dicionário contendo os erros de validação 
        public IDictionary<string, string[]> Errors { get; }

        // Construtor padrão.
        public ValidationException()
            : base("Uma ou mais falhas de validação ocorreram.")
        {
            Errors = new Dictionary<string, string[]>();
        }


        // Construtor que recebe as falhas do FluentValidation e as converte em um dicionário de erros fácil de serializar para JSON.
        public ValidationException(IEnumerable<ValidationFailure> failures)
            : this()
        {
            // Agrupa as falhas pelo nome da propriedade
            Errors = failures
                .GroupBy(e => e.PropertyName, e => e.ErrorMessage)
                .ToDictionary(failureGroup => failureGroup.Key, failureGroup => failureGroup.ToArray());
        }
    }
}