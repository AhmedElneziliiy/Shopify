using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;
using Core.Specifications;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class SpecificationEvaluator<TEntity> where TEntity :BaseEntity
    {//that class applying specifications you had made and return query to push to repository
        public static IQueryable<TEntity> GetQuery
        (IQueryable<TEntity> inputQuery,ISpecification<TEntity> spec) 
        {
            var query=inputQuery;

            if(spec.Criteria != null)
            {
                query=query.Where(spec.Criteria);
            }

            if (spec.OrderBy != null)
            {
                query = query.OrderBy(spec.OrderBy);
            }

            if (spec.OrderByDescending != null)
            {
                query = query.OrderByDescending(spec.OrderByDescending);
            }

            if (spec.IsPagingEnabled) // pagination come in last
            {
                query=query.Skip(spec.Skip).Take(spec.Take);
            }
            
            //applying includes
            query=spec.Includes.Aggregate(query,(current,include)=>current.Include(include));
            return query;
        }
    }
}