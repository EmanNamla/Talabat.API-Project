using System.ComponentModel.DataAnnotations;
using Talabat.Core.Entites.Identity;

namespace Talabat.APIs.DTOS
{
    public class OrderDto
    {
        [Required]
        public string BasketId { get; set; }

        public AddresDto shipToAddress { get; set; }

        public int DeliveryMethodId { get; set; }
    }
}
