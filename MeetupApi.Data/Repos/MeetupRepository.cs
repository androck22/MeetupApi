using MeetupApi.Data.Models;
using MeetupApi.Data.Queries;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeetupApi.Data.Repos
{
    public class MeetupRepository : IMeetupRepository
    {
        private readonly MeetupApiContext _context;

        public MeetupRepository(MeetupApiContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Выгрузить все митапы
        /// </summary>
        public async Task<IEnumerable<Meetup>> GetMeetups()
        {
            return await _context.Meetups.Include(m => m.Sponsors).ToListAsync();
        }

        /// <summary>
        /// Найти митап по имени
        /// </summary>
        public async Task<Meetup> GetMeetupByName(string name)
        {
            return await _context.Meetups.Include(m => m.Sponsors).Where(m => m.Name == name).FirstOrDefaultAsync();
        }

        /// <summary>
        /// Найти митап по идентификатору
        /// </summary>
        public async Task<Meetup> GetMeetupById(Guid id)
        {
            return await _context.Meetups.Include(m => m.Sponsors).Where(m => m.Id == id).FirstOrDefaultAsync();
        }

        /// <summary>
        /// Добавить новый митап
        /// </summary>
        public async Task SaveMeetup(Meetup meetup, List<Sponsor> sponsors)
        {
            //meetup.SponsorId = sponsor.Id;

            var entry = _context.Entry(meetup);
            if (entry.State == EntityState.Detached)
                await _context.Meetups.AddAsync(meetup);

            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Обновить существующий митап
        /// </summary>
        public async Task UpdateMeetup(Meetup meetup, 
            UpdateMeetupQuery query, List<Sponsor> newSponsors)
        {
            if (!string.IsNullOrEmpty(query.Name))
                meetup.Name = query.Name;
            if (!string.IsNullOrEmpty(query.Description))
                meetup.Description = query.Description;
            if (!string.IsNullOrEmpty(query.Organizer))
                meetup.Organizer = query.Organizer;
            if (!string.IsNullOrEmpty(query.EventDate.ToString()))
                meetup.EventDate = query.EventDate;
            if (!string.IsNullOrEmpty(query.House.ToString()))
                meetup.House = query.House;
            if (!string.IsNullOrEmpty(query.Building.ToString()))
                meetup.Building = query.Building;
            if (!string.IsNullOrEmpty(query.Street))
                meetup.Street = query.Street;
            if (!string.IsNullOrEmpty(query.Sponsors))
                meetup.Sponsors = newSponsors;

            var entry = _context.Entry(meetup);
            if (entry.State == EntityState.Detached)
                _context.Meetups.Update(meetup);

            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Удалить митап
        /// </summary>
        public async Task DeleteMeetup(Meetup meetup)
        {
            _context.Meetups.Remove(meetup);

            await _context.SaveChangesAsync();
        }
    }
}
