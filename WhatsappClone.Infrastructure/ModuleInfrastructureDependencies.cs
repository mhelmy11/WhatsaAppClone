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
using WhatsappClone.Infrastructure.Data;
using WhatsappClone.Infrastructure.Helpers;

namespace WhatsappClone.Infrastructure
{
    public static class ModuleInfrastructureDependencies
    {

        public static IServiceCollection AddModuleInfrastructureDependencies(this IServiceCollection services, IConfiguration configuration)
        {

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
