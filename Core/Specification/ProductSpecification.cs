using Core.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Specification
{
    public class ProductSpecification : BaseSpecification<Product>
    {
        public ProductSpecification(ProductSpecParams specParams): base( x =>
        (string.IsNullOrEmpty(specParams.Search) || x.Name.Contains(specParams.Search, StringComparison.CurrentCultureIgnoreCase)) &&
        (specParams.Name.Count == 0 || specParams.Name.Contains(x.Name)) &&

        (specParams.Type.Count == 0 || specParams.Type.Contains (x.Type))
        )
        {
            switch (specParams.Sort)
            {
                case "priceAsc":
                    AddOrderBy(x => x.Price);
                    break;
                case "priceDesc":
                    AddOrderByDescending(x => x.Price);
                    break;
                default:
                    AddOrderBy(x => x.Name); break;
            }
        }
    }
}
