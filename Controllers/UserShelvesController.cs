using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BookTracker.Data;
using BookTracker.Models;

namespace BookTracker.Controllers
{
    public class UserShelvesController : Controller
    {
        private readonly BookTrackerContext _context;

      
        /// <summary>
        /// filter category(string) and user
        /// </summary>
        /// <param name="searchEnum"></param>
        /// <returns></returns>
        public async Task<IActionResult> Index(Category? searchEnum)
        {
           
            var shelf = from m in _context.UserShelf.Include(m => m.Book).Include(m => m.UserDetails)
                        select m;

            
            int enrollId = (int)TempData["EnrollId"];
                if (searchEnum != null)
                {
                    shelf = shelf.Where(s => s.Category.Equals(searchEnum) && s.UserDetails.EnrollId.Equals(enrollId));
                }
                else
                {
                    shelf = shelf.Where(s => s.UserDetails.EnrollId.Equals(enrollId));

                }
                TempData.Keep("EnrollId");

         
            return View(await shelf.ToListAsync());
        }
       
       
        public UserShelvesController(BookTrackerContext context)
        {
            _context = context;
        }

        // GET: UserShelves
        //public async Task<IActionResult> Index()
        //{
        //    return View(await _context.UserShelf.ToListAsync());
        //}

        // GET: UserShelves/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userShelf = await _context.UserShelf
                .FirstOrDefaultAsync(m => m.Serialno == id);
            if (userShelf == null)
            {
                return NotFound();
            }

            return View(userShelf);
        }
        /// <summary>
        /// Add to Shelf
        /// </summary>
        /// <returns></returns>
        // GET: UserShelves/Create
        public IActionResult Create()
        {
            return View();
            //return RedirectToAction("Index", "Books");
        }

        // POST: UserShelves/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("Serialno,Category,BookId,UserDetailsEnrollId")] UserShelf userShelf)
        public async Task<IActionResult> Create(string shelf)

        {
            

            string[] Search = shelf.Trim().Split(' ');
            int bookid = Convert.ToInt32(Search[0]);
            Category? category = (Category?)Convert.ToInt32(Search[1]);

           
            UserShelf userShelf = new UserShelf();
            userShelf.Category = category;
            int enrollid = (int)TempData["EnrollId"];
             System.Diagnostics.Debug.WriteLine(bookid+category);

            if (bookid != 0)
            {
              
                userShelf.Book = _context.Book.Where(o => o.Id ==
                 bookid).FirstOrDefault();
                userShelf.UserDetails = _context.UserDetails.Where(o => o.EnrollId ==
                enrollid).FirstOrDefault();

            }
            else
            {
                return RedirectToAction("Index", "Books");
            }

            _context.Add(userShelf);
                await _context.SaveChangesAsync();
            TempData.Keep("EnrollId");
            TempData["AddedToShelf"] = "Added Successfully";
                return RedirectToAction("IndexUser","Books");

           
        }

        // GET: UserShelves/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userShelf = await _context.UserShelf.FindAsync(id);
            if (userShelf == null)
            {
                return NotFound();
            }
            return View(userShelf);
        }

        // POST: UserShelves/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Serialno,Category")] UserShelf userShelf)
        {
            if (id != userShelf.Serialno)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(userShelf);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserShelfExists(userShelf.Serialno))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(userShelf);
        }
        /// <summary>
        /// Remove from Shelf
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // GET: UserShelves/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
           

            var userShelf = await _context.UserShelf
                .FirstOrDefaultAsync(m => m.Serialno == id);
            var user = from m in _context.UserShelf.Include(m => m.Book)
                       select m;
         
            var users = user.Where(m => m.Serialno == id).Select(o => o.Book.Title);
            System.Diagnostics.Debug.WriteLine(users.First());
            string userName = users.FirstOrDefault();
          
            if (userShelf == null)
            {
                return NotFound();
            }
            System.Diagnostics.Debug.WriteLine(users);
            TempData["BookName"] = userName;
            return View();
        }

        // POST: UserShelves/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var userShelf = await _context.UserShelf.FindAsync(id);
            _context.UserShelf.Remove(userShelf);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "UserShelves");
        }

        private bool UserShelfExists(int id)
        {
            return _context.UserShelf.Any(e => e.Serialno == id);
        }
    }
}
