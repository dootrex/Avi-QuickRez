using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using rolesDemoSSD.Data;
using Microsoft.AspNetCore.Http;

using rolesDemoSSD.Models;
using rolesDemoSSD.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace rolesDemoSSD.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        private ApplicationDbContext _context;
        public DashboardController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            string userName = User.Identity.Name;
            Restaurant restaurant = _context.restaurants.Where(res => string.Equals(userName, res.Email)).FirstOrDefault();
            var restaurantTables = _context.restaurantTables.Where(rt => rt.RestaurantID == restaurant.RestaurantId);
            ViewBag.tableCount = restaurantTables.Count();
            ViewBag.OpeningHour = restaurant.OpeningHour;
            ViewBag.ClosingHour = restaurant.ClosingHour;
            ViewBag.RestaurantName = restaurant.RestaurantName;
            ViewBag.RestaurantId = restaurant.RestaurantId;

            List<Reservation> allRes = _context.reservations.Where(res => res.RestaurantTable.RestaurantID == restaurant.RestaurantId).ToList();

            var allResVM = from r in allRes
                           from c in _context.customers
                           where r.CustomerID == c.CustomerID
                           orderby r.ReservationStart
                           select new ReservationVM()
                           {
                               ReservationID = r.ReservationID,
                               CustomerName = c.bookingName,
                               PhoneNum = c.PhoneNum,
                               Memo = c.memo,
                               TableNum = r.RestaurantTableID,
                               StartTime = r.ReservationStart,
                               EndTime = r.ReservationEnd
                           };
            int count = allResVM.Count();

            return View(allResVM);
        }

    }
}
