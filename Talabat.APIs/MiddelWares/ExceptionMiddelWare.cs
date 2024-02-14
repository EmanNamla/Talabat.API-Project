using System.Net;
using System.Text.Json;
using Talabat.APIs.Error;

namespace Talabat.APIs.MiddelWares
{
    public class ExceptionMiddelWare
    {
        private readonly RequestDelegate next;
        private readonly ILogger<ExceptionMiddelWare> logger;
        private readonly IHostEnvironment env;

        public ExceptionMiddelWare(RequestDelegate Next,ILogger<ExceptionMiddelWare> logger,IHostEnvironment env )
        {
            next = Next;
            this.logger = logger;
            this.env = env;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await next.Invoke(context);
            }
            catch(Exception ex)
            {
                logger.LogError(ex, ex.Message);
                context.Response.ContentType= "application/json";
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
             
                var Response= env.IsDevelopment()? new ApiExptionResponse((int)HttpStatusCode.InternalServerError, ex.Message, ex.StackTrace.ToString()):
                    new ApiExptionResponse((int)HttpStatusCode.InternalServerError, ex.Message, ex.StackTrace.ToString());
                var options = new JsonSerializerOptions()
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                };
                var jsonResonse=JsonSerializer.Serialize(Response, options);
                context.Response.WriteAsync(jsonResonse);
            }
        }
    }
}
