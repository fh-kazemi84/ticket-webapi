using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ticket_webapi.Core.DTO
{
    public class PersonDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PassportNummber { get; set; }
        public string Nationality { get; set; }
        public string Phone { get; set; }
        public DateTime BirthDate { get; set; }
        public string Gender { get; set; }
        public string Address { get; set; }
    }
}