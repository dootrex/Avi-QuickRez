using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace rolesDemoSSD.Models
{
    public class Restaurant
    {
        [Key]
        public int RestaurantId { get; set; }
        public string BusinessNum { get; set; } //These values can't be nullable since the business number, name, open/close hours are required 
        public string RestaurantName { get; set; }
        public string BusinessUrl { get; set; }  //Business URL can be nullable because not every business has a website that is available
        public int OpeningHour { get; set; }
        public int ClosingHour { get; set; }
        //added columns
        public string StreetAddress { get; set; }
        public string CityName { get; set; }
        public string Province { get; set; }
        public string Email { get; set; }
        /*        public int ClosingDay { get; set; } //0 is Sunday and 6 is Saturday
                public DateTime SpecificClosingDate { get; set; }*/

        // end added columns

        public virtual ICollection<RestaurantTable> RestaurantTables { get; set; }
    }
}
