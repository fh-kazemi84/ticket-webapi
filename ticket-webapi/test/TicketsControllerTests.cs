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

        //Test: GetTickets (GET)
        [TestMethod]
        public async Task GetTickets_ShouldReturnAllTicketsWithPassengers()
        {
            // Arrange
            _context.Tickets.Add(new Ticket
            {
                FromCity = "Berlin",
                ToCity = "Munich",
                Passenger = new Person
                {
                    FirstName = "Sara",
                    LastName = "Meo",
                    Nationality = "US",
                    PassportNummber = "H523695321",
                    BirthDate = DateTime.Now,
                    Gender = "f",
                    Address = "Sanferasisco",
                    Phone = "1234567890"
                }
            });

            _context.Tickets.Add(new Ticket
            {
                FromCity = "Hamburg",
                ToCity = "Cologne",
                Passenger = new Person
                {
                    FirstName = "John",
                    LastName = "Doe",
                    Nationality = "EN",
                    PassportNummber = "N564457545",
                    BirthDate = DateTime.Now,
                    Gender = "m",
                    Address = "London",
                    Phone = "0987654321"
                }
            });

            await _context.SaveChangesAsync();

            // Act
            var result = await _controller.GetTickets();
            var okResult = result.Result as OkObjectResult;
            var tickets = okResult.Value as IEnumerable<GetTicketDto>;

            // Assert
            Assert.IsNotNull(okResult);
            Assert.IsNotNull(tickets);

            // Convert to list to assert properties
            var ticketList = tickets.ToList();

            Assert.AreEqual(2, ticketList.Count);
            Assert.AreEqual("Berlin", ticketList[0].FromCity);
            Assert.AreEqual("Munich", ticketList[0].ToCity);
            Assert.AreEqual("Sara", ticketList[0].Passenger.FirstName); // Check passenger information
            Assert.AreEqual("Hamburg", ticketList[1].FromCity);
            Assert.AreEqual("Cologne", ticketList[1].ToCity);
            Assert.AreEqual("John", ticketList[1].Passenger.FirstName); // Check passenger information
        }

        //Test: GetTicketById (GET by ID)
        [TestMethod]
        public async Task GetTicketById_ShouldReturnCorrectTicket()
        {
            // Arrange
            _context.Tickets.Add(new Ticket
            {
                Id = 1,
                FromCity = "Berlin",
                ToCity = "Munich",
                Passenger = new Person
                {
                    FirstName = "Sara",
                    LastName = "Meo",
                    Nationality = "US",
                    PassportNummber = "H523695321",
                    BirthDate = DateTime.Now,
                    Gender = "f",
                    Address = "Sanferasisco",
                    Phone = "1234567890"
                }
            });
            await _context.SaveChangesAsync();

            // Act
            var result = await _controller.GetTicketById(1);
            var okResult = result.Result as OkObjectResult;
            var ticketDto = okResult.Value as GetTicketDto;

            // Assert
            Assert.IsNotNull(ticketDto);
            Assert.AreEqual("Berlin", ticketDto.FromCity);
            Assert.AreEqual("Sara", ticketDto.Passenger.FirstName);
        }   

        // Test: UpdateTicket (PUT)
        [TestMethod]
        public async Task UpdateTicket_ShouldUpdateTicketDetails()
        {
            // Arrange
            _context.Tickets.Add(new Ticket
            {
                Id = 1,
                FromCity = "Berlin",
                ToCity = "Munich",
                Passenger = new Person
                {
                    FirstName = "Sara",
                    LastName = "Meo",
                    Nationality = "US",
                    PassportNummber = "H523695321",
                    BirthDate = DateTime.Now,
                    Gender = "f",
                    Address = "Sanferasisco",
                    Phone = "1234567890"
                }
            });
            await _context.SaveChangesAsync();

            var updateTicketDto = new UpdateTicketDto
            {
                Time = DateTime.Now,
                FromCity = "Hamburg",
                ToCity = "London",
                Price = 104,
                Passenger = new PersonDto
                {
                    FirstName = "Sara",
                    LastName = "Meo",
                    Nationality = "US",
                    PassportNummber = "H523695321",
                    BirthDate = DateTime.Now,
                    Gender = "f",
                    Address = "Sanferasisco",
                    Phone = "1234567890"
                }
            };

            // Act
            var result = await _controller.UpdateTicket(1, updateTicketDto);

            // Assert
            var updatedTicket = _context.Tickets.First(t => t.Id == 1);
            Assert.AreEqual("Hamburg", updatedTicket.FromCity);
        }   

        // Test: DeleteTicket (DELETE)
        [TestMethod]
        public async Task DeleteTicket_ShouldRemoveTicketFromDatabase()
        {
            // Arrange
            _context.Tickets.Add(new Ticket
            {
                Id = 1,
                FromCity = "Berlin",
                ToCity = "Munich",
                Passenger = new Person
                {
                    FirstName = "Sara",
                    LastName = "Meo",
                    Nationality = "US",
                    PassportNummber = "H523695321",
                    BirthDate = DateTime.Now,
                    Gender = "f",
                    Address = "Sanferasisco",
                    Phone = "1234567890"
                }
            });
            await _context.SaveChangesAsync();

            // Act
            var result = await _controller.DeleteTicket(1);

            // Assert
            Assert.AreEqual(0, _context.Tickets.Count());
        }  
    } 
}