namespace ticket_webapi.Core.DTO
{
    public class GetTicketDto
    {
        public long Id { get; set; }
        public DateTime Time { get; set; }
        public PersonDto Passenger { get; set; }
        public string FromCity { get; set; }
        public string ToCity { get; set; }
        public float Price { get; set; }
    }
}