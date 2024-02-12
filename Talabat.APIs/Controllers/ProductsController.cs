using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Talabat.APIs.DTOS;
using Talabat.APIs.Error;
using Talabat.APIs.Helpers;
using Talabat.Core;
using Talabat.Core.Entites;
using Talabat.Core.Repositores;
using Talabat.Core.Specifications;

namespace Talabat.APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : APIBaseController
    {
        private readonly IUnitofwork unitofwork;
        private readonly IMapper mapper;

        public ProductsController(IUnitofwork unitofwork,IMapper mapper)
        {
            this.unitofwork = unitofwork;
            this.mapper = mapper;
        }
        //[Authorize]
        [CachedAttribute(300)]
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<Pagination<ProductToReturnDto>>>> GetAllProducts([FromQuery]ProductSpecParams specParams)
        {
            var spec =new ProductWithBrandAndTypeSpecification(specParams);
            var products = await unitofwork.Repository<Product>().GetAllWithSpecAsync(spec);
            var ProductMapped=mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductToReturnDto>>(products);
            var CountSpec=new ProductWithFilterationForCountSpecification(specParams);
            var count =await unitofwork.Repository<Product>().GetCountWithSpecAsync(CountSpec);
            var Pigination = new Pagination<ProductToReturnDto>()
            {
                PageSize= specParams.PageSize,
                PageIndex= specParams.PageIndex,
                Data = ProductMapped,
                Count = count
            };
            
            return Ok(Pigination);
        }
        [CachedAttribute(300)]
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ProductToReturnDto), 200)]
        [ProducesResponseType(typeof(APIResponse),StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ProductToReturnDto>> GetProductById(int id)
        {
            var spec =new ProductWithBrandAndTypeSpecification(id);
            var products = await unitofwork.Repository<Product>().GetByIdEntitySpecAsync(spec);
            if(products ==null) return NotFound( new APIResponse(404));
            var ProductMapped = mapper.Map<Product, ProductToReturnDto>(products);
            return Ok(ProductMapped);
        }
        [CachedAttribute(300)]
        [HttpGet("Brands")]
        public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetAllBrands()
        {
            var Brand =await unitofwork.Repository<ProductBrand>().GatAllAsync();
            return Ok(Brand);
        }
        [CachedAttribute(300)]
        [HttpGet("Types")]
        public async Task<ActionResult<IReadOnlyList<ProductType>>> GetAllTypes()
        {
            var Brand = await unitofwork.Repository<ProductType>().GatAllAsync();
            return Ok(Brand);
        }
    }
}
