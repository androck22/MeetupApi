using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeetupApi.Data.Queries
{
    public class UpdateSponsorQuery
    {
        /// <summary>
        /// Класс для передачи дополнительных параметров при обновлении спонсора
        /// </summary>
        public string Name { get; set; }

        public UpdateSponsorQuery(string name)
        {
            Name = name;
        }
    }
}
