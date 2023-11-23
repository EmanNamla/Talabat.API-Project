using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entites;
using Talabat.Core.Repositores;
using Talabat.Core.Specifications;
using Talabat.Repository.Data;

namespace Talabat.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly StoreDbContext dbContext;

        public GenericRepository(StoreDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        #region BefourSpecification
        public async Task<IReadOnlyList<T>> GatAllAsync()
        {
            if (typeof(T) == typeof(Product))
            {
                return (IReadOnlyList<T>)await dbContext.Products.Include(p => p.ProductBrand).Include(p => p.ProductType).ToListAsync();
            }
            else

                return await dbContext.Set<T>().ToListAsync();
        }


        public async Task<T> GetByIdAsync(int id)
        {
            return await dbContext.Set<T>().FindAsync(id);

        }

        public async Task AddAsync(T item)=>  await dbContext.Set<T>().AddAsync(item);
        
        #endregion 
        public async Task<IReadOnlyList<T>> GetAllWithSpecAsync(ISpecifications<T> Spec)
        {
            return await ApplySpecification(Spec).ToListAsync();
        }
        public async Task<T> GetByIdEntitySpecAsync(ISpecifications<T> Spec)
        {
            return await ApplySpecification(Spec).FirstOrDefaultAsync();
        }

        public async Task<int> GetCountWithSpecAsync(ISpecifications<T> Spec)
        {
            return await  ApplySpecification(Spec).CountAsync();
        }

        private IQueryable<T> ApplySpecification(ISpecifications<T> Spec)
        {
          return  SpecificationEvalutor<T>.GetQuery(dbContext.Set<T>(), Spec);
        }

        public void Delete(T item)
        {
            dbContext.Set<T>().Remove(item);
        }

        public void Update(T item)
        {
            dbContext.Set<T>().Update(item);
        }
    }
}
