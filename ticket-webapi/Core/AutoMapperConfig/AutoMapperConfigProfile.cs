using AutoMapper;
using ticket_webapi.Core.DTO;
using ticket_webapi.Core.Entities;

namespace ticket_webapi.Core.AutoMapperConfig
{
    public class AutoMapperConfigProfile : Profile
    {
        public AutoMapperConfigProfile()
        {
            //Tickets
            CreateMap<CreateTicketDto, Ticket>();
        }
    }
}