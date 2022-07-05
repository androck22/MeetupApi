using AutoMapper;
using MeetupApi.Contracts.Models.Meetup;
using MeetupApi.Contracts.Models.Sponsor;
using MeetupApi.Contracts.Models.User;
using MeetupApi.Data.Models;

namespace MeetupApi
{
    /// <summary>
    /// Настройки маппинга всех сущностей приложения
    /// </summary>
    public class MappingProfile : Profile
    {
        /// <summary>
        /// В конструкторе настроим соответствие сущностей при маппинге
        /// </summary>
        public MappingProfile()
        {
            CreateMap<AddMeetupRequest, Meetup>();
            CreateMap<Meetup, MeetupView>();
            CreateMap<Meetup, GetMeetupByIdResponse>();
            CreateMap<Sponsor, SponsorView>();
            CreateMap<Sponsor, GetSponsorByIdResponse>();
            CreateMap<User, UserViewModel>();
        }
    }
}
