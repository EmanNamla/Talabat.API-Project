using Microsoft.Extensions.Configuration;
using Stripe;
using Stripe.FinancialConnections;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core;
using Talabat.Core.Entites;
using Talabat.Core.Entites.Order_Aggregate;
using Talabat.Core.Repositores;
using Talabat.Core.Services;
using Talabat.Core.Specifications.OrderSpec;
using Product = Talabat.Core.Entites.Product;

namespace Talabat.Service
{
    public class PaymentService : IPaymentService
    {
        private readonly IConfiguration configuration;
        private readonly IBasketRepository basketRepository;
        private readonly IUnitofwork unitofwork;

        public PaymentService(IConfiguration configuration,IBasketRepository basketRepository,IUnitofwork unitofwork)
        {
           
            this.configuration = configuration;
            this.basketRepository = basketRepository;
            this.unitofwork = unitofwork;
        }

        public async Task<CustomarBasket> CreateOrUpdatePaymentIntent(string BasketId)
        {
            StripeConfiguration.ApiKey = configuration["StripeKey:Secretkey"];

            var Basket = await basketRepository.GetBasketAsynk(BasketId);
            if (Basket == null) return null;

            var ShippingPrice = 0M;
            if (Basket.DelivaryMethodId.HasValue)
            {
                var DelivaryMethods = await unitofwork.Repository<DeliveryMethod>().GetByIdAsync(Basket.DelivaryMethodId.Value);
                ShippingPrice = DelivaryMethods.Cost;
            }
            if (Basket.Items.Count > 0)
            {
                foreach (var item in Basket.Items)
                {
                    var product = await unitofwork.Repository<Product>().GetByIdAsync(item.Id);
                    if (item.Price != product.Price)
                        item.Price = product.Price;
                }
            }
            var subTotal=Basket.Items.Sum(Item=>Item.Price*Item.Quantity);

            //create PaymentIntent
            var Services = new PaymentIntentService();
            PaymentIntent paymentIntent;
            if(string.IsNullOrEmpty(Basket.PaymentIntentId))
            {
                var options = new PaymentIntentCreateOptions()
                {
                    Amount=(long)subTotal *100 + (long) ShippingPrice *100,
                    Currency="usd",
                    PaymentMethodTypes=new List<string>() { "card"}
                };
                paymentIntent = await Services.CreateAsync(options);
                Basket.PaymentIntentId=paymentIntent.Id;
                Basket.ClientSecret=paymentIntent.ClientSecret;
            }
            else
            {
                var options = new PaymentIntentUpdateOptions()
                {
                    Amount = (long)subTotal * 100 + (long)ShippingPrice * 100,
               
                };
                paymentIntent = await Services.UpdateAsync(Basket.PaymentIntentId, options);
                Basket.PaymentIntentId=paymentIntent.Id;
                Basket.ClientSecret= Basket.ClientSecret;
            }
            await basketRepository.UpdateBasketAsync(Basket);
            return Basket;
        }

        public async Task<Order> UpdatePaymentIntentToSucceedOrFeiled(string PaymentIntentId, bool flag)
        {
            var spec = new OrderPaymentintentId(PaymentIntentId);
            var order =await unitofwork.Repository<Order>().GetByIdEntitySpecAsync(spec);
            if (flag)
            {
                order.Status = OrderStatus.PaymentReceived;
            }
            else
                order.Status = OrderStatus.PaymentFalied;
            unitofwork.Repository<Order>().Update(order);
            await unitofwork.CompleteAsync();
            return order;
        }
    }
}
