using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhatsappClone.Data.Helpers;
using WhatsappClone.Data.Models;
using WhatsappClone.Infrastructure.Bases;
using WhatsappClone.Infrastructure.Interfaces;
using WhatsappClone.Infrastructure.Repositories;

namespace WhatsappClone.Infrastructure
{
    public static class ModuleInfrastructureDependencies
    {

        public static IServiceCollection AddModuleInfrastructureDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            #region CustomServices
            services.AddScoped<IChat, ChatRepo>();
            services.AddScoped<IRefreshToken, RefreshTokenRepo>();
            services.AddScoped(typeof(IRepo<>), typeof(Repo<>));

            #endregion

            #region Identity And DB
            services.AddDbContext<Context>(options =>
             {
                 options.UseSqlServer(configuration.GetConnectionString("whatsapp"));
             });

            services.AddIdentity<AppUser, IdentityRole>(

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

                ).AddEntityFrameworkStores<Context>().AddDefaultTokenProviders(); ;

            #endregion


            #region JWT
            var JwtSettings = new JwtSettings();
            configuration.GetSection("jwtSettings").Bind(JwtSettings);
            services.AddSingleton(JwtSettings);
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
                    ClockSkew = TimeSpan.Zero // Disable the default 5 minute clock skew

                };

                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        var token = context.Request.Query["access_token"];
                        var path = context.HttpContext.Request.Path;
                        if (!string.IsNullOrEmpty(token) && (path.StartsWithSegments("/startChat")))
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
