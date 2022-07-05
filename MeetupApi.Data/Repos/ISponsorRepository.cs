using MeetupApi.Data.Models;
using MeetupApi.Data.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeetupApi.Data.Repos
{
    /// <summary>
    /// Интерфейс определяет методы для доступа к объектам типа Sponsor в базе 
    /// </summary>
    public interface ISponsorRepository
    {
        List<Sponsor> GetAllSponsors();
        Task<Sponsor> GetSponsorByName(string name);
        Task<Sponsor> GetSponsorById(Guid id);
        Task SaveSponsor(Sponsor sponsor);
        Task UpdateSponsor(Sponsor sponsor, UpdateSponsorQuery query);
        Task DeleteSponsor(Sponsor sponsor);
    }
}
