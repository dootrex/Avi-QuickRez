using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace rolesDemoSSD.ViewModels
{
    public class ReservationVM
    {
        public int ReservationID { get; set; }
        public string CustomerName { get; set; }
        public string PhoneNum { get; set; }
        public string Memo { get; set; }
        public int TableNum { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }
}
