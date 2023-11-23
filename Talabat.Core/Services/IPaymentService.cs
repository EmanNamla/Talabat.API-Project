using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entites;
using Talabat.Core.Entites.Order_Aggregate;

namespace Talabat.Core.Services
{
    public interface IPaymentService
    {
        Task<CustomarBasket> CreateOrUpdatePaymentIntent(string BasketId);

        Task<Order> UpdatePaymentIntentToSucceedOrFeiled(string PaymentIntentId, bool flag);
    }
}
