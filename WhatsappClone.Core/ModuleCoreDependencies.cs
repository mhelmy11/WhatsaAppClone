using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using MoMediatoR;
using System.Reflection;
using WhatsappClone.Core.Behaviours;
using WhatsappClone.Core.RequirementsHandlers;

namespace WhatsappClone.Core
{
    public static class ModuleCoreDependencies
    {

        public static IServiceCollection AddCoreDependencies(this IServiceCollection services)
        {
            // Register MediatR


            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));


            // Register AutoMapper
            services.AddAutoMapper(Assembly.GetExecutingAssembly());



            // 2. تسجيل Fluent Validation Validators:
            // بنقول لـ Fluent Validation إنه يدور ويلاقي كل الـ Validators (زي CreateChatValidator)
            // اللي موجودة في الـ Assembly الحالي ويسجلها.
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            // 3. تسجيل الـ ValidationBehavior في MediatR Pipeline:
            // بنقول لـ MediatR إن أي أمر أو استعلام (IPipelineBehavior<,>) بيعدي،
            // يعدي الأول على الـ ValidationBehavior بتاعنا.
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));


            services.AddTransient<SeesionNotRevokedRequirementHandler>();

            return services;
        }



    }
}
