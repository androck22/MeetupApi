using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeetupApi.Data.Models
{
    [Table("Meetups")]
    public class Meetup
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; }
        public string Description { get; set; }
        public string Organizer { get; set; }
        public DateTime EventDate { get; set; }
        public int House { get; set; }
        public int Building { get; set; }
        public string Street { get; set; }

        public List<Sponsor> Sponsors { get; set; }
    }
}
