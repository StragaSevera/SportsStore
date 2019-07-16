using System.Linq;
using Microsoft.AspNetCore.Mvc;
using SportsStore.Models;
using SportsStore.Models.ViewModels;

namespace SportsStore.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductRepository _repository;
        public int PageSize = 4;

        public ProductController(IProductRepository repository)
        {
            _repository = repository;
        }

        // GET
        public IActionResult List(string category, int page = 1)
        {
            var products = _repository.Products.OrderBy(p => p.ProductID)
                .Where(p => category == null || p.Category == category);
            var pagingInfo = new PagingInfo
            {
                CurrentPage = page,
                ItemsPerPage = PageSize,
                TotalItems = products.Count()
            };
            var pagedProducts = products
                .Skip((page - 1) * PageSize)
                .Take(PageSize);
            return View(new ProductsListViewModel
            {
                Products = pagedProducts,
                PagingInfo = pagingInfo,
                CurrentCategory = category
            });
        }
    }
}