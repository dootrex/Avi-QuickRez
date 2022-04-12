using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using rolesDemoSSD.Data;
using rolesDemoSSD.Models;
using rolesDemoSSD.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace rolesDemoSSD.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class ManageReservationController : ControllerBase
    {
        private ApplicationDbContext _context;
        public ManageReservationController(ApplicationDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public IActionResult GetHello()
        {
            string wave = "Hello";
            return new ObjectResult(wave);
        }

        [HttpGet("{resId}")]

        public IActionResult GetCapacities(long resID)
        {
            HashSet<int> capacities = new HashSet<int>();
            var tables = _context.restaurantTables.Where(rt => rt.RestaurantID == resID);
            if (tables != null)
            {
                foreach (var table in tables)
                {
                    capacities.Add(table.TableCapacity);
                }
            }
            var restaurant = _context.restaurants.Where(r => r.RestaurantId == resID).FirstOrDefault();
            if (restaurant != null)
            {
                RestaurantWithCapacityVM rwcVM = new RestaurantWithCapacityVM()
                {
                    Capacities = capacities,
                    RestaurantId = restaurant.RestaurantId,
                    RestaurantName = restaurant.RestaurantName,
                    OpeningHour = restaurant.OpeningHour,
                    ClosingHour = restaurant.ClosingHour
                };
                return new ObjectResult(rwcVM);
            }
            return BadRequest();
        }

        [HttpGet("{resID}/{cap}/{time}")]
        public IActionResult GetTables(long resID, int cap, string time)
        {

            DateTime requestedTime = DateTime.Parse(time);
            bool found = false;
            int foundTableID = -1;
            var tables = _context.restaurantTables.Where(rt => rt.RestaurantID == resID && rt.TableCapacity == cap);
            if (tables != null)
            {
                foreach (var table in tables)
                {
                    if (!found)
                    {
                        var resos = _context.reservations.Where(r => r.RestaurantTableID == table.RestaurantTableID).ToList();
                        if (resos != null)
                        {
                            bool foundReso = false;
                            foreach (var reso in resos)
                            {
                                if (requestedTime >= reso.ReservationStart && requestedTime <= reso.ReservationEnd)
                                {
                                    foundReso = true;
                                }
                            }
                            if (!foundReso)
                            {
                                found = true;
                                foundTableID = table.RestaurantTableID;
                            }
                        }
                    }
                }
            }
            if (!found && foundTableID == -1)
            {
                return new ObjectResult(-1);
            }
            else
            {
                return new ObjectResult(foundTableID);
            }
        }

        //only allow this if the above end points return an available reservation
        [HttpPost]
        /*  [Route("make")]*/
        public IActionResult MakeReservation([FromBody] CustomerReservationVM crVM)
        {
            Customer tempCustomer = new Customer();
            Customer customer = _context.customers.Where(c => c.bookingName == crVM.bookingName && c.PhoneNum == crVM.PhoneNum).FirstOrDefault();

            if (customer == null)
            {

                tempCustomer.bookingName = crVM.bookingName;
                tempCustomer.PhoneNum = crVM.PhoneNum;
                tempCustomer.memo = crVM.memo;

                _context.customers.Add(tempCustomer);
                _context.SaveChanges();
            }
            else
            {
                tempCustomer = customer;
            }
            Reservation reservation = new Reservation()
            {

                ReservationStart = DateTime.Parse(crVM.ReservationStart),
                ReservationEnd = DateTime.Parse(crVM.ReservationStart).AddHours(1),
                RestaurantTableID = crVM.RestaurantTableID,
                CustomerID = tempCustomer.CustomerID
            };
            _context.reservations.Add(reservation);
            _context.SaveChanges();
            //made a json object because causing object cycle issue if returning a c# object
            var jsonReservation = new
            {
                ReservationID = reservation.ReservationID,
                ReservationStart = reservation.ReservationStart,
                ReservationEnd = reservation.ReservationEnd,
                RestaurantTableID = reservation.RestaurantTableID,
                CustomerID = reservation.CustomerID
            };
            return new ObjectResult(jsonReservation);
        }

        [HttpPost]
        [Route("saveTable")]
        public IActionResult GenerateTables([FromBody] TableList tableData)
        {
            foreach (var row in tableData.lists)
            {
                for (int i = 0; i < row.numberOfTables; i++)
                {
                    RestaurantTable rt = new RestaurantTable()
                    {
                        RestaurantID = tableData.resID,
                        TableCapacity = row.cap
                    };
                    _context.restaurantTables.Add(rt);
                    _context.SaveChanges();
                }
            }

            return new ObjectResult(1);
        }

        [HttpGet("search/{searchTerm}")]

        public IActionResult SearchRestaurant(string searchTerm)
        {
            searchTerm = searchTerm.ToUpper();
            var restaurants = _context.restaurants;
            List<Restaurant> result = new List<Restaurant>();
            foreach (var item in restaurants)
            {
                if (item.RestaurantName.ToUpper().Contains(searchTerm))
                {
                    result.Add(item);
                }
            }
            return new ObjectResult(result);
        }
    }
}
