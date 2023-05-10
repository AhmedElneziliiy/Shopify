using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Core.Entities;

namespace Core.Specifications
{
    public class ProductsWithTypesAndBrandsSpecification:BaseSpecification<Product>
    {
        public ProductsWithTypesAndBrandsSpecification(string sort) //used for getting all products
        {
            AddInclude(x=>x.ProductType);
            AddInclude(x=>x.ProductBrand);
            AddOrderBy(x=>x.Name);
            
            if(!string.IsNullOrEmpty(sort))
            {
                switch (sort)
                {
                    case "priceAsc":
                        AddOrderBy(p=>p.Price);
                        break;
                    case "priceDesc":
                        AddOrderByDescending(p=>p.Price);
                        break;
                    default:
                        AddOrderBy(c=>c.Name);
                        break;    
                }
            }
        }

        public ProductsWithTypesAndBrandsSpecification(int id) //used for getting single product for that we are using criteria
         : base(x=>x.Id==id)
        {
            AddInclude(x=>x.ProductType);
            AddInclude(x=>x.ProductBrand);
        }
    }
}