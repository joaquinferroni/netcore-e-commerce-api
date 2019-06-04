using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace e_commerce.webapi.Helpers
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate next;

        public ErrorHandlingMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext context /* other dependencies */)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var code = HttpStatusCode.InternalServerError; // 500 if unexpected

            if (exception is NotFoundException) code = HttpStatusCode.NotFound;
            if (exception is ArgumentException || exception is ArgumentNullException) code= HttpStatusCode.BadRequest;
            else if (exception is Exception) code = HttpStatusCode.BadRequest;

            var result = JsonConvert.SerializeObject(new { error = exception.Message  ,showErrorMessage= false });
            if(exception is ArgumentException || exception is ArgumentNullException || exception is NotFoundException){
                result= JsonConvert.SerializeObject(new { error = exception.Message ,showErrorMessage= true});
            }
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)code;
            return context.Response.WriteAsync(result);
        }
    }

    public class NotFoundException:Exception
    {
        public NotFoundException(string message):base(message){}
    }
}