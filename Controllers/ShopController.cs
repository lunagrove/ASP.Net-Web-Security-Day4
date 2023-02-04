using Day3Paypal.Data;
using Day3Paypal.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Day3Paypal.ViewModels;

namespace Day3Paypal.Controllers
{
    public class ShopController : Controller
    {
        private readonly ApplicationDbContext _db;

        public ShopController(ApplicationDbContext context)
        {
            _db = context;
        }
        public IActionResult Index()
        {
            ProductRepo productRepo = new ProductRepo(_db);
            var products = productRepo.GetAllProducts();
            return View(products);
        }
        
    }
}
