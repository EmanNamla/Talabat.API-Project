using AutoMapper;
using Talabat.APIs.DTOS;
using Talabat.Core.Entites;
using Talabat.Core.Entites.Identity;
using Talabat.Core.Entites.Order_Aggregate;
using IdentityAddress = Talabat.Core.Entites.Identity.Address;
using ShippingAddress = Talabat.Core.Entites.Order_Aggregate.Address;

namespace Talabat.APIs.Helpers
{
    public class MappingProfiles:Profile
    {
        public MappingProfiles()
        {
            CreateMap<Product, ProductToReturnDto>()
                .ForMember(p => p.ProductBrand, o => o.MapFrom(p => p.ProductBrand.Name))
                .ForMember(p => p.ProductType, o => o.MapFrom(p => p.ProductType.Name))
                .ForMember(p => p.PictureUrl, o => o.MapFrom<ProductPictureUrlResolver>());
            CreateMap<AddresDto, ShippingAddress>().ReverseMap();
            CreateMap<IdentityAddress, AddresDto>().ReverseMap();
            CreateMap<BasketItemDto, BasketItem>().ReverseMap();
            CreateMap<CustomarBasketDto, CustomarBasket>().ReverseMap();

            CreateMap<Order, OrderToReturnDto>()
                .ForMember(OR => OR.DeliveryMethodName, o => o.MapFrom(o => o.DeliveryMethod.ShortName))
                .ForMember(OR => OR.DeliveryMethodCost, o => o.MapFrom(o => o.DeliveryMethod.Cost));

            CreateMap<OrderItem, OrderItemDto>()
                .ForMember(OR => OR.ProductId, o => o.MapFrom(p => p.Product.ProductId))
                .ForMember(OR => OR.ProductName, o => o.MapFrom(p => p.Product.ProductName))
                 .ForMember(OR => OR.PictureUrl, o => o.MapFrom(p=>p.Product.PictureUrl))
                .ForMember(OR => OR.PictureUrl, o => o.MapFrom<OrderPictureUrlResolver>());
        }
    }
}
