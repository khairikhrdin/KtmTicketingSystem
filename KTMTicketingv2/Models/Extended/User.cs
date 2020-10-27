using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace KTMTicketingv2.Models
{
    [MetadataType(typeof(UserMetaData))]
    public partial class User
    {
        public string ConfirmPassword { get; set; }
    }

    public class UserMetaData
    {
        [Display(Name="First Name")]
        [Required(AllowEmptyStrings=false, ErrorMessage="First name required")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Last name required")]
        public string LastName { get; set; }

        [Display(Name = "Email ID")]
        [Required(AllowEmptyStrings=false, ErrorMessage="Email ID required")]
        [DataType(DataType.EmailAddress)]
        public string EmailID { get; set; }

        [Required(AllowEmptyStrings=false, ErrorMessage="Password required")]
        [DataType(DataType.Password)]
        [MinLength(6, ErrorMessage="Minimum 6 characters")]
        public string Password { get; set; }

        [Display(Name="Confirm Password")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage="Password did not match")]
        public string ConfirmPassword { get; set; }

        [Display(Name="IC Number")]
        [RegularExpression("^[0-9]{12}$", ErrorMessage="Must be 12 digits e.g: 971118016377")]
        [Required(AllowEmptyStrings=false, ErrorMessage="IC Number required")]
        public string ICNumber { get; set; }
    }
}