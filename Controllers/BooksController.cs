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
    public class BooksController : Controller
    {

        private readonly BookTrackerContext _context;

      
        public BooksController(BookTrackerContext context)
        {
            _context = context;
        }

        // GET: Books

        /// <summary>
        /// filter Title
        /// </summary>
        /// <param name="searchString"></param>
        /// <returns>List of Books</returns>
        public async Task<IActionResult> Index(string searchString)
        {
            System.Diagnostics.Debug.WriteLine("coming to BooksController");
            var book = from m in _context.Book
                       select m;
            if (!String.IsNullOrEmpty(searchString))
            {
                book = book.Where(s => s.Title.Contains(searchString));
            }
          
           
            return View(await book.ToListAsync());
        }
        /// <summary>
        /// home page User
        /// </summary>
        /// <param name="searchString"></param>
        /// <returns></returns>
        public async Task<IActionResult> IndexUser(string searchString)
        {
            System.Diagnostics.Debug.WriteLine("coming to BooksController");
            var book = from m in _context.Book
                       select m;
            if (!String.IsNullOrEmpty(searchString))
            {
                book = book.Where(s => s.Title.Contains(searchString));
            }

            TempData.Keep("EnrollId");
            TempData.Keep("UserName");
            return View(await book.ToListAsync());
        }

      

        /// <summary>
        /// get all data
        /// </summary>
        /// <param name="id"></param>
        /// <returns>list of Books</returns>
        //public async Task<IActionResult> Index()
        //{
        //    return View(await _context.Book.ToListAsync());
        //}

        // GET: Books/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Book
                .FirstOrDefaultAsync(m => m.Id == id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        // GET: Books/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Books/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,SubTitle,Author,PublishedOn,Pages,Genre,Language,Rating")] Book book)
        {
            if (ModelState.IsValid)
            {
                _context.Add(book);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(book);
        }

        // GET: Books/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Book.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }
            return View(book);
        }

        // POST: Books/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,SubTitle,Author,PublishedOn,Pages,Genre,Language,Rating")] Book book)
        {
            if (id != book.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(book);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookExists(book.Id))
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
            return View(book);
        }

        // GET: Books/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Book
                .FirstOrDefaultAsync(m => m.Id == id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        // POST: Books/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var book = await _context.Book.FindAsync(id);
            _context.Book.Remove(book);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BookExists(int id)
        {
            return _context.Book.Any(e => e.Id == id);
        }
    }
}
