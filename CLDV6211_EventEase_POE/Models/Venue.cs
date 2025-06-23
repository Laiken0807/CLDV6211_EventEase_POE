using System.ComponentModel.DataAnnotations;

namespace CLDV6211_EventEase_POE.Models
{
    public class Venue
    {
        [Key]
        public int VenueId { get; set; }

        public string? venueName { get; set; }
        public string? location { get; set; }
        public int capacity { get; set; }
    
        public DateOnly? VenueDate { get; set; }
        
        public TimeOnly? VenueTime { get; set; }
    }
}
