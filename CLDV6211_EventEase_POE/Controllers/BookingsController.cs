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
    public class BookingsController : Controller
    {
        private readonly CLDV6211_EventEase_POEContext _context;

        public BookingsController(CLDV6211_EventEase_POEContext context)
        {
            _context = context;
        }

        // GET: Bookings
        public async Task<IActionResult> Index(string searchstring)
        {
            if (_context.Booking == null)
            {
                return Problem("Entity set not found");
            }
            var bookings = from b in _context.Booking
                           select b;

            if (!String.IsNullOrEmpty(searchstring))
            {
                bookings = bookings.Where(b => b.BookingName!.ToUpper().Contains(searchstring.ToUpper()));
            }
            return View(await bookings.ToListAsync());
        
        }
        public bool IsVenueAvailable(int venueId, DateTime eventDate)
        {
            return !_context.Booking.Any(b => b.VenueId == venueId && b.BookingDate == eventDate);
        }


        // GET: Bookings/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var booking = await _context.Booking
                .Include(b => b.Event)
                .Include(b => b.Venue)
                .FirstOrDefaultAsync(m => m.BookingId == id);
            if (booking == null)
            {
                return NotFound();
            }

            return View(booking);
        }

        // GET: Bookings/Create
        public IActionResult Create()
        {
            ViewData["EventId"] = new SelectList(_context.Set<Event>(), "EventId", "EventId");
            ViewData["VenueId"] = new SelectList(_context.Set<Venue>(), "VenueId", "VenueId");
            return View();
        }

        // POST: Bookings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BookingId,EventId,VenueId,BookingDate,BookingTime")] Booking booking)
        {
            if (ModelState.IsValid)
            {
                // Ensure no other booking exists at the same venue, date, and time
                bool isDoubleBooked = _context.Booking.Any(b =>
                    b.VenueId == booking.VenueId &&
                    b.BookingDate == booking.BookingDate && b.BookingTime == booking.BookingTime);

                if (isDoubleBooked)
                {
                    ModelState.AddModelError("", "This venue is already booked at the selected date and time.");
                    return View(booking);
                }

                _context.Booking.Add(booking);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            // If model validation fails, show errors in view
            return View(booking);
        }

        // GET: Bookings/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var booking = await _context.Booking.FindAsync(id);
            if (booking == null)
            {
                return NotFound();
            }
            ViewData["EventId"] = new SelectList(_context.Set<Event>(), "EventId", "EventId", booking.EventId);
            ViewData["VenueId"] = new SelectList(_context.Set<Venue>(), "VenueId", "VenueId", booking.VenueId);
            return View(booking);
        }

        // POST: Bookings/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("BookingId,EventId,VenueId,BookingDate,BookingTime")] Booking booking)
        {
            if (id != booking.BookingId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(booking);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookingExists(booking.BookingId))
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
            ViewData["EventId"] = new SelectList(_context.Set<Event>(), "EventId", "EventId", booking.EventId);
            ViewData["VenueId"] = new SelectList(_context.Set<Venue>(), "VenueId", "VenueId", booking.VenueId);
            return View(booking);
        }

        // GET: Bookings/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var booking = await _context.Booking
                .Include(b => b.Event)
                .Include(b => b.Venue)
                .FirstOrDefaultAsync(m => m.BookingId == id);
            if (booking == null)
            {
                return NotFound();
            }

            return View(booking);
        }

        // POST: Bookings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            bool bookings = await _context.Booking.AnyAsync(gp => gp.BookingId == id);

            if (bookings)
            {
                // retrieve to let display Delete with error
                var booking = await _context.Booking.FindAsync(id);
                ModelState.AddModelError("", "Cannot delete this booking, there are existing booking records");
                return View(booking);
            }
            var bookingToDelete = await _context.Event.FindAsync(id);
            _context.Event.Remove(bookingToDelete);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BookingExists(int id)
        {
            return _context.Booking.Any(e => e.BookingId == id);
        }

    }
}
