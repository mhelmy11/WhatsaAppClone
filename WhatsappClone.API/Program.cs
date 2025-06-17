
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MoMediatoR;
using System.Reflection;
using WhatsappClone.API.Requirements.Handlers;
using WhatsappClone.Core;
using WhatsappClone.Core.Features.Chats.Queries.Handler;
using WhatsappClone.Core.Features.Chats.Queries.Models;
using WhatsappClone.Core.Filters;
using WhatsappClone.Data.Helpers;
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
            builder.Services.AddTransient<IAuthorizationHandler, SessionNotRevokedHandler>();
            //builder.Services.AddScoped<UnitOfWork>();

            builder.Services.Configure<SendGridSettings>(builder.Configuration.GetSection("SendGrid"));
            builder.Services.AddCoreDependencies();
            builder.Services.AddModuleInfrastructureDependencies(builder.Configuration);
            builder.Services.AddModuleServiceDependencies();

            // إضافة الـ Policy
            builder.Services.AddAuthorization(options =>
            {
                options.DefaultPolicy = new AuthorizationPolicyBuilder()
                       .RequireAuthenticatedUser() // هذا هو الشرط الافتراضي الأصلي، يجب إبقاؤه
                       .AddRequirements(new SessionNotRevokedRequirement()) // نضيف شرطنا المخصص إليه
                       .Build();
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
