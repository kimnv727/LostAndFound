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
using System.Text;
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
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using LostAndFound.API.Extensions;
using LostAndFound.Infrastructure.Data;
using LostAndFound.Infrastructure.DTOs.Media;
using LostAndFound.Infrastructure.Extensions;
using LostAndFound.Infrastructure.DTOs.Post;
using LostAndFound.API.Services;
using System.Text.Json;
using LostAndFound.API.Extensions.FirestoreService;
using Google.Cloud.Firestore;

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
            //Aws Credentials
            AwsCredentials awsCredentials = new AwsCredentials();
            Configuration.Bind("AwsCredentials", awsCredentials);
            services.AddSingleton(awsCredentials);

            //Facebook Credentials
            FacebookCredentials facebookCredentials = new FacebookCredentials();
            Configuration.Bind("FacebookCredentials", facebookCredentials);
            services.AddSingleton(facebookCredentials);

            services.AddCors();
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
            var firebaseJson = JsonSerializer.Serialize(new FirebaseSetting());
            services.AddSingleton(_ => new FirestoreProvider(
                new FirestoreDbBuilder
                {
                    ProjectId = "lostandfound-test-b0e12",
                    JsonCredentials = firebaseJson
                }.Build()
            ));

            services.AddScoped<IFirebaseAuthClient>(u => new FirebaseAuthClient(firebaseAuthConfig));
            services.AddSingleton(new FirebaseAuthClient(firebaseAuthConfig));
            services.AddServices();
            services.AddRepositories();
            services.AddUOW();
            services.ConfigureAutoMapper();
            services.AddServiceFilters();

            //Add hosted services 
            services.AddHostedService<ItemOutdatedCheckService>();
            services.AddHostedService<PostOutdatedCheckService>();
            services.AddHostedService<GiveawayHandlingService>();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            {
                options.Authority = Configuration["FirebaseJwt:Firebase:ValidIssuer"];
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = Configuration["FirebaseJwt:Firebase:ValidIssuer"],
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,
                    ValidAudience = Configuration["FirebaseJwt:Firebase:ValidAudience"],
                    ValidateLifetime = true
                };
            });
            services.AddAuthorization();

            services.AddControllers(opt => opt.ConfigureCacheProfiles())
                                  .AddJsonOptions(opt => opt
                                                             .JsonSerializerOptions.Converters
                                                             .Add(new JsonStringEnumConverter()))
                                                             .ConfigureNewtonsoftJson();
            services.ConfigureDbContext(Configuration);
            services.ConfigureDistributedCaching(Configuration);
            services.AddUOW();
            services.ConfigureAutoMapper();
            services.ConfigureSwagger();
            
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

            app.ConfigureExceptionHandler();

            app.UseRouting();

            app.UseCors(x => x
                .AllowAnyMethod()
                .AllowAnyHeader()
                .SetIsOriginAllowed(origin => true) // allow any origin
                .AllowCredentials());

            app.UseAuthentication();

            app.UseTokenCheckMiddleware();

            app.UseAuthorization();

            app.UseResponseCaching();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            context.Database.Migrate();

            context.MapInitialData();
        }
    }
}
