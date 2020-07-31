using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BookTracker.Models
{
    public class UserDetails
    {
        [Key]
        public int EnrollId { get; set; }

        [Required(ErrorMessage ="Enter Name")]
        public string Name { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime? DateOfBirth { get; set; }


        [Required]
        [EmailAddress]
        public string EmailId { get; set; }

        [Required]
        [PasswordPropertyText]
        public string Password { get; set; }

        public ICollection<UserShelf> UserShelves { get; set; }
    }



}
