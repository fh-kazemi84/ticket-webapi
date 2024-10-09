using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
            var newTicket = _mapper.Map<Ticket>(createTicketDto);

            await _context.Tickets.AddAsync(newTicket);
            await _context.SaveChangesAsync();

            return Ok("Ticket saved successfully");
        }
    
        //Read all
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetTicketDto>>> GetTickets()
        {
            //Get Tickets from Context
            var tickets = await _context.Tickets.Include(t => t.Passenger).ToListAsync();

            var convertedTickets = _mapper.Map<IEnumerable<GetTicketDto>>(tickets);

            return Ok(convertedTickets);
        }

        //Read by name
        [HttpGet]
        [Route("getbyname")]
        public async Task<ActionResult<IEnumerable<GetTicketDto>>> GetTicketsByName(string? name)
        {
            IQueryable<Ticket> query = _context.Tickets;

            if (name is not null)
            {
                query = query.Where(t => t.Passenger.FirstName.Contains(name) || t.Passenger.LastName.Contains(name));
            }

            var tickets = await query.Include(t => t.Passenger).ToListAsync();
            var convertedTickets = _mapper.Map<IEnumerable<GetTicketDto>>(tickets);

            return Ok(convertedTickets);
        }
    }
}