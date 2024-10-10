using Microsoft.VisualStudio.TestTools.UnitTesting;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ticket_webapi.Controller;
using ticket_webapi.Core.Context;
using ticket_webapi.Core.DTO;
using ticket_webapi.Core.Entities;


namespace ticket_webapi.test
{
    [TestClass]
    public class TicketsControllerTests
    {
        private ApplicationDbContext _context;
        private IMapper _mapper;
        private TicketsController _controller;

        //Run before each test
        [TestInitialize]
        public void Setup()
        {
            //Setup the in-memory database
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;
            _context = new ApplicationDbContext(options);

            //Configure AutoMapper
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<CreateTicketDto, Ticket>();
                cfg.CreateMap<Ticket, GetTicketDto>().ForMember(des => des.Passenger, opt => opt.MapFrom(src => src.Passenger));
                cfg.CreateMap<UpdateTicketDto, Ticket>();
                cfg.CreateMap<PersonDto, Person>();
                cfg.CreateMap<Person, PersonDto>();
            });
            _mapper = config.CreateMapper();

            //Initialize controller with mocked dependencies
            _controller = new TicketsController(_context, _mapper);
        }

    }
}