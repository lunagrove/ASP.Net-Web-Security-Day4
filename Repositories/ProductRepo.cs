using Day3Paypal.Data;
using Day3Paypal.Models;
using Microsoft.Data.SqlClient;

namespace Day3Paypal.Repositories
{
    public class ProductRepo
    {
        ApplicationDbContext _db;

        public ProductRepo(ApplicationDbContext db)
        {
            _db = db;
        }

        public IEnumerable<Product> GetAllProducts()
        {
            var products = _db.Products;

            return (products);
        }
    }
}
