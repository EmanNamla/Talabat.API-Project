using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entites;
using Talabat.Core.Specifications;

namespace Talabat.Repository
{
    public static class SpecificationEvalutor<T>where T : BaseEntity
    {
        public static IQueryable<T> GetQuery(IQueryable<T> InputQuery,ISpecifications<T> spec)
        {
            var Query = InputQuery;
            if(spec.Criteria is not null)
            {
                Query = Query.Where(spec.Criteria);
            }
            if(spec.OrderByAsyning is not null)
            {
                Query=Query.OrderBy(spec.OrderByAsyning);
            }
            if (spec.OrderByDsyning is not null)
            {
                Query = Query.OrderByDescending(spec.OrderByDsyning);
            }
            if(spec.EnablePagination)
            {
                Query=Query.Skip(spec.Skip).Take(spec.Take);
            }

            Query =spec.Includes.Aggregate(Query,(CurrentQuery,IncludeExpression)=>CurrentQuery.Include(IncludeExpression));
            return Query;
        }
    }
}
