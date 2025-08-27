using Microsoft.EntityFrameworkCore;

namespace EventEase.Models
{
    public class EventEaseContext : DbContext
    {
        public DbSet<Venues> Venues { get; set; }
        public DbSet<Events> Events { get; set; }
        public DbSet<Bookings> Bookings { get; set; }

        public EventEaseContext(DbContextOptions<EventEaseContext> options) : base(options)
        {
        }
    }
}
