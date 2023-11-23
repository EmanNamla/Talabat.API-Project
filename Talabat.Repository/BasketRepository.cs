using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using Talabat.Core.Entites;
using Talabat.Core.Repositores;

namespace Talabat.Repository
{
    public class BasketRepository : IBasketRepository
    {
        private readonly IDatabase database ;

        public BasketRepository(IConnectionMultiplexer connection)
        {
            database = connection.GetDatabase();
        }
        public async Task<bool> DeleteBasketAsynk(string basketId)
        {
            return await database.KeyDeleteAsync(basketId);
        }

        public async Task<CustomarBasket?> GetBasketAsynk(string basketId)
        {
           var Basket= await database.StringGetAsync(basketId);
            if (Basket.IsNull) return null;

            return  JsonSerializer.Deserialize<CustomarBasket>(Basket);
        }

        public async Task<CustomarBasket?> UpdateBasketAsync(CustomarBasket basket)
        {
            var JsonBasket = JsonSerializer.Serialize(basket);
            var CreateOrUpdate =await database.StringSetAsync(basket.Id, JsonBasket, TimeSpan.FromDays(1));
            if (!CreateOrUpdate) return null;
             return await GetBasketAsynk(basket.Id);
        }
    }
}
