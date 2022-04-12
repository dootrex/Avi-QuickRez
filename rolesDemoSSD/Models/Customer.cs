using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace rolesDemoSSD.Models
{
    public class Customer
    {
        [Key]
        public int CustomerID { get; set; }
        public string PhoneNum { get; set; }
        public string bookingName { get; set; }
        public string memo { get; set; }
        public virtual ICollection<Reservation> Reservations { get; set; }
    }
}
