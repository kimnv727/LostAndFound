using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using LostAndFound.API.Filters;
using LostAndFound.Infrastructure.Profiles;
using Microsoft.EntityFrameworkCore;
using LostAndFound.Infrastructure.Data;
using LostAndFound.Infrastructure.Repositories.Interfaces;
using LostAndFound.Infrastructure.Repositories.Implementations;

namespace LostAndFound.API.Extensions
{
    public static class ServiceExtensions
    {
        public static void AddServices(this IServiceCollection services)
        {
        }

        public static void AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IUserRepository, UserRepository>();
        }
        public static void AddServiceFilters(this IServiceCollection services)
        {
            services.AddScoped<AutoValidateModelState>();
        }

        public static void AddUOW(this IServiceCollection services)
        {
            //services.AddScoped<IUnitOfWork, UnitOfWork>();
        }

        public static void ConfigureDistributedCaching(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDistributedMemoryCache();
        }

        public static void ConfigureAutoMapper(this IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetAssembly(typeof(MappingProfile)));
        }

        public static void ConfigureDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<LostAndFoundDbContext>(options => options.UseSqlServer(x => x.MigrationsAssembly("LostAndFound.API")));
        }

        public static void ConfigurePermissionAuthorization(this IServiceCollection services)
        {
            // services.AddSingleton<IAuthorizationPolicyProvider, PermissionPolicyProvider>();
            // services.AddScoped<IAuthorizationHandler, PermissionAuthorizationHandler>();
        }

        public static void ConfigureSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                // For recognizing iFormFiles in multiple-params controller actions
                c.SchemaGeneratorOptions.CustomTypeMappings.Add(typeof(IFormFile),
                    () => new OpenApiSchema() { Type = "file", Format = "binary" });

                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "GoPark",
                    Version = "Beta 1",
                    Description = $"GoPark API application",
                });
                c.UseInlineDefinitionsForEnums();
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
                var entitiesXmlPath = Path.Combine(AppContext.BaseDirectory, "LostAndFound.Infrastructure.xml");
                c.IncludeXmlComments(entitiesXmlPath);

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Add GoPark Bearer token here.",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Name = "Bearer"
                        },
                        new List<string>()
                    }
                });
            });
        }

        public static void ConfigureJWTAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = configuration["TokenGenerator:Issuer"],
                    ValidAudience = configuration["TokenGenerator:Audience"],
                    IssuerSigningKey = new
                        SymmetricSecurityKey
                        (Encoding.UTF8.GetBytes
                            (configuration["TokenGenerator:TokenSecret"]))
                };

                //options.EventsType = typeof(JWTConfigureHubsAndCheckAccountActiveEvents);
            });

            //services.AddScoped<JWTConfigureHubsAndCheckAccountActiveEvents>();
        }
    }
}
