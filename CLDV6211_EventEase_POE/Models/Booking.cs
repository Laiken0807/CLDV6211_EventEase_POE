using System.ComponentModel.DataAnnotations;

namespace CLDV6211_EventEase_POE.Models
{
    public class Booking
    {
        [Key]
        public int BookingId { get; set; }

        public string? BookingName { get; set; }
        public int EventId { get; set; }

        public int VenueId { get; set; }

        public DateTime BookingDate { get; set; } = DateTime.Now;

        public TimeSpan BookingTime { get; set; }

        public Event? Event { get; set; }

        public Venue? Venue { get; set; }

    }

}
