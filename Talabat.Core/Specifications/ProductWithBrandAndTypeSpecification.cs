using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entites;

namespace Talabat.Core.Specifications
{
    public class ProductWithBrandAndTypeSpecification:BaseSpecifications<Product>
    {
        public ProductWithBrandAndTypeSpecification(ProductSpecParams Params)
            : base(p =>
            (string.IsNullOrEmpty(Params.Search) || p.Name.ToLower().Contains(Params.Search))&&
            (!Params.brandId.HasValue||p.ProductBrandId== Params.brandId) &&(!Params.typeId.HasValue||p.ProductTypeId==Params.typeId))
        {
            Includes.Add(p => p.ProductBrand);
            Includes.Add(p => p.ProductType);

            if (!string.IsNullOrEmpty(Params.sort))
            {
                switch (Params.sort)
                {
                    case "PriceA":
                        OrderByAsyn(p => p.Price);
                        break;
                    case "PriceD":
                        OrderByDsyn(p =>p.Price);
                        break;
                        default:
                        OrderByAsyn(p => p.Name);
                        break;
                }
            }

            ApplyPagination(Params.PageSize * (Params.PageIndex - 1), Params.PageSize);
        }

        public ProductWithBrandAndTypeSpecification(int id):base(p=>p.Id==id)
        {
            Includes.Add(p => p.ProductBrand);
            Includes.Add(p => p.ProductType);
        }
    }
}
