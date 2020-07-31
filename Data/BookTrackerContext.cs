using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BookTracker.Models;

namespace BookTracker.Data
{
    public class BookTrackerContext : DbContext
    {
        public BookTrackerContext (DbContextOptions<BookTrackerContext> options)
            : base(options)
        {
        }

        public DbSet<BookTracker.Models.Book> Book { get; set; }
        public DbSet<BookTracker.Models.UserDetails> UserDetails { get; set; }

        public DbSet<BookTracker.Models.UserShelf> UserShelf { get; set; }
    }
}
