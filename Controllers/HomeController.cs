using Day3Paypal.Data;
using Day3Paypal.Models;
using Humanizer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Day3Paypal.Controllers;

    public class HomeController : Controller
    {

        private readonly ApplicationDbContext _db;

    public HomeController(ApplicationDbContext context)
    {
        _db = context;
    }   

    public IActionResult Index()
    {
        return View();
    }

    // This method receives and stores
    // the Paypal transaction details.
    [HttpPost]
    public JsonResult PaySuccess([FromBody] IPN ipn)
    {
        try
        {
            _db.IPNs.Add(ipn);
            _db.SaveChanges();
        }
        catch (Exception ex)
        {
            return Json(ex.Message);
        }
        return Json(ipn);
    }

    // Home page shows list of items.
    // Item price is set through the ViewBag.
    public IActionResult Confirmation(string confirmationId)
    {
        IPN transaction = _db.IPNs.FirstOrDefault(t => t.paymentID == confirmationId);

        return View("Confirmation", transaction);
    }
}