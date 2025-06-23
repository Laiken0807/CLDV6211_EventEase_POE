using System.ComponentModel.DataAnnotations.Schema;

namespace CLDV6211_EventEase_POE.Models
{
    [Table("Events")]
    public class eventease
    {
        public int Id { get; set;}

        public string Name { get; set; }

        public string description { get; set; }

        public string ImageUrl { get; set; } // Storage blob url
    }
}
