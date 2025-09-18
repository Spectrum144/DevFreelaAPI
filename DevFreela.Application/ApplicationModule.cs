using DevFreela.Application.Commands.InsertComment;
using DevFreela.Application.Commands.InsertProject;
using DevFreela.Application.Models;
using DevFreela.Application.Services;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevFreela.Application {
    public static class ApplicationModule {
        public static IServiceCollection AddApplication(this IServiceCollection services) {

            services.AddServices()
                    .AddHandlers();

            
            return services;
        }

        private static IServiceCollection AddServices(this IServiceCollection services) {
            //Injeção de dependência que ficaria no Program:
            services.AddScoped<IProjectService, ProjectService>();

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
    }
}
