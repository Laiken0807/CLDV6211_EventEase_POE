using CLDV6211_EventEase_POE.Models;
using System.ComponentModel.DataAnnotations;

namespace CLDV6211_EventEase_POE.Models
{
    public class Event
    {
        [Key]
        public int EventId { get; set; }

        public string? eventName { get; set; }

        public DateOnly eventDate { get; set; }

        public TimeOnly eventTime { get; set; }

        public string? description { get; set; }

        public int VenueId { get; set; }
        public Venue?Venue { get; set; }

        public int EventTypeId { get; set; }
        public EventType? EventType { get; set; }

    }
}
