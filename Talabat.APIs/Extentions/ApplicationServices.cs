using Microsoft.AspNetCore.Mvc;
using Talabat.APIs.Error;
using Talabat.APIs.Helpers;
using Talabat.Core;
using Talabat.Core.Repositores;
using Talabat.Core.Services;
using Talabat.Repository;
using Talabat.Service;

namespace Talabat.APIs.Extentions
{
    public static class ApplicationServices
    {
        public static  IServiceCollection  AddApplicationServies(this IServiceCollection Services)
        {
            Services.AddScoped(typeof(IResponseCacheService), typeof(ResponseCacheService));
            Services.AddScoped(typeof(IPaymentService), typeof(PaymentService));
            Services.AddScoped(typeof(IUnitofwork),typeof(Unitofwork));
            Services.AddScoped(typeof(IOrderService), typeof(OrderService));
            Services.AddScoped(typeof(IBasketRepository), typeof(BasketRepository));
            //Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

            Services.AddAutoMapper(typeof(MappingProfiles));

            #region ValidationErrorHandling
           Services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = (actionContext) =>
                {
                    var errors = actionContext.ModelState.Where(p => p.Value.Errors.Count() > 0)
                    .SelectMany(p => p.Value.Errors).Select(e => e.ErrorMessage).ToArray();

                    var validationErrorResonse = new ApiValidationErrorResponse()
                    {
                        Errors = errors
                    };
                    return new BadRequestObjectResult(validationErrorResonse);
                };
            });
            #endregion

            return Services;

        }
    }
}
