using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Core.Entites.Order_Aggregate
{
    public class Order:BaseEntity
    {
        public Order()
        {

        }
        public Order(string buyerEmail, Address shippingAddress, DeliveryMethod deliveryMethod, ICollection<OrderItem> items, decimal subTotal,string paymentintentId)
        {
            BuyerEmail = buyerEmail;
            shipToAddress = shippingAddress;
            DeliveryMethod = deliveryMethod;
            Items = items;
            SubTotal = subTotal;
            PaymentIntentId = paymentintentId;
        }

        public string BuyerEmail { get; set; }

        public DateTimeOffset OrderDate { get; set; }=DateTimeOffset.Now;

        public OrderStatus Status { get; set; } = OrderStatus.Pending;

        public Address shipToAddress { get; set; }

        public DeliveryMethod DeliveryMethod { get; set; }

        public ICollection<OrderItem> Items { get; set;}=new HashSet<OrderItem>();

        public decimal SubTotal { get; set; }

        public decimal GetTotal () => SubTotal + DeliveryMethod.Cost;

        public string PaymentIntentId { get; set; } 


    }
}
