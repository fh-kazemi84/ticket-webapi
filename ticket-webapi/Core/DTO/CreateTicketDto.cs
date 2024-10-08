using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ticket_webapi.Core.DTO
{
    public class CreateTicketDto
    {
        public DateTime Time { get; set; }
        public PersonDto Passenger { get; set; }
        public string FromCity { get; set; }
        public string ToCity { get; set; }
        public float Price { get; set; }
    }
}