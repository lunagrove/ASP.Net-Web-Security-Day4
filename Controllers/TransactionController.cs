using Day3Paypal.Data;
using Day3Paypal.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Day3Paypal.Controllers
{
    public class TransactionController : Controller
    {
        private readonly ApplicationDbContext _db;

        public TransactionController(ApplicationDbContext context)
        {
            _db = context;
        }

        [Authorize]
        public IActionResult Index()
        {

            DbSet<IPN> items = _db.IPNs;

            return View(items);
        }
    }
}
