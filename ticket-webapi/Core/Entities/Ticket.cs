using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ticket_webapi.Core.Entities
{
    public class Ticket
    {
        [Key]
        public long Id { get; set; }
        public DateTime Time { get; set; }
        public Person Passenger { get; set; }
        public string FromCity { get; set; }
        public string ToCity { get; set; }
        public int Price { get; set; }
        public DateTime CreateAt { get; set; } = DateTime.Now;
        public DateTime UpdateAt { get; set; } = DateTime.Now;
        public string ConfidentialComment { get; set; } = "Normal";
    }
}