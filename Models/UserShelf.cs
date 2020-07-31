using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BookTracker.Models
{
    public enum Category
    {
        Reading=1, Read=2, KeepOnShelf=3
    }
    public class UserShelf
    {
        [Key]
        public int Serialno { get; set; }

        public Category? Category { get; set; }

        public Book Book { get; set; }
        public UserDetails UserDetails { get; set; }
    }

   


}
