using System.Linq;
using API.Errors;
using API.Helpers;
using API.Middleware;
using AutoMapper;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace API
{

    public class Startup
    {
        private readonly IConfiguration _configuration;
        
        public Startup(IConfiguration configuration) => _configuration = configuration;

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAutoMapper(typeof(MappingProfiles));
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
             // You need both the interface and the concrete implementation.
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddDbContext<StoreContext>(o => 
                                                    o.UseSqlite(_configuration.GetConnectionString("DefaultConnection")));
            services.AddControllers();
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
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseMiddleware<ExceptionMiddleware>();

            app.UseStatusCodePagesWithReExecute("/errors/{0}");

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseStaticFiles();
            
            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }

}