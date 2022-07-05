using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeetupApi.Contracts.Models.Sponsor
{
    /// <summary>
    /// Добавляет нового спонсора
    /// </summary>
    public class AddSponsorRequest
    {
        public string Name { get; set; }
    }
}
