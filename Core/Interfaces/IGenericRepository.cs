using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;
using Core.Specifications;

namespace Core.Interfaces
{
    public interface IGenericRepository<T> where T :BaseEntity //used generic with classes that derive from that class
    {
        Task<T>GetByIdAsync(int id);
        Task<IReadOnlyList<T>>ListAllAync(); 

        //-------
        Task<T> GetEntityWithSpec(ISpecification<T> spec); //pass specs to excute
        Task<IReadOnlyList<T>> ListAsync(ISpecification<T> spec);
    }
}