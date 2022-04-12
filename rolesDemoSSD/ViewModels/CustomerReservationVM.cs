using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace rolesDemoSSD.ViewModels
{
    public class CustomerReservationVM
    {
        public string PhoneNum { get; set; }
        public string bookingName { get; set; }
        public int cap { get; set; }

        public string? memo { get; set; }
        public int RestaurantTableID { get; set; }
        public string ReservationStart { get; set; }
    }
}
