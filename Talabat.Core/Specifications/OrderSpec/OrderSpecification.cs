using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entites;
using Talabat.Core.Entites.Order_Aggregate;

namespace Talabat.Core.Specifications.OrderSpec
{
    public class OrderSpecification:BaseSpecifications<Order>
    {
        public OrderSpecification(string email):base(o=>o.BuyerEmail==email)
        {
            Includes.Add(o => o.DeliveryMethod);
            Includes.Add(o => o.Items);
            OrderByDsyn(o => o.OrderDate);
        }

        public OrderSpecification(int id,string email):base(o=>o.Id==id&&o.BuyerEmail==email)
        {
            Includes.Add(o => o.DeliveryMethod);
            Includes.Add(o => o.Items);
            OrderByDsyn(o => o.OrderDate);
        }
        

    }
}
