using EventEase.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Reflection;


namespace EventEase.Controllers
{
  

        public class EventsController : Controller
        {
            private readonly EventEaseContext _context;

        [HttpGet]
        public IActionResult EventsAdd()
        {
            return View();
        }

        [HttpPost]
        public IActionResult EventsAdd(Events @event)
        {
            if (!ModelState.IsValid)
            {
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine($"Validation Error: {error.ErrorMessage}");
                }
                return View(@event);
            }

            Console.WriteLine($"Attempting to add event with EventName: {@event.EventName}, EventDate: {@event.EventDate}, Description: {@event.Description}");
            _context.Events.Add(@event);
            try
            {
                var rowsAffected = _context.SaveChanges();
                Console.WriteLine($"SaveChanges affected {rowsAffected} rows");
                if (rowsAffected > 0)
                {
                    return RedirectToAction("EventsView");
                }
                else
                {
                    ModelState.AddModelError("", "Failed to save the event. No rows affected.");
                    return View(@event);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving event: {ex.Message}");
                ModelState.AddModelError("", $"Database error: {ex.Message}");
                return View(@event);
            }
        }



        public EventsController(EventEaseContext context)
            {
                _context = context;
            }

            public IActionResult EventsView()
            {
                return View(_context.Events.ToList());
            }

            public IActionResult EventsEdit(int id)
            {
                var @event = _context.Events.Find(id);
                if (@event == null)
                {
                    return NotFound();
                }
                return View(@event);
            }

            [HttpPost]
            public IActionResult EventsEdit(int id, Events @event)
            {
                if (id != @event.EventId)
                {
                    return NotFound();
                }

                if (ModelState.IsValid)
                {
                    _context.Update(@event);
                    _context.SaveChanges();
                    return RedirectToAction("EventsView");
                }
                return View(@event);
            }

        [HttpGet]
        public IActionResult EventsDelete(int id)
        {
            Console.WriteLine($"Attempting to delete event with ID: {id}");
            var eventItem = _context.Events.Find(id); // Using eventItem to avoid keyword conflict
            if (eventItem == null)
            {
                Console.WriteLine($"Event with ID {id} not found.");
                return NotFound();
            }

            _context.Events.Remove(eventItem);
            try
            {
                var rowsAffected = _context.SaveChanges();
                Console.WriteLine($"SaveChanges affected {rowsAffected} rows");
                if (rowsAffected > 0)
                {
                    return RedirectToAction("EventsView");
                }
                else
                {
                    return Content("Failed to delete the event. No rows affected.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting event: {ex.Message}");
                return Content($"Database error: {ex.Message}");
            }
        }

        [HttpPost, ActionName("EventsDeleteConfirmed")]
            public IActionResult EventsDeleteConfirmed(int id)
            {
                var @event = _context.Events.Find(id);
            if (@event == null)
            {
                Console.WriteLine($"Event with ID {id} not found.");
                return NotFound();
            }
            _context.Events.Remove(@event);
                _context.SaveChanges();
                return RedirectToAction("EventsView");
            }
        }
    }
