using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;
using Talabat.APIs.DTOS;
using Talabat.APIs.Error;
using Talabat.Core.Entites;
using Talabat.Core.Repositores;

namespace Talabat.APIs.Controllers
{
    public class BasketController : APIBaseController
    {
        private readonly IBasketRepository basketRepository;
        private readonly IMapper mapper;

        public BasketController(IBasketRepository basketRepository,IMapper mapper)
        {
            this.basketRepository = basketRepository;
            this.mapper = mapper;
        }
        [HttpGet]
        public async Task<ActionResult<CustomarBasket>> GetCustomarBasket(string BasketId)
        {
            var Basket = await basketRepository.GetBasketAsynk(BasketId);
            return Basket is null ? new CustomarBasket(BasketId) : Basket;
        }

        [HttpPost]
        public async Task<ActionResult<CustomarBasket>> UpdateBasket(CustomarBasketDto basket)
        {
            var mapped = mapper.Map<CustomarBasketDto, CustomarBasket>(basket);
            var CreateOrUpdate = await basketRepository.UpdateBasketAsync(mapped);
            if (CreateOrUpdate is null) return BadRequest(new APIResponse(400));
            return Ok(CreateOrUpdate);
        }
        [HttpDelete]
        public async Task<ActionResult<bool>> DeleteBasket(string BasketId)
        {
            return await basketRepository.DeleteBasketAsynk(BasketId);

        }
    }
}

