﻿using Microsoft.Extensions.Configuration;
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
using LostAndFound.API.Authentication;
using LostAndFound.API.Filters;
using LostAndFound.Infrastructure.Profiles;
using Microsoft.EntityFrameworkCore;
using LostAndFound.Infrastructure.Data;
using LostAndFound.Infrastructure.Repositories.Interfaces;
using LostAndFound.Infrastructure.Repositories.Implementations;
using LostAndFound.Infrastructure.Services.Interfaces;
using Firebase.Auth;
using Firebase.Auth.Providers;
using LostAndFound.Infrastructure.Services.Implementations;
using LostAndFound.Infrastructure.UnitOfWork;

namespace LostAndFound.API.Extensions
{
    public static class ServiceExtensions
    {
        public static void AddServices(this IServiceCollection services)
        {
            services.AddScoped<IFirebaseAuthService, FirebaseAuthService>();
            services.AddScoped<IMediaService, MediaService>();
            services.AddScoped<IUserMediaService, UserMediaService>();
            services.AddScoped<IPostMediaService, PostMediaService>();
            services.AddScoped<IItemMediaService, ItemMediaService>();
            services.AddScoped<IReportMediaService, ReportMediaService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IPasswordHasherService, PasswordHasherService>();
            services.AddScoped<IEmailSendingService, EmailSendingService>();
            services.AddScoped<IReportService, ReportService>();
            services.AddScoped<IItemService, ItemService>();
            services.AddScoped<IPostService, PostService>();
            services.AddScoped<ICommentService, CommentSerivce>();
            services.AddScoped<IPostBookmarkService, PostBookmarkService>();
            services.AddScoped<IPostFlagService, PostFlagService>();
            services.AddScoped<ICommentFlagService, CommentFlagService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<ILocationService, LocationService>();
            services.AddScoped<ICampusService, CampusService>();
            services.AddScoped<ICategoryGroupService, CategoryGroupService>();
            services.AddScoped<IItemFlagService, ItemFlagService>();
            services.AddScoped<IItemBookmarkService, ItemBookmarkService>();
            services.AddScoped<INotificationService, NotificationService>();
            services.AddScoped<IUserDeviceService, UserDeviceService>();
            services.AddScoped<IGiveawayService, GiveawayService>();
            services.AddScoped<IGiveawayParticipantService, GiveawayParticipantService>();
            services.AddScoped<IOcrService, OcrService>();
            services.AddScoped<IItemClaimService, ItemClaimService>();
            services.AddScoped<IItemClaimService, ItemClaimService>();
            services.AddScoped<IStorageService, StorageService>();
            services.AddScoped<ICabinetService, CabinetService>();
            services.AddScoped<IReceiptService, ReceiptService>();
            services.AddScoped<IDashboardService, DashboardService>();
        }

        public static void AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IMediaRepository, MediaRepository>();
            services.AddScoped<IUserMediaRepository, UserMediaRepository>();
            services.AddScoped<IItemMediaRepository, ItemMediaRepository>();
            services.AddScoped<IPostMediaRepository, PostMediaRepository>();
            services.AddScoped<IReportMediaRepository, ReportMediaRepository>();
            services.AddScoped<IReportRepository, ReportRepository>();
            services.AddScoped<IItemRepository,ItemRepository>();
            services.AddScoped<IPostRepository,PostRepository>();
            services.AddScoped<ICommentRepository,CommentRepository>();
            services.AddScoped<IPostBookmarkRepository, PostBookmarkRepository>();
            services.AddScoped<IPostFlagRepository, PostFlagRepository>();
            services.AddScoped<ICommentFlagRepository, CommentFlagRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<ILocationRepository, LocationRepository>();
            services.AddScoped<ICampusRepository, CampusRepository>();
            services.AddScoped<ICategoryGroupRepository, CategoryGroupRepository>();
            services.AddScoped<IItemFlagRepository, ItemFlagRepository>();
            services.AddScoped<IItemBookmarkRepository, ItemBookmarkRepository>();
            services.AddScoped<INotificationRepository, NotificationRepository>();
            services.AddScoped<IUserDeviceRepository, UserDeviceRepository>();
            services.AddScoped<IGiveawayRepository, GiveawayRepository>();
            services.AddScoped<IGiveawayParticipantRepository, GiveawayParticipantRepository>();
            services.AddScoped<IItemClaimRepository, ItemClaimRepository>();
            services.AddScoped<IStorageRepository, StorageRepository>();
            services.AddScoped<ICabinetRepository, CabinetRepository>();
            services.AddScoped<IReceiptRepository, ReceiptRepository>();
        }
        public static void AddServiceFilters(this IServiceCollection services)
        {
            services.AddScoped<AutoValidateModelState>();
        }

        public static void AddUOW(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
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
                    Title = "LostAndFound",
                    Version = "Beta 1",
                    Description = $"LostAndFound API application",
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
                    Description = "Add LostAndFound Bearer token here.",
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
    }
}
