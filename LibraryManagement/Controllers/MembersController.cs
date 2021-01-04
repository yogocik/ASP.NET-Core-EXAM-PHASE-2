using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LibraryManagement.Data;
using LibraryManagement.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace LibraryManagement.Controllers
{
    [Authorize(Roles="Member")]
    public class MembersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MembersController(ApplicationDbContext context)
        {
            _context = context;
        }

        //GET Member
        [Authorize(Roles ="Admin,Staff")]
        public async Task<IActionResult> Index(string sortOrder, string searchString,
                                                string currentFilter, int? pageNumber)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["DateSortParm"] = sortOrder == "Date" ? "date_desc" : "Date";

            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewData["CurrentFilter"] = searchString;
            var members = from s in _context.Members select s;
            if (!String.IsNullOrEmpty(searchString))
            {
                members = members.Where(s => s.Name.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "name_desc":
                    members = members.OrderByDescending(s => s.Name);
                    break;
                case "Date":
                    members = members.OrderBy(s => s.BirthDate);
                    break;
                case "date_desc":
                    members = members.OrderByDescending(s => s.BirthDate);
                    break;
                default:
                    members = members.OrderBy(s => s.Name);
                    break;
            }
            int pageSize = 3;
            return View(await PaginatedList<Member>.CreateAsync(members.AsNoTracking(), pageNumber ?? 1, pageSize));
            // return View(await members.AsNoTracking().ToListAsync());
            // return View(await _context.Members.ToListAsync());
        }

        // GET: Members/Details/5
        [Authorize(Roles ="Admin,Staff")]
        public async Task<IActionResult>Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var member = await _context.Members.Include(s => s.Rentals)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (member == null)
            {
                return NotFound();
            }

            return View(member);
        }
        

        // GET: Members/Edit/5 
        [Authorize(Roles = "Admin,Staff")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var member = await _context.Members.FindAsync(id);
            if (member == null)
            {
                return NotFound();
            }
            return View(member);
        }

        // POST: Members/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles ="Admin,Staff")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditMembers(int id, [Bind("Name,Gender,BirthDate,PhoneNumber,Email,Category,Validated")] Member member)
        {
            if (id != member.ID)
            {
                return NotFound();
            }
            
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(member);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MemberExists(member.ID))
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
            return View(member);
        }

        // GET: Members/Delete/5 ---> Delete Rent Collection
        [Authorize(Roles ="Admin,Staff")]
        public async Task<IActionResult> DeleteMembers(int? id, bool? saveChangesError = false)
        {
            if (id == null)
            {
                return NotFound();
            }

            var member = await _context.Members.AsNoTracking()
                .FirstOrDefaultAsync(m => m.ID == id);
            if (member == null)
            {
                return NotFound();
            }
            if (saveChangesError.GetValueOrDefault())
            {
                ViewData["ErrorMessage"] = "Delete Failed. Try again and if the problem still continues" +
                    "Look the structure of code.";
            }
            if (member.Validated)
            {
                ViewData["ErrorMessage"] = "Access Denied. The request is only updated by staff after validated";
            }
            return View(member);
        }

        // POST: Members/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize(Roles ="Admin,Staff")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var member = await _context.Members.FindAsync(id);
            if (member == null)
            {
                return RedirectToAction(nameof(Index));
            }
            if (member.Validated == false)
            try
            {
                _context.Members.Remove(member);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException)
            {
                return RedirectToAction(nameof(DeleteMembers), new { id = id, saveChangesError = true });
            }
            return RedirectToAction(nameof(Index));
        }
        private bool MemberExists(int id)
        {
            return _context.Members.Any(e => e.ID == id);
        }
    }
}
