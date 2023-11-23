using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entites;

namespace Talabat.Core.Repositores
{
    public interface IBasketRepository
    {
        Task<bool> DeleteBasketAsynk(string basketId);

        Task<CustomarBasket?> GetBasketAsynk(string basketId);

        Task<CustomarBasket?> UpdateBasketAsync(CustomarBasket basket);
    }
}
