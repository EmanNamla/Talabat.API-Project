using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entites;
using Talabat.Core.Specifications;

namespace Talabat.Core.Repositores
{
    public interface IGenericRepository<T>where T:BaseEntity
    {
        #region BefourSpecification
        Task<IReadOnlyList<T>> GatAllAsync();

        Task<T> GetByIdAsync(int id);

        Task AddAsync(T item);

        void Delete(T item);

        void Update (T item);
        #endregion

        #region AfterSpecification
        Task<IReadOnlyList<T>> GetAllWithSpecAsync(ISpecifications<T>Spec);

        Task<T> GetByIdEntitySpecAsync(ISpecifications<T> Spec);

        Task<int>GetCountWithSpecAsync(ISpecifications<T> Spec);
        #endregion
    }
}
