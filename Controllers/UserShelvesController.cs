using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BookTracker.Data;
using BookTracker.Models;
using System.Text.Json;
using Newtonsoft.Json;

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
            TempData["category"] = Convert.ToInt32(searchEnum);
         
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
        //// GET: UserShelves/Create
        //public IActionResult Create(int shelf)
        //{
        //    return View();
        //}

        // POST: UserShelves/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.

        //public async Task<IActionResult> Create([Bind("Serialno,Category,BookId,UserDetailsEnrollId")] UserShelf userShelf)
        [HttpPost]
        public async Task<IActionResult> Create(string shelf)

        {


            string[] Search = shelf.Trim().Split(' ');
            int bookid = Convert.ToInt32(Search[0]);
            Category? category = (Category?)Convert.ToInt32(Search[1]);


            UserShelf userShelf = new UserShelf();
            userShelf.Category = category;
            int enrollid = (int)TempData["EnrollId"];
            //System.Diagnostics.Debug.WriteLine(bookid + category);

                userShelf.Book = _context.Book.Where(o => o.Id ==
                 bookid).FirstOrDefault();
                userShelf.UserDetails = _context.UserDetails.Where(o => o.EnrollId ==
                enrollid).FirstOrDefault();

            //here checking where book exists with the same entry
            var entries = from m in _context.UserShelf.Include(m => m.Book).Include(m => m.UserDetails)
                        select m;
          
             var   entry = entries.Where(s => s.UserDetails.EnrollId.Equals(userShelf.UserDetails.EnrollId) && s.Book.Id.Equals(userShelf.Book.Id));
            
            System.Diagnostics.Debug.WriteLine(entry.FirstOrDefault());
            int serialno = entry.Select(o => o.Serialno).FirstOrDefault();

            if (serialno !=0)
            {
                TempData["AddedToShelf"] = "Already Added";
                TempData["Bookid"] = bookid;
                TempData.Keep("EnrollId");

                return Json("Already Added");

            }
           

                _context.Add(userShelf);
                await _context.SaveChangesAsync();
                TempData.Keep("EnrollId");
                TempData["AddedToShelf"] = "Added Successfully";
                TempData["Bookid"] = bookid;

                //   return RedirectToPage("IndexUser", "Books");
                return Json("Added Successfully");

          

           
        }

       // GET: UserShelves/Edit/5
        //public async Task<IActionResult> Edit()
        //{
        //    //if (id == null)
        //    //{
        //    //    return NotFound();
        //    //}

        //    //var userShelf = await _context.UserShelf.FindAsync(id);
        //    //if (userShelf == null)
        //    //{
        //    //    return NotFound();
        //    //}
        //    return View();
        //}

        // POST: UserShelves/Edit/
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //  [ValidateAntiForgeryToken]

        [HttpPost]
        public async Task<IActionResult> Edit(string shelf)
        {

            string[] Search = shelf.Trim().Split(' ');
            int bookid = Convert.ToInt32(Search[0]);
            Category? category = (Category?)Convert.ToInt32(Search[1]);


            UserShelf userShelf = new UserShelf();


            int enrollid = (int)TempData["EnrollId"];

            //userShelf.Book = _context.Book.Where(o => o.Id ==
            //     bookid).FirstOrDefault();
            //userShelf.UserDetails = _context.UserDetails.Where(o => o.EnrollId ==
            //enrollid).FirstOrDefault();

            var entries = from m in _context.UserShelf.Include(m => m.Book).Include(m => m.UserDetails)
                          select m;
            var entry = entries.Where(s => s.UserDetails.EnrollId.Equals(enrollid) && s.Book.Id.Equals(bookid)).Select(o => o.Serialno);

            userShelf.Serialno = entry.FirstOrDefault();
            userShelf.Category = category;


            //var entry = entries.Where(s => s.UserDetails.EnrollId.Equals(userShelf.UserDetails.EnrollId) && s.Book.Id.Equals(userShelf.Book.Id)).Select(o=>o.Serialno);

            _context.Update(userShelf);
                    await _context.SaveChangesAsync();
                    TempData.Keep("EnrollId");
            TempData["AddedToShelf"] = "Added to" + category;
            string message = "Added to" + category;

            return Json(message);
            
        }


        /// <summary>
        /// Remove from Shelf
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // GET: UserShelves/Delete/


      //  [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            if (id == 0)
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
            //  return View(users);
            var shelfBook = await _context.UserShelf.FindAsync(id);
            _context.UserShelf.Remove(userShelf);
            await _context.SaveChangesAsync();
            TempData["AddedToShelf"] = "Removed";
            return Json("Removed");
        }

        //public async Task<IActionResult> Delete(UserShelf users)
        //{
        //    return View();
        //}

        // POST: UserShelves/Delete/5
     //   [HttpPost, ActionName("Delete")]
      //  [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var userShelf = await _context.UserShelf.FindAsync(id);
            _context.UserShelf.Remove(userShelf);
            await _context.SaveChangesAsync();
            return RedirectToPage("Index", "UserShelves");
        }

        private bool UserShelfExists(int id)
        {
            return _context.UserShelf.Any(e => e.Serialno == id);
        }
    }
}
