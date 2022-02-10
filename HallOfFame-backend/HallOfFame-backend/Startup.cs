using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerUI;
using HallOfFame_backend.DataBase;
using HallOfFame_backend.Services;
using HallOfFame_backend.Services.Interfaces;
using HallOfFame_backend.Middlewares;
using Microsoft.Extensions.Configuration;

namespace HallOfFame_backend
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            AppConfiguration = configuration;
        }

        public IConfiguration AppConfiguration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            services.AddCors();

            services.Configure<AppSettings>(AppConfiguration);

            var connection = AppConfiguration.GetConnectionString("DefaultConnection");

            services.AddDbContext<ApplicationContext>(options => options.UseSqlServer(connection));

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Swagger API",
                    Version = "v1"
                });
            });
            
            services.AddControllersWithViews();

            services.AddScoped<IPersonService, PersonService>();
            services.AddSingleton<ILoggerService, LoggerService>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "API v1");
                c.DocExpansion(DocExpansion.None);
            });

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors();

            app.UseMiddleware<ExceptionHandlerMiddleware>();

            app.UseMiddleware<AppSettingsMiddleware>();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}
