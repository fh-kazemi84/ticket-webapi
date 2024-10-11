using Microsoft.VisualStudio.TestTools.UnitTesting;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ticket_webapi.Controller;
using ticket_webapi.Core.Context;
using ticket_webapi.Core.DTO;
using ticket_webapi.Core.Entities;
using Microsoft.AspNetCore.Mvc;

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

        //Clean Up the database after each test
        [TestCleanup]
        public void CleanUp()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        //Test: CreateTickets (Post)
        [TestMethod]
        public async Task CreateTicket_ShouldSaveTicketToDatabase()
        {
            // Arrange
            var createTicketDto = new CreateTicketDto
            {
                Time = DateTime.Now,
                FromCity = "Berlin",
                ToCity = "Munich",
                Price = 50.0f,
                Passenger = new PersonDto
                {
                    FirstName = "Sara",
                    LastName = "Meo",
                    PassportNummber = "Z14523654",
                    Nationality = "De",
                    Phone = "14523659875",
                    BirthDate = DateTime.Today,
                    Gender = "Male",
                    Address = "Frankfurt"
                }
            };

            // Act
            var result = await _controller.createTicket(createTicketDto);
            var okResult = result as OkObjectResult;

            // Assert
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);

            // Verify the ticket was saved in the database
            var savedTicket = await _context.Tickets.Include(t => t.Passenger).FirstOrDefaultAsync();
            Assert.IsNotNull(savedTicket); // Ensure the ticket was actually saved
            Assert.AreEqual("Berlin", savedTicket.FromCity);
            Assert.AreEqual("Munich", savedTicket.ToCity);
            Assert.AreEqual("Sara", savedTicket.Passenger.FirstName);
            Assert.AreEqual("Meo", savedTicket.Passenger.LastName);
        }
    }
}