using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Logging;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EventEase.Models
{
    public class Bookings
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BookingId { get; set; }

        [Required]
        public int EventId { get; set; }

        [ForeignKey("EventId")]
        [BindNever]
        public Events Event { get; set; }

        [Required]
        public int VenueId { get; set; }

        [ForeignKey("VenueId")]
        [BindNever]
        public Venues Venue { get; set; }

        [Required]
        public DateTime BookingDate { get; set; }
    }
}
