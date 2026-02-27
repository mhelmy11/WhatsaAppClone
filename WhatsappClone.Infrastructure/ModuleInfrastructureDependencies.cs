using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhatsappClone.Data.Helpers;
using WhatsappClone.Data.Models;
using WhatsappClone.Data.SqlServerModels;
using WhatsappClone.Infrastructure.Bases;
using WhatsappClone.Infrastructure.Data;
using WhatsappClone.Infrastructure.Helpers;
using WhatsappClone.Infrastructure.Interfaces;
using WhatsappClone.Infrastructure.Repositories;

namespace WhatsappClone.Infrastructure
{
    public static class ModuleInfrastructureDependencies
    {

        public static IServiceCollection AddModuleInfrastructureDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            #region CustomServices
            services.AddScoped<IChatRepository, ChatRepository>();
            services.AddScoped<IMessageRepository, MessageRepository>();
            services.AddScoped<IGroupRepository, GroupRepository>();
            services.AddScoped<IUserGroupRepository, UserGroupRepository>();
            services.AddScoped<IRefreshTokenAuditRepository, RefreshTokenAuditRepository>();
            services.AddScoped<IUserContactRepository, UserContactRepository>();
            services.AddScoped<IStoryRepository, StoryRepository>();
            services.AddScoped(typeof(ISqlBaseRepository<>), typeof(SqlBaseRepository<>));
            services.AddScoped(typeof(IMongoBaseRepository<>), typeof(MongoBaseRepository<>));
            #endregion

            #region MongoDB
            services.Configure<MongoDbSettings>(configuration.GetSection("MongoDbSettings"));//then we can inject IOptions<MongoDbSettings> to get the settings values
            var MongoDBSettings = configuration.GetSection("MongoDbSettings").Get<MongoDbSettings>();

            // Register MongoDB client
            services.AddSingleton<IMongoClient>(new MongoClient(MongoDBSettings.ConnectionString));

            // Register MongoDB factory
            services.AddSingleton<IMongoDBFactory>(sp =>
            {
                var client = sp.GetRequiredService<IMongoClient>();
                return new MongoDBFactory(client, MongoDBSettings.DatabaseName);
            });
            #endregion

            #region Identity And DBContext
            services.AddDbContext<SqlDBContext>(options =>
             {
                 options.UseSqlServer(configuration.GetConnectionString("whatsapp"));
             });
            services.AddDataProtection();
            services.AddIdentity<User, Role>(

                options =>
                {
                    options.User.RequireUniqueEmail = true;
                    options.Password.RequiredLength = 4;
                    options.Password.RequireUppercase = false;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Lockout.MaxFailedAccessAttempts = 3;
                    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(1);
                    options.SignIn.RequireConfirmedEmail = true; // Require email confirmation for sign-in
                }

                ).AddEntityFrameworkStores<SqlDBContext>().AddDefaultTokenProviders(); ;

            #endregion


            #region JWT
            services.Configure<JwtSettings>(configuration.GetSection("jwtSettings"));//then we can inject IOptions<JwtSettings> to get the settings values

            var JwtSettings = configuration.GetSection("jwtSettings").Get<JwtSettings>();
            #endregion


            #region Bearer
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = "default";
                options.DefaultChallengeScheme = "default";


            }).AddJwtBearer("default", options =>
            {

                options.TokenValidationParameters = new()
                {
                    ValidateIssuer = JwtSettings.ValidateIssuer,
                    ValidateAudience = JwtSettings.ValidateAudience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtSettings.SecretKey)),
                    ValidIssuer = JwtSettings.Issuer,
                    ValidAudience = JwtSettings.Audience,
                    ValidateIssuerSigningKey = JwtSettings.ValidateIssuerSigningKey,
                    ValidateLifetime = JwtSettings.ValidateLifetime,

                };

                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        var token = context.Request.Query["access_token"];
                        var path = context.HttpContext.Request.Path;
                        if (!string.IsNullOrEmpty(token) && path.StartsWithSegments("/startChat"))
                        {
                            context.Token = token;
                        }
                        return Task.CompletedTask;
                    }
                };
            });
            #endregion


            return services;
        }
    }
}
