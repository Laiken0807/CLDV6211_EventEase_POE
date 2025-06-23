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
    public class EventsController : Controller
    {
        private readonly CLDV6211_EventEase_POEContext _context;

        public EventsController(CLDV6211_EventEase_POEContext context)
        {
            _context = context;
        }

        // GET: Events
        public async Task<IActionResult> Index(string searchstring)
        {
            if (_context.Event == null)
            {
                return Problem("Entity set not found");
            }
            var events = from e in _context.Event
                           select e;

            if (!String.IsNullOrEmpty(searchstring))
            {
                events = events.Where(e => e.eventName!.ToUpper().Contains(searchstring.ToUpper()));
            }
            return View(await events.ToListAsync());
        }

        // GET: Events/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @event = await _context.Event
                .FirstOrDefaultAsync(m => m.EventId == id);
            if (@event == null)
            {
                return NotFound();
            }

            return View(@event);
        }

        // GET: Events/Create
        public IActionResult Create()
        {
            ViewBag.VenueId = new SelectList(_context.Venue.ToList(), "VenueId", "venueName");
            ViewBag.EventTypeId = new SelectList(_context.EventType.ToList(), "EventTypeId", "Name");
            return View();
        }

        


        // POST: Events/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
       
        public async Task<IActionResult> Create([Bind("EventId,eventName,eventDate,eventTime,description,VenueId,EventTypeId")] Event @event)
        {
            if (ModelState.IsValid)
            {
                bool isDoubleBooked = _context.Event.Any(e => e.eventDate == @event.eventDate && e.eventTime == @event.eventTime);

                if (isDoubleBooked)
                {
                    ModelState.AddModelError("", "An event is already booked at this date and time");
                }
                else
                {
                    _context.Add(@event);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }


            ViewBag.VenueId = new SelectList(_context.Venue.ToList(), "VenueId", "venueName"); 
            ViewBag.EventTypeId = new SelectList(_context.EventType.ToList(), "EventTypeId", "Name");

            return View(@event);
        }




        // GET: Events/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @event = await _context.Event.FindAsync(id);
            if (@event == null)
            {
                return NotFound();
            }
            return View(@event);
        }

        // POST: Events/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("EventId,eventName,eventDate,eventTime,description")] Event @event)
        {
            if (id != @event.EventId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(@event);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EventExists(@event.EventId))
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
            return View(@event);
        }

        // GET: Events/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @event = await _context.Event
                .FirstOrDefaultAsync(m => m.EventId == id);
            if (@event == null)
            {
                return NotFound();
            }

            return View(@event);
        }

        // POST: Events/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            bool events = await _context.Event.AnyAsync(gp => gp.EventId == id);

            if (events)
            {
                var Events = await _context.Event.FindAsync(id);
                ModelState.AddModelError("", "Cannot delete this event there are existing event records");
            
            }
            var bookingToDelete = await _context.Venue.FindAsync(id);
            _context.Venue.Remove(bookingToDelete);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EventExists(int id)
        {
            return _context.Event.Any(e => e.EventId == id);
        }
        public async Task<IActionResult> Search(int? eventTypeId, DateTime? startDate, DateTime? endDate, bool onlyAvailable = false)
        {
            var events = _context.Event
                .Include(e => e.EventType)
                .Include(e => e.Venue)
                .AsQueryable();

            // Filter by event type if provided
            if (eventTypeId.HasValue)
                events = events.Where(e => e.EventTypeId == eventTypeId);

            // Filter by date range if provided
            if (startDate.HasValue && endDate.HasValue)
            {
                // Convert startDate and endDate to DateOnly to match eventDate type
                var start = DateOnly.FromDateTime(startDate.Value);
                var end = DateOnly.FromDateTime(endDate.Value);

                events = events.Where(e => e.eventDate >= start && e.eventDate <= end);
            }

            // Filter by availability if requested
            if (onlyAvailable)
            {
                // Get all bookings as IQueryable to avoid loading all into memory
                var bookings = _context.Booking.AsQueryable();

                events = events.Where(e =>
                    !bookings.Any(b =>
                        b.VenueId == e.VenueId &&
                        // Convert BookingDate (DateTime) to DateOnly for comparison with e.eventDate
                        DateOnly.FromDateTime(b.BookingDate) == e.eventDate
                    ));
            }

            ViewBag.EventTypes = await _context.EventType.ToListAsync();

            return View("Search", await events.ToListAsync());
        }

    }
}
