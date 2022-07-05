using MeetupApi.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeetupApi.Data.Queries
{
    /// <summary>
    /// Класс для передачи дополнительных параметров при обновлении митапа
    /// </summary>
    public class UpdateMeetupQuery
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Organizer { get; set; }
        public DateTime EventDate { get; set; }
        public int House { get; set; }
        public int Building { get; set; }
        public string Street { get; set; }
        public string Sponsors { get; set; }

        public UpdateMeetupQuery(string name, string description, string organizer, DateTime eventDate, int house, int building, string street, string sponsors)
        {
            Name = name;
            Description = description;
            Organizer = organizer;
            EventDate = eventDate;
            House = house;
            Building = building;
            Street = street;
            Sponsors = sponsors;
        }
    }
}
