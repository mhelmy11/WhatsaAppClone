
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MoMediatoR;
using Scalar.AspNetCore;
using Serilog;
using System.Reflection;
using WhatsappClone.API.Base;
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
            #region Serilog
            Log.Logger = new LoggerConfiguration()
                    .WriteTo.Console()
                    .Enrich.FromLogContext()
                    .CreateBootstrapLogger();

            Log.Information("Starting up the web host");
            #endregion

            try
            {

                var builder = WebApplication.CreateBuilder(args);
                //builder.Services.AddOpenApi("v1", options =>
                //               {
                //                   options.AddDocumentTransformer<BearerSecuritySchemeTransformer>();
                //               });

                #region Serilog
                builder.Host.UseSerilog((context, services, configuration) => configuration
                               .ReadFrom.Configuration(context.Configuration) // اقرأ الإعدادات من appsettings
                               .ReadFrom.Services(services) // اسمح بحقن خدمات أخرى في الـ sinks
                               .Enrich.FromLogContext()); // أضف بيانات سياقية مثل RequestId 
                #endregion

                // Add services to the container.

                builder.Services.AddControllers();


                // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

                builder.Services.AddEndpointsApiExplorer();
                builder.Services.AddSwaggerGen(options =>
                {

                    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                    {
                        Name = "Authorization", // اسم الـ Header
                        Type = SecuritySchemeType.Http, // نوع المخطط
                        Scheme = "bearer", // يجب أن تكون "bearer" (بحروف صغيرة)
                        BearerFormat = "JWT", // نوضح أن الصيغة هي JWT
                        In = ParameterLocation.Header, // مكان التوكن في الـ Header
                        Description = "Please enter a valid token. \n\n" +
                                      "Enter 'Bearer' [space] and then your token in the text input below.\n\n" +
                                      "Example: \"Bearer 12345abcdef\""
                    });

                    // 2. تطبيق مخطط الأمان على كل العمليات التي تحتاج لمصادقة
                    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer" // يجب أن يتطابق مع الاسم في AddSecurityDefinition
                            }
                        },
                        new string[] {}
                    }
    });
                });
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

                app.UseMiddleware<ErrorHandlerMiddleware>();
                app.UseSerilogRequestLogging();
                if (app.Environment.IsDevelopment())
                {
                    app.UseSwagger();
                    app.UseSwaggerUI();
                    //app.MapScalarApiReference();
                    //app.MapOpenApi();

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
                app.Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Application start-up failed");
                throw;
            }
            finally
            {
                Log.Information("Shut down complete");
                Log.CloseAndFlush();
            }
        }
    }
}
