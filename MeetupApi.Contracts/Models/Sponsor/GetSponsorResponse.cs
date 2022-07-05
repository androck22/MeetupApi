using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeetupApi.Contracts.Models.Sponsor
{
    public class GetSponsorResponse
    {
        public int SponsorAmount { get; set; }
        public List<SponsorView> Sponsors { get; set; }
    }

    public class SponsorView
    {
        public string Name { get; set; }
    }
}
