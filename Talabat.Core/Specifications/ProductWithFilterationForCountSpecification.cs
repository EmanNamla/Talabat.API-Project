using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entites;

namespace Talabat.Core.Specifications
{
    public class ProductWithFilterationForCountSpecification: BaseSpecifications<Product>
    {
        public ProductWithFilterationForCountSpecification(ProductSpecParams Params)
            : base(p =>
            (string.IsNullOrEmpty(Params.Search) || p.Name.ToLower().Contains(Params.Search)) &&
            (!Params.brandId.HasValue || p.ProductBrandId == Params.brandId) && (!Params.typeId.HasValue || p.ProductTypeId == Params.typeId))
        {

        }

    }
}
