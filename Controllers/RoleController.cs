using Day3Paypal.Data;
using Day3Paypal.Repositories;
using Day3Paypal.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace Day3Paypal.Controllers
{
    [Authorize(Roles = "Admin")]
    public class RoleController : Controller
    {
        ApplicationDbContext _db;

        public RoleController(ApplicationDbContext db)
        {
            _db = db;
        }

        // GET: Role
        public ActionResult Index()
        {
            RoleRepo roleRepo = new RoleRepo(_db);
            return View(roleRepo.GetAllRoles());
        }

        public ActionResult Create()
        {
            return View(new RoleVM());
        }

        [ValidateAntiForgeryToken]
        [HttpPost] // POST: Index/Create
        public IActionResult Create(RoleVM role)
        {
            var token = HttpContext.Request.Form["__RequestVerificationToken"];

            string addMessage = "";
            bool success = false;

            if (ModelState.IsValid)
            {
                RoleRepo rr = new RoleRepo(_db);
                success = rr.CreateRole(role.RoleName);
            }
            else
            {
                success = false;
            }
            if (success)
            {
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return View(role);
            }
        }
    }
}
