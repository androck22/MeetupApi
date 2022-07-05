using FluentValidation.AspNetCore;
using MeetupApi.Contracts.Validators;
using MeetupApi.Data;
using MeetupApi.Data.Repos;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

namespace MeetupApi
{
    public class Startup
    {
        /// <summary>
        /// Загрузка конфигурации из файла Jsong
        /// </summary>
        private IConfiguration Configuration
        { get; } = new ConfigurationBuilder()
          .AddJsonFile("appsettings.json")
          .AddJsonFile("appsettings.Development.json")
          .Build();

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Подключаем автомаппинг
            var assembly = Assembly.GetAssembly(typeof(MappingProfile));
            services.AddAutoMapper(assembly);

            // регистрация сервиса репозитория для взаимодействия с базой данных
            services.AddSingleton<IMeetupRepository, MeetupRepository>();
            services.AddSingleton<ISponsorRepository, SponsorRepository>();
            services.AddSingleton<IUserRepository, UserRepository>();

            string connection = Configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<MeetupApiContext>(options => options.UseSqlServer(connection), ServiceLifetime.Singleton);

            // Подключаем валидацию
            services.AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<AddMeetupRequestValidator>());

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath); 
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "MeetupApi", Version = "v1" });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please insert token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "bearer"
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[]{}
                    }
                });
            });

            services.AddAuthentication(options => options.DefaultScheme = "Cookies")
                .AddCookie("Cookies", options =>
                {
                    options.Events = new Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationEvents
                    {
                        OnRedirectToLogin = redirectContext =>
                        {
                            redirectContext.HttpContext.Response.StatusCode = 401;
                            return Task.CompletedTask;
                        }
                    };
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "MeetupApi v1"));
            }

            //app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
