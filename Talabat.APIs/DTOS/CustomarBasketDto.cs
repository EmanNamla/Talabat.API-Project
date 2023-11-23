using System.ComponentModel.DataAnnotations;
using Talabat.Core.Entites;

namespace Talabat.APIs.DTOS
{
    public class CustomarBasketDto
    {
        [Required]
        public string Id { get; set; }
        public string? PaymentIntentId { get; set; }

        public string? ClientSecret { get; set; }

        public int? DelivaryMethodId { get; set; }
        public List<BasketItemDto> Items { get; set; }
    }
}
