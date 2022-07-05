using MeetupApi.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeetupApi.Data.Repos
{
    public interface IUserRepository
    {
        IEnumerable<User> GetAll();
        User GetByLogin(string login);
    }
}
