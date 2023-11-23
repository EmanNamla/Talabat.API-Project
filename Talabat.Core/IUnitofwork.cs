using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entites;
using Talabat.Core.Repositores;

namespace Talabat.Core
{
    public interface IUnitofwork:IAsyncDisposable
    {
        public IGenericRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity;
        public  Task<int> CompleteAsync();
    }
}
