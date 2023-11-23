using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core;
using Talabat.Core.Entites;
using Talabat.Core.Repositores;
using Talabat.Repository.Data;

namespace Talabat.Repository
{
    public class Unitofwork : IUnitofwork
    {
        private readonly StoreDbContext dbContext;
        private readonly Hashtable _repository;
        public Unitofwork(StoreDbContext dbContext)
        {
            this.dbContext = dbContext;
            this._repository = new Hashtable();
        }
        public async Task<int> CompleteAsync()
        { return await dbContext.SaveChangesAsync(); }

        public async ValueTask DisposeAsync()
        =>await dbContext.DisposeAsync();

        public IGenericRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity
        {
           var type=typeof(TEntity).Name;
            if(!_repository.ContainsKey(type))
            {
                var Repository =new GenericRepository<TEntity>(dbContext);
                _repository.Add(type, Repository);
            }
            return _repository[type] as IGenericRepository<TEntity>;
        }
    }
}
