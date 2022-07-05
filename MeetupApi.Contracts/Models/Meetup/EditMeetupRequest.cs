using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeetupApi.Contracts.Models.Meetup
{
    public class EditMeetupRequest
    {
        /// <summary>
        /// Запрос для обновления свойств имеющегося митапа
        /// </summary>
        public string NewName { get; set; }
        public string NewDescription { get; set; }
        public string NewOrganizer { get; set; }
        public DateTime NewEventDate { get; set; }
        public NewLocationInfo NewLocationInfo { get; set; }
        public string NewSponsorNames { get; set; }
    }

    public class NewLocationInfo
    {
        public int NewHouse { get; set; }
        public int NewBuilding { get; set; }
        public string NewStreet { get; set; }
    }
}
