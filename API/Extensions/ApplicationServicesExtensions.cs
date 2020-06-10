using System.Linq;
using API.Errors;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace API.Extensions
{

    public static class ApplicationServicesExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            // You need both the interface and the concrete implementation.
            services.AddScoped<IProductRepository, ProductRepository>();
            services.Configure<ApiBehaviorOptions>(
                                                   o =>
                                                   {
                                                       o.InvalidModelStateResponseFactory = context =>
                                                                                            {
                                                                                                string[] errors =
                                                                                                    context
                                                                                                       .ModelState
                                                                                                       .Where(e =>
                                                                                                                  e.Value
                                                                                                                   .Errors
                                                                                                                   .Count >
                                                                                                                  0)
                                                                                                       .SelectMany(x =>
                                                                                                                       x.Value
                                                                                                                        .Errors)
                                                                                                       .Select(x =>
                                                                                                                   x.ErrorMessage)
                                                                                                       .ToArray();

                                                                                                ApiValidationErrorResponse
                                                                                                    errorResponse =
                                                                                                        new
                                                                                                            ApiValidationErrorResponse
                                                                                                            {
                                                                                                                Errors = errors
                                                                                                            };

                                                                                                return new
                                                                                                    BadRequestObjectResult(errorResponse);
                                                                                            };

                                                   });
            return services;
        }
    }

}