using EventEase.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Reflection;

namespace EventEase.Controllers
{
    public class VenuesController : Controller
    {
        private readonly EventEaseContext _context;

        [HttpGet]
        public IActionResult VenuesAdd()
        {
            return View();
        }

        [HttpPost]
        public IActionResult VenuesAdd(Venues venue)
        {
            if (ModelState.IsValid)
            {
                _context.Venues.Add(venue);
                _context.SaveChanges();
                return RedirectToAction("VenuesView");
            }
            return View(venue);
        }




        public VenuesController(EventEaseContext context)
        {
            _context = context;
        }

        public IActionResult VenuesView()
        {
            return View(_context.Venues.ToList());
        }

        public IActionResult VenuesEdit(int id)
        {
            var venue = _context.Venues.Find(id);
            if (venue == null)
            {
                return NotFound();
            }
            return View(venue);
        }

        [HttpPost]
        public IActionResult VenuesEdit(int id, Venues venue)
        {
            if (id != venue.VenueId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _context.Update(venue);
                _context.SaveChanges();
                return RedirectToAction("VenuesView");
            }
            return View(venue);
        }
        [HttpGet]
        public IActionResult VenuesDelete(int id)
        {
            Console.WriteLine($"Attempting to delete venue with ID: {id}");
            var venue = _context.Venues.Find(id);
            if (venue == null)
            {
                Console.WriteLine($"Venue with ID {id} not found.");
                return NotFound();
            }

            _context.Venues.Remove(venue);
            try
            {
                var rowsAffected = _context.SaveChanges();
                Console.WriteLine($"SaveChanges affected {rowsAffected} rows");
                if (rowsAffected > 0)
                {
                    return RedirectToAction("VenuesView");
                }
                else
                {
                    return Content("Failed to delete the venue. No rows affected.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting venue: {ex.Message}");
                return Content($"Database error: {ex.Message}");
            }
        }

        [HttpPost, ActionName("VenuesDeleteConfirmed")]
        public IActionResult VenuesDeleteConfirmed(int id)
        {
            var venue = _context.Venues.Find(id);
            if (venue == null)
            {
                Console.WriteLine($"Venue with ID {id} not found.");
                return NotFound();
            }
            _context.Venues.Remove(venue);
            _context.SaveChanges();
            return RedirectToAction("VenuesView");
        }
    }
}
