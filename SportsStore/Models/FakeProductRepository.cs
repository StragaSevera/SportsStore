using System.Collections.Generic;
using System.Linq;

namespace SportsStore.Models
{
    public class FakeProductRepository : IProductRepository
    {
        public IQueryable<Product> Products => new List<Product>
        {
            new Product {Name = "Football", Price = 25m},
            new Product {Name = "Surf board", Price = 179m},
            new Product {Name = "Running shoes", Price = 95m}
        }.AsQueryable();
    }
}