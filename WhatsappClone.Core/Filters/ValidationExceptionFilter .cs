using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using WhatsappClone.Core.Bases;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace WhatsappClone.Core.Filters
{
    public class ValidationExceptionFilter : IAsyncExceptionFilter
    {
        public async Task OnExceptionAsync(ExceptionContext context)
        {


            #region GeminiCode
            //if (context.Exception is ValidationException validationException)
            //{
            //    // لو هو، بنجهز رسالة خطأ مناسبة للـ Client
            //    var problemDetails = new ValidationProblemDetails(
            //        // بنحول الأخطاء اللي في الـ ValidationException لشكل سهل للـ Client
            //        // بنجمع الأخطاء حسب اسم الـ Property اللي فيه المشكلة
            //        validationException.Errors
            //            .GroupBy(e => e.PropertyName)
            //            .ToDictionary(
            //                g => g.Key, // اسم الـ Property
            //                g => g.Select(e => e.ErrorMessage).ToArray())) // رسالة الخطأ
            //    {
            //        Status = (int)HttpStatusCode.BadRequest, // كود الـ HTTP هيكون 400
            //        Title = "حدثت أخطاء في البيانات المدخلة.", // عنوان عام للخطأ
            //        Type = "https://tools.ietf.org/html/rfc7807#section-3.1" // ده بيتبع معيار عالمي لرسائل الأخطاء
            //    };

            //    // بنقول للـ ASP.NET Core إن الرد هيكون 400 مع تفاصيل المشكلة
            //    context.Result = new BadRequestObjectResult(problemDetails);
            //    context.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;

            //    // مهم جداً: بنقول للـ ASP.NET Core إننا خلاص اتعاملنا مع الـ Exception ده،
            //    // عشان ميطلعش أي أخطاء تانية أو يرجع 500.
            //    context.ExceptionHandled = true;
            //} 
            #endregion

            var response = context.HttpContext.Response;
            response.ContentType = "application/json";
            var error = context.Exception;
            var responseModel = new Response<string>() { Succeeded = false, Message = error.Message };

            switch (context.Exception)
            {
                case UnauthorizedAccessException e:
                    // custom application error
                    responseModel.Message = error.Message;
                    responseModel.StatusCode = HttpStatusCode.Unauthorized;
                    response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    break;

                case ValidationException e:
                    // custom validation error
                    responseModel.Message = error.Message;
                    responseModel.StatusCode = HttpStatusCode.UnprocessableEntity;
                    response.StatusCode = (int)HttpStatusCode.UnprocessableEntity;
                    break;
                case KeyNotFoundException e:
                    // not found error
                    responseModel.Message = error.Message; ;
                    responseModel.StatusCode = HttpStatusCode.NotFound;
                    response.StatusCode = (int)HttpStatusCode.NotFound;
                    break;

                case DbUpdateException e:
                    // can't update error
                    responseModel.Message = e.Message;
                    responseModel.StatusCode = HttpStatusCode.BadRequest;
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    break;


                default:
                    // unhandled error
                    responseModel.Message = error.Message;
                    responseModel.StatusCode = HttpStatusCode.InternalServerError;
                    response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    break;

            }
            context.ExceptionHandled = true;

            var result = JsonSerializer.Serialize(responseModel);

            await response.WriteAsync(result);



        }


    }
}
