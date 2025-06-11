using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhatsappClone.Core.Behaviours
{
    public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
       where TRequest : IRequest<TResponse> // بنقول إن ده هيشتغل على أي حاجة من نوع IRequest (يعني Command أو Query)
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators; // هنا بيتحط كل الـ Validators اللي تخص الـ TRequest

        public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators;
        }

        // دي الميثود اللي بتشتغل لما أي أمر بيعدي على البوابة دي
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            // لو فيه أي قواعد فحص موجودة للأمر ده
            if (_validators.Any())
            {
                // بنجهز الأمر للفحص
                var context = new ValidationContext<TRequest>(request);

                // بنشغل كل قواعد الفحص اللي لقيناها على الأمر ده
                var validationResults = await Task.WhenAll(
                    _validators.Select(v => v.ValidateAsync(context, cancellationToken)));

                // بنجمع كل الأخطاء اللي طلعت من كل الفحوصات
                var failures = validationResults
                    .Where(r => r.Errors.Any()) // بنختار بس اللي فيها أخطاء
                    .SelectMany(r => r.Errors) // بنخلي الأخطاء كلها في قائمة واحدة
                    .ToList();

                // لو لقينا أي أخطاء
                if (failures.Any())
                {
                    // بنرمي استثناء (Exception) اسمه ValidationException
                    // الاستثناء ده هيتلقف في طبقة الـ Web API عشان يتعامل معاه صح
                    throw new ValidationException(failures);
                }
            }

            // لو مفيش أخطاء، بنقول للأمر كمل طريقك للمسؤول عنك (الـ Handler)
            return await next();
        }
    }
}
