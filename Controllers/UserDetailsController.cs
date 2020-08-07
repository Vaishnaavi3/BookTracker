using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BookTracker.Data;
using BookTracker.Models;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.VisualStudio.Web.CodeGeneration.Contracts.Messaging;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Session;
namespace BookTracker.Controllers
{
    public class UserDetailsController : Controller
    {
        private readonly BookTrackerContext _context;

        
        /// <summary>
        /// LogIn
        /// </summary>
        /// <returns></returns>
        // GET: UserDetails/LogIn
        public IActionResult LogIn()
        {
           // ViewData["bool"] = false;

            return View();
        }

        //POST: UserDetails/LogIn
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LogIn([Bind("EmailId,Password")] UserDetails userDetails)
        {
          

            var user = await _context.UserDetails
           .FirstOrDefaultAsync(m => m.EmailId.Equals(userDetails.EmailId));

            if (user != null)
            {
                if (user.Password == userDetails.Password)
                {
                    TempData["UserName"] = user.Name;
                    TempData["EnrollId"] = user.EnrollId;
                
                    
                    return RedirectToAction("IndexUser", "Books");
                }
            }
              
            TempData["SignUp"] = " Invalid LogIn Credentials!";
            return RedirectToAction("Index", "Books");
        }



        /// <summary>
        /// SignUp
        /// </summary>
        /// <returns></returns>
         // GET: UserDetails/SignUp
        public IActionResult SignUp()
        {
           

            return View();
        }

        // POST: UserDetails/SignUp
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SignUp([Bind("EnrollId,Name,DateOfBirth,EmailId,Password")] UserDetails userDetails)
        {
           // ViewData["bool"] = false;

            if (ModelState.IsValid)
            {
                //     var user = await _context.UserDetails
                //.FirstOrDefaultAsync(m => m.EmailId.Equals(userDetails.EmailId));
                //     if (user ==null)
                //     {
                _context.Add(userDetails);
                await _context.SaveChangesAsync();
                TempData["SignUp"] = "Successfully Registered!";
                //     }
                //     else
                //     {
                //         TempData["emailExist"]="Email"
                //     }

            }
            else if (ModelState == null)
            {
                TempData["SignUp"] = "Registration unsuccessful";
                return RedirectToAction("SignUp", "UserDetails");
            }

            return RedirectToAction("Index", "Books");
        }


        public UserDetailsController(BookTrackerContext context)
        {
            _context = context;
        }

        // GET: UserDetails/Create
        public async Task<IActionResult> Index()
        {
            return View(await _context.UserDetails.ToListAsync());
        }

        // GET: UserDetails/Details/5
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
      
        // GET: UserDetails/Edit/5
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

        // POST: UserDetails/Edit/5
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

        // GET: UserDetails/Delete/5
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

        // POST: UserDetails/Delete/5
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
