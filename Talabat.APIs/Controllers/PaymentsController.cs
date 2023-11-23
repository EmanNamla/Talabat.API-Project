using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using Talabat.APIs.DTOS;
using Talabat.APIs.Error;
using Talabat.Core.Entites;
using Talabat.Core.Services;

namespace Talabat.APIs.Controllers
{
    
    public class PaymentsController : APIBaseController
    {
        private readonly IPaymentService paymentService;
        private readonly IMapper mapper;
        const string endpointSecret = "whsec_02820cf99741ce1d171aa47f16aa6e26add4c40e05151eb219d8afb3d9e9a58e";
        public PaymentsController(IPaymentService paymentService,IMapper mapper)
        {
            this.paymentService = paymentService;
            this.mapper = mapper;
        }
        [ProducesResponseType(typeof(CustomarBasketDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(CustomarBasketDto), StatusCodes.Status400BadRequest)]
        [HttpPost("{basketId}")]
        [Authorize]
        public async Task<ActionResult<CustomarBasketDto>>CreateOrUpdatePaymentIntent(string basketId)
        {
            var CustomerBasket=await paymentService.CreateOrUpdatePaymentIntent(basketId);
            if(CustomerBasket==null) { return BadRequest(new APIResponse(400)); }
            var MappedCustomerBasket = mapper.Map<CustomarBasket, CustomarBasketDto>(CustomerBasket);
            return Ok(MappedCustomerBasket);

        }
        [HttpPost("webhook")]
        public async Task<IActionResult> UpdatePaymentIntentSucessOrFiled()
        {
           
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
            try
            {
             
                var stripeEvent = EventUtility.ConstructEvent(json,
                    Request.Headers["Stripe-Signature"], endpointSecret);
                var paymentIntent = stripeEvent.Data.Object as PaymentIntent;
                // Handle the event
                if (stripeEvent.Type == Events.PaymentIntentPaymentFailed)
                {
                  await  paymentService.UpdatePaymentIntentToSucceedOrFeiled(paymentIntent.Id, false);
                }
                else if (stripeEvent.Type == Events.PaymentIntentSucceeded)
                {
                    await paymentService.UpdatePaymentIntentToSucceedOrFeiled(paymentIntent.Id, true);
                }
                // ... handle other event types
               

                return Ok();
            }
            catch (StripeException e)
            {
                return BadRequest();
            }
        }

    }
}
