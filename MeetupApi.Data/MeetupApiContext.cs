using MeetupApi.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeetupApi.Data
{
    public class MeetupApiContext : DbContext
    {
        public DbSet<Meetup> Meetups { get; set; }
        public DbSet<Sponsor> Sponsors { get; set; }

        public MeetupApiContext(DbContextOptions<MeetupApiContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Meetup>().ToTable("Meetups");
            builder.Entity<Sponsor>().ToTable("Sponsors");
        }
    }
}
