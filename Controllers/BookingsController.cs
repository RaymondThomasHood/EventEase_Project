
using Microsoft.AspNetCore.Mvc;
using EventEase.Models;
using Microsoft.EntityFrameworkCore;

namespace EventEase.Controllers
{
    public class BookingsController : Controller
    {
        private readonly EventEaseContext _context;

        public BookingsController(EventEaseContext context)
        {
            _context = context;
        }
        [HttpGet]
        public IActionResult BookingsView()
        {
            var bookings = _context.Bookings.ToList();
            Console.WriteLine($"Bookings count: {bookings?.Count ?? 0}");
            if (bookings != null)
            {
                foreach (var booking in bookings)
                {
                    Console.WriteLine($"Booking ID: {booking.BookingId}, EventId: {booking.EventId}, VenueId: {booking.VenueId}, BookingDate: {booking.BookingDate}");
                }
            }
            else
            {
                Console.WriteLine("Bookings is null");
            }
            return View(bookings);
        }

        [HttpGet]
        public IActionResult BookingsAdd()
        {
            var events = _context.Events.ToList();
            var venues = _context.Venues.ToList();
            Console.WriteLine($"Events count: {events?.Count ?? 0}, Venues count: {venues?.Count ?? 0}");
            ViewBag.Events = events ?? new List<Events>();
            ViewBag.Venues = venues ?? new List<Venues>();
            return View();
        }

        [HttpPost]
       
        public IActionResult BookingsAdd([Bind("EventId,VenueId,BookingDate")] Bookings booking)
        {
            //if (!ModelState.IsValid)
            //{
            //    foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
            //    {
            //        Console.WriteLine($"Validation Error: {error.ErrorMessage}");
            //    }
            //    return View(booking);
            //}

            
            if (!_context.Events.Any(e => e.EventId == booking.EventId))
            {
                ModelState.AddModelError("EventId", "Selected EventId does not exist.");
                return View(booking);
            }
            if (!_context.Venues.Any(v => v.VenueId == booking.VenueId))
            {
                ModelState.AddModelError("VenueId", "Selected VenueId does not exist.");
                return View(booking);
            }

            Console.WriteLine($"Attempting to add booking with EventId: {booking.EventId}, VenueId: {booking.VenueId}, BookingDate: {booking.BookingDate}");
            _context.Bookings.Add(booking);
            try
            {
                var rowsAffected = _context.SaveChanges();
                Console.WriteLine($"SaveChanges affected {rowsAffected} rows");
                if (rowsAffected > 0)
                {
                    return RedirectToAction("BookingsView");
                }
                else
                {
                    ModelState.AddModelError("", "Failed to save the booking. No rows affected. Check database constraints.");
                    return View(booking);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving booking: {ex.Message}");
                ModelState.AddModelError("", $"Database error: {ex.Message}");
                return View(booking);
            }
        }

        public IActionResult BookingsEdit(int id)
        {
            var booking = _context.Bookings.Find(id);
            if (booking == null)
            {
                return NotFound();
            }
            return View(booking);
        }

        [HttpPost]
        public IActionResult BookingsEdit(int id, Bookings booking)
        {
            if (id != booking.BookingId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _context.Update(booking);
                _context.SaveChanges();
                return RedirectToAction("BookingsView");
            }
            return View(booking);
        }

        [HttpGet]
        public IActionResult BookingsDelete(int id)
        {
            Console.WriteLine($"Attempting to delete booking with ID: {id}");
            var booking = _context.Bookings.Find(id);
            if (booking == null)
            {
                Console.WriteLine($"Booking with ID {id} not found.");
                return NotFound();
            }

            _context.Bookings.Remove(booking);
            try
            {
                var rowsAffected = _context.SaveChanges();
                Console.WriteLine($"SaveChanges affected {rowsAffected} rows");
                if (rowsAffected > 0)
                {
                    return RedirectToAction("BookingsView");
                }
                else
                {
                    return Content("Failed to delete the booking. No rows affected.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting booking: {ex.Message}");
                return Content($"Database error: {ex.Message}");
            }
        }

        [HttpPost, ActionName("BookingsDeleteConfirmed")]
        public IActionResult BookingsDeleteConfirmed(int id)
        {
            Console.WriteLine($"Attempting to delete booking with ID: {id}");
            var booking = _context.Bookings.Find(id);
            if (booking == null)
            {
                Console.WriteLine($"Booking with ID {id} not found.");
                return NotFound();
            }

            Console.WriteLine($"Deleting booking with EventId: {booking.EventId}, VenueId: {booking.VenueId}");
            _context.Bookings.Remove(booking);
            try
            {
                var rowsAffected = _context.SaveChanges();
                Console.WriteLine($"SaveChanges affected {rowsAffected} rows");
                if (rowsAffected > 0)
                {
                    return RedirectToAction("BookingsView");
                }
                else
                {
                    ModelState.AddModelError("", "Failed to delete the booking. No rows affected.");
                    return View("BookingsDelete", booking);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting booking: {ex.Message}");
                ModelState.AddModelError("", $"Database error: {ex.Message}");
                return View("BookingsDelete", booking);
            }
        }
    }
}