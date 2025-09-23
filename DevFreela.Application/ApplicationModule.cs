using DevFreela.Application.Commands.InsertProject;
using DevFreela.Application.Models;
using FluentValidation;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace DevFreela.Application {
    public static class ApplicationModule {
        public static IServiceCollection AddApplication(this IServiceCollection services) {

            services.AddHandlers()
                    .AddValidation();

            
            return services;
        }

        private static IServiceCollection AddHandlers(this IServiceCollection services) {
            // Por ser um método que atua somente no Assembly, ele busca somente na camada de Application.
            services.AddMediatR(config =>
                config.RegisterServicesFromAssemblyContaining<InsertProjectCommand>());

            services.AddTransient<IPipelineBehavior<
                InsertProjectCommand,
                ResultViewModel<int>>, 
                ValidateInsertProjectCommandBehavior>();

            return services;
        }

        private static IServiceCollection AddValidation(this IServiceCollection services) {

            // Apesar de atuar no assembly, não é preciso referenciar todas as classes pois já as localiza.
            services.AddFluentValidationAutoValidation()
                    .AddValidatorsFromAssemblyContaining<InsertProjectCommand>();

            return services;
        }
    }
}
