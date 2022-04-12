using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace rolesDemoSSD.Models
{
    public class RestaurantTable
    {
        [Key]
        public int RestaurantTableID { get; set; }
        public int TableCapacity { get; set; }
        public int RestaurantID { get; set; }
        public virtual Restaurant Restaurant { get; set; }
        public virtual ICollection<Reservation> Reservations { get; set; }
    }
}
