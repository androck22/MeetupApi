using MeetupApi.Data.Models;
using MeetupApi.Data.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeetupApi.Data.Repos
{
    public interface IMeetupRepository
    {
        Task<IEnumerable<Meetup>> GetMeetups();
        Task<Meetup> GetMeetupByName(string name);
        Task<Meetup> GetMeetupById(Guid id);
        Task SaveMeetup(Meetup meetup, List<Sponsor> sponsors);
        Task UpdateMeetup(Meetup meetup, UpdateMeetupQuery query, List<Sponsor> newSponsors);
        Task DeleteMeetup(Meetup meetup);        
    }
}
