using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entites.Order_Aggregate;

namespace Talabat.Core.Specifications.OrderSpec
{
    public class OrderPaymentintentId:BaseSpecifications<Order>
    {
        public OrderPaymentintentId(string PaymentIntentId):base(o=>o.PaymentIntentId==PaymentIntentId)
        {

        }
    }
}
