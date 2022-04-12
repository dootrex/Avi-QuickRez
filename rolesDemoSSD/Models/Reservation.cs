using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace rolesDemoSSD.Models
{
    public class Reservation
    {
        [Key]
        public int ReservationID { get; set; }
        /* public int ReservationStart { get; set; }
         public int ReservationEnd { get; set; }*/
        public DateTime ReservationStart { get; set; }
        public DateTime ReservationEnd { get; set; }
        public int RestaurantTableID { get; set; }
        public int CustomerID { get; set; }
        // public bool dumbField { get; set; }


        public virtual RestaurantTable RestaurantTable { get; set; }
        public virtual Customer Customer { get; set; }
    }
}