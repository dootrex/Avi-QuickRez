using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace rolesDemoSSD.ViewModels
{
    public class RestaurantWithCapacityVM
    {
        [Key]
        public int RestaurantId { get; set; }
        public string RestaurantName { get; set; }
        public int OpeningHour { get; set; }
        public int ClosingHour { get; set; }
        public HashSet<int> Capacities { get; set; }

    }
}
