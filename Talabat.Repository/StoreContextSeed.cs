using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Talabat.Core.Entites;
using Talabat.Core.Entites.Order_Aggregate;
using Talabat.Repository.Data;

namespace Talabat.Repository
{
    public class StoreContextSeed
    {
        public static async Task SeedAsync(StoreDbContext dbContext)
        {
            if(!dbContext.ProductTypes.Any()) 
            { 
              var BrandData = File.ReadAllText("../Talabat.Repository/Data/DataSeed/types.json");
                var Brands=JsonSerializer.Deserialize<List<ProductType>>(BrandData);
                if(Brands?.Count() > 0)
                {
                    foreach(var Brand in Brands)
                    {
                       await dbContext.Set<ProductType>().AddAsync(Brand);
                    }
                   await dbContext.SaveChangesAsync();
                }
            }

            if(!dbContext.ProductBrands.Any() )
            {
                var BrandData = File.ReadAllText("../Talabat.Repository/Data/DataSeed/brands.json");
                var Brands=JsonSerializer.Deserialize<List<ProductBrand>>(BrandData);
                if(Brands?.Count>0)
                {
                    foreach(var Brand in Brands )
                    {
                      await  dbContext.Set<ProductBrand>().AddAsync(Brand);
                    }
                    await dbContext.SaveChangesAsync();
                }
            }

            if (!dbContext.Products.Any())
            {
                var BrandData = File.ReadAllText("../Talabat.Repository/Data/DataSeed/products.json");
                var Brands = JsonSerializer.Deserialize<List<Product>>(BrandData);
                if (Brands?.Count > 0)
                {
                    foreach (var Brand in Brands)
                    {
                        await dbContext.Set<Product>().AddAsync(Brand);
                    }
                    await dbContext.SaveChangesAsync();
                }
            }
            if (!dbContext.DeliveryMethods.Any())
            {
                var DelivaryMethodsData = File.ReadAllText("../Talabat.Repository/Data/DataSeed/delivery.json");
                var DelivaryMethods = JsonSerializer.Deserialize<List<DeliveryMethod>>(DelivaryMethodsData);
                if (DelivaryMethods?.Count > 0)
                {
                    foreach (var DelivaryMethod in DelivaryMethods)
                    {
                        await dbContext.Set<DeliveryMethod>().AddAsync(DelivaryMethod);
                    }
                    await dbContext.SaveChangesAsync();
                }
            }



        }

    }
}
