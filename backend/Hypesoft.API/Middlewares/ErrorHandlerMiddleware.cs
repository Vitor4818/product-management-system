using Hypesoft.Application.Common.Exceptions;
using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using System; 
using System.Threading.Tasks; 
namespace Hypesoft.API.Middlewares
{
    /// <summary>
    /// Middleware customizado para interceptar exceções
    /// e formatar a resposta de erro (ex: 500, 400, 404).
    /// </summary>
    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public ErrorHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                // Se uma exceção for pega, chama o handler customizado
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            // Define o tipo de conteúdo da resposta como JSON
            context.Response.ContentType = "application/json";
            
            // Define o código de status
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            // Objeto que será serializado para JSON
            object? responseBody = null;

            // A exceção pega é a ValidationException?
            switch (exception)
            {
                // Sim é a exceção de validação
                case ValidationException validationException:
                    // Define o status para 400 Bad Request
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    // Define o corpo da resposta como o dicionário de erros
                    responseBody = new { errors = validationException.Errors };
                    break;
                // Não, é uma exceção genérica
                default:
                    // Mantém o status 500 e manda uma mensagem genérica
                    responseBody = new { error = "Um erro inesperado ocorreu." };
                    break;
            }

            // Serializa o objeto de resposta para JSON
            var result = JsonSerializer.Serialize(responseBody);
            return context.Response.WriteAsync(result);
        }
    }
}