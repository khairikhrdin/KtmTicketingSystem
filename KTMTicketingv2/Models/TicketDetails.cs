using Foolproof;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace KTMTicketingv2.Models
{
    public class TicketDetails
    {
        public Destination Destination { get; set; }
        public Passenger Passenger { get; set; }
    }

    public class Destination
    {
        [Display(Name = "Origin")]
        [Required(ErrorMessage = "Please specify your origin")]
        public String OriginLoc { get; set; }

        [Display(Name = "Destination")]
        [NotEqualTo("OriginLoc")]
        [Required(ErrorMessage = "Please specify your destination")]
        public String DestiLoc { get; set; }

        [Display(Name = "Trip Type")]
        [Required(ErrorMessage = "Please select your way")]
        public String WayLoc { get; set; }

        [Display(Name = "Citizen Type")]
        [Required(ErrorMessage = "Please select your type of citizen")]
        public String Citizen { get; set; }

        [Display(Name = "Quantity")]
        [Required(ErrorMessage = "Please insert quantity of ticket")]
        [Range(1, 50)]
        public int Quantity { get; set; }
    }

    public class Passenger
    {
        public String EmailID { get; set; }
        public String FirstName { get; set; }
        public String LastName { get; set; }
        public String ICNumber { get; set; }
        public String OriginLoc { get; set; }
        public String DestiLoc { get; set; }
        public String WayLoc { get; set; }
        public String Citizen { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }
        public String OriginLocID { get; set; }
        public String DestiLocID { get; set; }
        public String WayLocID { get; set; }
        public String CitizenID { get; set; }
        public int TicketID { get; set; }
        public DateTime PurchasedOn { get; set; }

    }
}