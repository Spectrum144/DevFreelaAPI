using DevFreela.Application.Commands.InsertComment;
using DevFreela.Application.Commands.InsertProject;
using DevFreela.Application.Services;
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
            services.AddMediatR(config =>
                config.RegisterServicesFromAssemblyContaining<InsertProjectCommand>());

            return services;
        }
    }
}
