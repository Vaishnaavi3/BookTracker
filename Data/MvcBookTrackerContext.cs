using BookTracker.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookTracker.Data
{
    public class MvcBookTrackerContext :DbContext
    {
       public MvcBookTrackerContext(DbContextOptions<MvcBookTrackerContext> options)
            : base(options)
        {

        }
        public DbSet<Book> Book { get; set; }
        public DbSet<UserDetails> UserDetails { get; set; }

        public DbSet<UserShelf> BookShelf { get; set; }


    }
}
