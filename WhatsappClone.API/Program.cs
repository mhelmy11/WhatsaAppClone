
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MoMediatoR;
using System.Reflection;
using WhatsappClone.Core;
using WhatsappClone.Core.Features.Chats.Queries.Handler;
using WhatsappClone.Core.Features.Chats.Queries.Models;
using WhatsappClone.Core.Filters;
using WhatsappClone.Data.Models;
using WhatsappClone.Infrastructure;
using WhatsappClone.Service;

namespace WhatsappClone.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddSignalR();
            builder.Services.AddDbContext<Context>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("whatsapp"));
            });

            builder.Services.AddIdentity<AppUser, IdentityRole>(

                options =>
                {
                    options.User.RequireUniqueEmail = true;
                    options.Password.RequiredLength = 4;
                    options.Password.RequireUppercase = false;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Lockout.MaxFailedAccessAttempts = 3;
                    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(1);
                }

                ).AddEntityFrameworkStores<Context>();

            //builder.Services.AddScoped<UnitOfWork>();
            builder.Services.AddCoreDependencies();
            builder.Services.AddScoped<ChatsQueryHandler>();
            builder.Services.AddModuleInfrastructureDependencies();
            builder.Services.AddModuleServiceDependencies();
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = "default";
                options.DefaultChallengeScheme = "default";


            }).AddJwtBearer("default", options =>
            {

                options.TokenValidationParameters = new()
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(builder.Configuration["secretKey"]))
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

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", builder =>
                {
                    builder.SetIsOriginAllowed(_ => true);
                    builder
                        .AllowAnyMethod()
                        .AllowAnyHeader().AllowCredentials()
                        ;
                });
            });


            builder.Services.Configure<HostOptions>(options =>
            {
                options.ShutdownTimeout = TimeSpan.FromSeconds(60);
            });

            //builder.Services.AddScoped<UserRepo>();

            //builder.Services.AddTransient<AuthFilter>();








            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseStaticFiles();

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseCors("CorsPolicy");
            //app.MapHub<ChatHub>("/startChat");
            app.MapControllers();
            app.Lifetime.ApplicationStopping.Register(() =>
            {
                using var scope = app.Services.CreateScope();
                var context = scope.ServiceProvider.GetRequiredService<Context>();
                context.Database.ExecuteSqlRaw("DELETE FROM UserConnections");
                Console.WriteLine("Table UserConnections cleared before shutdown.");
            });
            app.UseMiddleware<ErrorHandlerMiddleware>();
            app.Run();
        }
    }
}
