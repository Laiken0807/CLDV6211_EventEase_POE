namespace CLDV6211_EventEase_POE.Models
{
    public class EventType
    {
      
            public int EventTypeId { get; set; }
            public string? Name { get; set; } // e.g., "Wedding", "Concert", etc.

            public ICollection<Event> Event { get; set; }
        }

    }