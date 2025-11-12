using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using FluentValidation;

namespace Hypesoft.Application
{
    /// <summary>
    /// Método de Extensão para registrar todos os serviços
    /// da camada de Aplicação na Injeção de Dependência.
    /// </summary>
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            // AutoMapper
            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            // Registra o MediatR
            services.AddMediatR(cfg => 
                cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
            
            // Registra o FluentValidation
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            return services;
        }
    }
}