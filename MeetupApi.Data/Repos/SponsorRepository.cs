using MeetupApi.Data.Models;
using MeetupApi.Data.Queries;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MeetupApi.Data.Repos
{
    /// <summary>
    /// Репозиторий для операций с объектами типа "Sponsor" в базе
    /// </summary>
    public class SponsorRepository : ISponsorRepository
    {
        private readonly MeetupApiContext _context;

        public SponsorRepository(MeetupApiContext context)
        {
            _context = context;
        }

        /// <summary>
        ///  Найти всех спонсоров
        /// </summary>
        public List<Sponsor> GetAllSponsors()
        {
            return  _context.Sponsors.ToList();
        }

        /// <summary>
        ///  Найти спонсора по имени
        /// </summary>
        public async Task<Sponsor> GetSponsorByName(string name)
        {
            return await _context.Sponsors.Where(s => s.Name == name).FirstOrDefaultAsync();
        }

        /// <summary>
        ///  Найти спонсора по идентификатору
        /// </summary>
        public async Task<Sponsor> GetSponsorById(Guid id)
        {
            return await _context.Sponsors.Where(s => s.Id == id).FirstOrDefaultAsync();
        }

        /// <summary>
        /// Добавить новый спонсора
        /// </summary>
        public async Task SaveSponsor(Sponsor sponsor)
        {
            var entry = _context.Entry(sponsor);
            if (entry.State == EntityState.Detached)
                await _context.Sponsors.AddAsync(sponsor);

            await _context.SaveChangesAsync();
        }

        /// <summary>
        ///  Обновить существующего спонсора
        /// </summary>
        public async Task UpdateSponsor(Sponsor sponsor, UpdateSponsorQuery query)
        {
            if (!string.IsNullOrEmpty(query.Name))
                sponsor.Name = query.Name;

            var entry = _context.Entry(sponsor);
            if (entry.State == EntityState.Detached)
                _context.Sponsors.Update(sponsor);

            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Удалить существующего спонсора
        /// </summary>
        public async Task DeleteSponsor(Sponsor sponsor)
        {
            _context.Sponsors.Remove(sponsor);

            await _context.SaveChangesAsync();
        }
    }
}
