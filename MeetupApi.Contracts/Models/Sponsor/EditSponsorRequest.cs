using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeetupApi.Contracts.Models.Sponsor
{
    public class EditSponsorRequest
    {
        /// <summary>
        /// Запрос для обновления свойств имеющегося спонсора
        /// </summary>
        public string NewName { get; set; }
    }
}
