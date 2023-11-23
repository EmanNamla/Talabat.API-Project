using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core;
using Talabat.Core.Entites;
using Talabat.Core.Entites.Order_Aggregate;
using Talabat.Core.Repositores;
using Talabat.Core.Services;
using Talabat.Core.Specifications.OrderSpec;

namespace Talabat.Service
{
    public class OrderService : IOrderService
    {
        private readonly IBasketRepository basketRepository;
        private readonly IUnitofwork unitofwork;
        private readonly IPaymentService paymentService;

        public OrderService(IBasketRepository basketRepository,IUnitofwork unitofwork,IPaymentService paymentService)
        {
            this.basketRepository = basketRepository;
            this.unitofwork = unitofwork;
            this.paymentService = paymentService;
        }
        public async Task<Order?> CreateOrderAsync(string buyerEmail, string basketId, int DeliveryMethodId, Address ShippingAddress)
        {
            //1.Get Basket From BasketRepo 
            var Basket =await basketRepository.GetBasketAsynk(basketId);

            //2.Get Selected Items At Basket From ProductRepo
            var OrderItems=new List<OrderItem>();
            if(Basket?.Items.Count > 0)
            {
                foreach(var item in Basket.Items)
                {
                    var Product =await unitofwork.Repository<Product>().GetByIdAsync(item.Id);
                    var ProductItemOrdered=new ProductItemOrdered(item.Id,Product.Name,Product.PictureUrl);
                    var OrderItem = new OrderItem(ProductItemOrdered, Product.Price, item.Quantity);
                    OrderItems.Add(OrderItem);
                }
            }

            //3.Calculate SubTotal  Price *Quantity
            var SubTotal = OrderItems.Sum(p => p.Price * p.Quantity);

            //4.Get Delivary Method From DelivaryMethod Repo
            var DelivaryMethod =await unitofwork.Repository<DeliveryMethod>().GetByIdAsync(DeliveryMethodId);

            //الخطوه دي بتأكد ان في Paymentموجود قبل كده ولالا 
            //Because Payment Must Be Unique
            //بقوله لو موجود قبل كده امسحهولي واعملي واحد جديد
            var spec = new OrderPaymentintentId(Basket.PaymentIntentId);
            var Exorder =await unitofwork.Repository<Order>().GetByIdEntitySpecAsync(spec);
            if(Exorder != null)
            {
                unitofwork.Repository<Order>().Delete(Exorder);
                await paymentService.CreateOrUpdatePaymentIntent(basketId);
            }

            //5.Create Order
            var Order=new Order(buyerEmail, ShippingAddress, DelivaryMethod, OrderItems,SubTotal,Basket.PaymentIntentId);

            //6.AddOrder Localy
            await unitofwork.Repository<Order>().AddAsync(Order);

            //7.Save Order To Db
            var result = await unitofwork.CompleteAsync();
            if(result <=0)
            
                return null;
            
            return Order;
        }

        public async Task<Order> GetOrderByIdForSpecifcUserAsync(string buyerEmail, int OrderId)
        {
            var spec = new OrderSpecification(OrderId, buyerEmail);
            var order=await unitofwork.Repository<Order>().GetByIdEntitySpecAsync(spec);
            return order;
        }

        public Task<IReadOnlyList<Order>> GetOrderForSpecifcUserAsync(string buyerEmail)
        {
            var spec = new OrderSpecification(buyerEmail);
            var order=unitofwork.Repository<Order>().GetAllWithSpecAsync(spec);
            return order;
        }
    }
}
