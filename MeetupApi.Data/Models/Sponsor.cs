using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeetupApi.Data.Models
{
    [Table("Soponsors")]
    public class Sponsor
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; }

        public ICollection<Meetup> Meetups { get; set; }
    }
}
