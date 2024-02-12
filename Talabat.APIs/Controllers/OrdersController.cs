using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;
using Talabat.APIs.DTOS;
using Talabat.APIs.Error;
using Talabat.APIs.Helpers;
using Talabat.Core;
using Talabat.Core.Entites.Order_Aggregate;
using Talabat.Core.Services;
using Talabat.Repository;

namespace Talabat.APIs.Controllers
{

    public class OrdersController : APIBaseController
    {
        private readonly IOrderService orderService;
        private readonly IMapper mapper;
        private readonly IUnitofwork unitofwork;

        public OrdersController(IOrderService orderService, IMapper mapper,IUnitofwork unitofwork)
        {
            this.orderService = orderService;
            this.mapper = mapper;
            this.unitofwork = unitofwork;
        }
        [ProducesResponseType(typeof(OrderToReturnDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Order), StatusCodes.Status400BadRequest)]
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<OrderToReturnDto>> CreateOrder(OrderDto orderDto)
        {
            var BuyerEmail = User.FindFirstValue(ClaimTypes.Email);
            var MappedAddress = mapper.Map<AddresDto, Address>(orderDto.shipToAddress);
            var order = await orderService.CreateOrderAsync(BuyerEmail, orderDto.BasketId, orderDto.DeliveryMethodId, MappedAddress);
            if (order == null) { return BadRequest(new APIResponse(400, "This is a problem")); }
            var MappedOrder = mapper.Map<Order,OrderToReturnDto>(order);
            return Ok(MappedOrder);
        }


        [ProducesResponseType(typeof(IReadOnlyList<OrderToReturnDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Order), StatusCodes.Status400BadRequest)]
        [HttpGet]
        [Authorize]
        [CachedAttribute(300)]
        public async Task<ActionResult<IReadOnlyList<OrderToReturnDto>>> GetOrderForUser()
        {
            var BuyerEmail = User.FindFirstValue(ClaimTypes.Email);
            var order = await orderService.GetOrderForSpecifcUserAsync(BuyerEmail);
            if (order is null) return NotFound(new APIResponse(404, "There is No Order For This User"));
            var MappedOrder = mapper.Map< IReadOnlyList< Order>,IReadOnlyList< OrderToReturnDto>>(order);
            return Ok(MappedOrder);
        }

        [ProducesResponseType(typeof(Order), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Order), StatusCodes.Status400BadRequest)]
        [HttpGet("{id}")]
        [Authorize]
        [CachedAttribute(300)]
        public async Task<ActionResult<Order>>GetOrderByIdForSpecificUser(int id)
        {
            string BuyerEmail=User.FindFirstValue(ClaimTypes.Email);
            var order=await orderService.GetOrderByIdForSpecifcUserAsync(BuyerEmail, id);
            if (order is null) return NotFound(new APIResponse(404, $"There is No Order WithId={id} For This User"));
            return Ok(order);
        }

        [ProducesResponseType(typeof(DeliveryMethod), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(DeliveryMethod), StatusCodes.Status400BadRequest)]
        [HttpGet("deliveryMethods")]
        [CachedAttribute(300)]
        public async Task<ActionResult<IReadOnlyList<DeliveryMethod>>> GetDelivaryMethod()
        {
            var Delivarymethods =await unitofwork.Repository<DeliveryMethod>().GatAllAsync();
           return Ok(Delivarymethods);
        }
        

    }
}
