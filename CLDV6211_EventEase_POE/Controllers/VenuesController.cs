using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CLDV6211_EventEase_POE.Data;
using CLDV6211_EventEase_POE.Models;

namespace CLDV6211_EventEase_POE.Controllers
{
    public class VenuesController : Controller
    {
        private readonly CLDV6211_EventEase_POEContext _context;

        public VenuesController(CLDV6211_EventEase_POEContext context)
        {
            _context = context;
        }

        // GET: Venues
        public async Task<IActionResult> Index(string searchstring)
        {
            if (_context.Venue == null)
            {
                return Problem("Entity set not found");
            }
            var venues = from v in _context.Venue
                         select v;

            if (!String.IsNullOrEmpty(searchstring))
            {
                venues = venues.Where(v => v.venueName!.ToUpper().Contains(searchstring.ToUpper()));
            }
            return View(await venues.ToListAsync());
        }

        // GET: Venues/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var venue = await _context.Venue
                .FirstOrDefaultAsync(m => m.VenueId == id);
            if (venue == null)
            {
                return NotFound();
            }

            return View(venue);
        }

        // GET: Venues/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Venues/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("VenueId,venueName,location,capacity,VenueDate,VenueTime")] Venue venue)
        {
            if (ModelState.IsValid)
            {


                bool isDoubleBooked = await _context.Venue.AnyAsync(v => v.VenueDate == venue.VenueDate && v.VenueTime == venue.VenueTime);

                if (isDoubleBooked)
                {
                    ModelState.AddModelError("", "An event is already booked at this date and time");
                    return View(venue);

                }
               
                _context.Venue.Add(venue);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(venue);
        }

        // GET: Venues/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var venue = await _context.Venue.FindAsync(id);
            if (venue == null)
            {
                return NotFound();
            }
            return View(venue);
        }

        // POST: Venues/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("VenueId,venueName,location,capacity,VenueDate,VenueTime")] Venue venue)
        {
            if (id != venue.VenueId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(venue);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VenueExists(venue.VenueId))
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
            return View(venue);
        }

        // GET: Venues/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var venue = await _context.Venue
                .FirstOrDefaultAsync(m => m.VenueId == id);
            if (venue == null)
            {
                return NotFound();
            }

            return View(venue);
        }

        // POST: Venues/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            bool venues = await _context.Venue.AnyAsync(gp => gp.VenueId == id);

            if (venues)
            {
                var Venue = await _context.Venue.FindAsync(id);
                ModelState.AddModelError("", "Cannot delete this venue as there are existing venue records");
                return View(Venue);
            }
            var bookingToDelete = await _context.Booking.FindAsync(id);
            _context.Booking.Remove(bookingToDelete);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VenueExists(int id)
        {
            return _context.Venue.Any(e => e.VenueId == id);
        }
    }
}
