using MeetupApi.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeetupApi.Contracts.Models.Meetup
{
    /// <summary>
    /// Добавляет новое событие
    /// </summary>
    public class AddMeetupRequest
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Organizer { get; set; }
        public DateTime EventDate { get; set; }
        public LocationInfo LocationInfo { get; set; }
        public string SponsorNames { get; set; }
    }

    public class LocationInfo
    {
        public int House { get; set; }
        public int Building { get; set; }
        public string Street { get; set; }
    }
}
