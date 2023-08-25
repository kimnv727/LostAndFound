using LostAndFound.API.Extensions;
using LostAndFound.Infrastructure.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Firebase.Auth;
using Firebase.Auth.Providers;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using LostAndFound.API.Authentication;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace LostAndFound.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers();
            // Configure Google Services
            FirebaseApp.Create(new AppOptions()
            {
                Credential = GoogleCredential.FromFile("./firebase-config.json"),
            });
            
            var firebaseAuthConfig = new FirebaseAuthConfig
            {
                ApiKey = "AIzaSyDj7Wa-uQkY9jO4NQP5s6MwvQJMO_b2PkA",
                AuthDomain = $"LostAndFound-Test.firebaseapp.com",
                Providers = new FirebaseAuthProvider[]
                {
                    new EmailProvider(),
                    new GoogleProvider()
                }
            };
            services.AddScoped<IFirebaseAuthClient>(u => new FirebaseAuthClient(firebaseAuthConfig));
            services.AddSingleton(new FirebaseAuthClient(firebaseAuthConfig));
            services.AddServices();
            services.AddRepositories();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            {
                options.Authority = "https://securetoken.google.com/LostAndFound-Test";
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = "https://securetoken.google.com/LostAndFound-Test",
                    ValidateAudience = true,
                    ValidAudience = "LostAndFound-Test",
                    ValidateLifetime = true,
                };
            });

            services.AddAuthentication();
            services.AddAuthorization();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "LostAndFound.API", Version = "v1" });
            });

            services.AddControllers(opt => opt.ConfigureCacheProfiles())
                                  .AddJsonOptions(opt => opt
                                                             .JsonSerializerOptions.Converters
                                                             .Add(new JsonStringEnumConverter()))
                                                             .ConfigureNewtonsoftJson();
            services.ConfigureDbContext(Configuration);
            services.ConfigureDistributedCaching(Configuration);
            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, LostAndFoundDbContext context)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "LostAndFound.API v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            
            app.UseCors(options =>
            {
                options.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
            });

            context.Database.Migrate();
            
            /*string firebaseFileName = "lostandfound-test-b0e12-firebase-adminsdk-8j53l-641398cc43.json";
            string path = Path.Combine(Environment.CurrentDirectory, @"Data\", firebaseFileName);
            FirebaseApp.Create(new AppOptions()
            {
                Credential = GoogleCredential.FromFile(path),
            });*/

        }
    }
}
