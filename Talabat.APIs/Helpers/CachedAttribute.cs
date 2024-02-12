using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Text;
using Talabat.Core.Services;

namespace Talabat.APIs.Helpers
{
    public class CachedAttribute:Attribute,IAsyncActionFilter
    {
        private readonly int expireTimeInSeconds;

        public CachedAttribute(int ExpireTimeInSeconds)
        {
            expireTimeInSeconds = ExpireTimeInSeconds;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var CacheService=context.HttpContext.RequestServices.GetRequiredService<IResponseCacheService>();

            var CacheKey=GenerateCachKeyFromRequest(context.HttpContext.Request);
            //GetCachedResponse
            var CachedRespnse =await CacheService.GetCachedResponseAsync(CacheKey);

            if(!string.IsNullOrEmpty(CachedRespnse))
            {
                var ContentResult = new ContentResult()
                {
                    Content = CachedRespnse,
                    ContentType = "application/json",
                    StatusCode = 200
                };
                context.Result= ContentResult;
                return;
            }

            var ExecutedEndPointContext = await next.Invoke();
            if( ExecutedEndPointContext.Result is OkObjectResult result)
            {
                await CacheService.SetCacheResponseAsync(CacheKey,result.Value,TimeSpan.FromSeconds(expireTimeInSeconds));
            }

        }

        private string GenerateCachKeyFromRequest(HttpRequest request)
        {
            var KeyBuilder = new StringBuilder();
            KeyBuilder.Append(request.Path);
            foreach (var (key,value) in request.Query.OrderBy(x=>x.Key))
            {
                KeyBuilder.Append($"|{key}-{value}");
            }
            return KeyBuilder.ToString();

        }
    }
}
