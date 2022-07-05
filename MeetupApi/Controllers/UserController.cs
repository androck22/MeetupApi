using AutoMapper;
using MeetupApi.Contracts.Models.User;
using MeetupApi.Data.Models;
using MeetupApi.Data.Repos;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MeetupApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private IMapper _mapper;
        private IUserRepository _users;
        public UserController(IMapper mapper, IUserRepository users)
        {
            _mapper = mapper;
            _users = users;
        }

        [HttpGet]
        public User GetUser()
        {
            return new User()
            {
                Id = Guid.NewGuid(),
                FirstName = "Иван",
                LastName = "Иванов",
                Email = "ivan@gmail.com",
                Password = "11111122222qq",
                Login = "ivanov"
            };
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        [Route("viewmodel")]
        public UserViewModel GetUserViewModel()
        {
            User user = new User()
            {
                Id = Guid.NewGuid(),
                FirstName = "Иван",
                LastName = "Иванов",
                Email = "ivan@gmail.com",
                Password = "11111122222qq",
                Login = "ivanov"
            };

            var userViewModel = _mapper.Map<UserViewModel>(user);

            return userViewModel;
        }

        [HttpPost]
        [Route("authenticate")]
        public async Task<UserViewModel> Authenticate(string login, string password)
        {
            if (String.IsNullOrEmpty(login) || String.IsNullOrEmpty(password))
                throw new ArgumentNullException("Логин или пароль не корректен");

            User user = _users.GetByLogin(login);
            if (user is null)
                throw new AuthenticationException("Пользователь не найден");

            if (user.Password != password)
                throw new AuthenticationException("Введенный пароль не корректен");

            var claim = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, user.Login),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, user.Role.Name)
            };

            ClaimsIdentity claimsIdentity = new ClaimsIdentity(
                claim,
                "AppCookie",
                ClaimsIdentity.DefaultNameClaimType,
                ClaimsIdentity.DefaultRoleClaimType);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

            return _mapper.Map<User, UserViewModel>(user);
        }
    }
}
