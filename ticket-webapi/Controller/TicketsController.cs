using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ticket_webapi.Core.Context;
using ticket_webapi.Core.DTO;
using ticket_webapi.Core.Entities;

namespace ticket_webapi.Controller
{
    public class TicketsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public TicketsController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        //CRUD

        //Create
        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> createTicket([FromBody] CreateTicketDto createTicketDto)
        {
            var newTicket = new Ticket();

            _mapper.Map(createTicketDto, newTicket);

            await _context.Tickets.AddAsync(newTicket);
            await _context.SaveChangesAsync();

            return Ok("Ticket saved successfully");
        }
    }
}