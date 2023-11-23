using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entites.Order_Aggregate;

namespace Talabat.Core.Services
{
    public interface IOrderService
    {
        public Task<Order?> CreateOrderAsync(string buyerEmail, string basketId, int DeliveryMethodId, Address ShippingAddress);
        Task<IReadOnlyList<Order>> GetOrderForSpecifcUserAsync(string buyerEmail);

        Task<Order> GetOrderByIdForSpecifcUserAsync(string buyerEmail, int OrderId);
    }
}
