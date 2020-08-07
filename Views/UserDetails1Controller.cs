using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BookTracker.Data;
using BookTracker.Models;
using Newtonsoft.Json;

namespace BookTracker.Views
{
    public class UserDetails1Controller : Controller
    {
        private readonly BookTrackerContext _context;

        public UserDetails1Controller(BookTrackerContext context)
        {
            _context = context;
        }

        //POST: UserDetails/Index1
        [HttpPost]
        public IActionResult Index1(string number)
        {
            // JsonConvert.DeserializeObject<MyObjectTye(number)>
            int value = number.Length;

            System.Diagnostics.Debug.WriteLine(number+"from contoller");
            return RedirectToAction("Index", "Home");
        }

        // GET: UserDetails1
        public async Task<IActionResult> Create()
        {
            return View(await _context.UserDetails.ToListAsync());
        }

        // GET: UserDetails1/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userDetails = await _context.UserDetails
                .FirstOrDefaultAsync(m => m.EnrollId == id);
            if (userDetails == null)
            {
                return NotFound();
            }

            return View(userDetails);
        }

        // GET: UserDetails1/Create
        public IActionResult Index()
        {
            return View();
        }

        // POST: UserDetails1/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index([Bind("EnrollId,Name,DateOfBirth,EmailId,Password")] UserDetails userDetails)
        {
            if (ModelState.IsValid)
            {
                _context.Add(userDetails);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(userDetails);
        }

        // GET: UserDetails1/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userDetails = await _context.UserDetails.FindAsync(id);
            if (userDetails == null)
            {
                return NotFound();
            }
            return View(userDetails);
        }

        // POST: UserDetails1/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("EnrollId,Name,DateOfBirth,EmailId,Password")] UserDetails userDetails)
        {
            if (id != userDetails.EnrollId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(userDetails);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserDetailsExists(userDetails.EnrollId))
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
            return View(userDetails);
        }

        // GET: UserDetails1/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userDetails = await _context.UserDetails
                .FirstOrDefaultAsync(m => m.EnrollId == id);
            if (userDetails == null)
            {
                return NotFound();
            }

            return View(userDetails);
        }

        // POST: UserDetails1/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var userDetails = await _context.UserDetails.FindAsync(id);
            _context.UserDetails.Remove(userDetails);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserDetailsExists(int id)
        {
            return _context.UserDetails.Any(e => e.EnrollId == id);
        }
    }
}
