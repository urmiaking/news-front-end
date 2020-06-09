using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Http;
using News.Services.Repositories;
using System.Threading.Tasks;
using News.Models.DomainModels;
using News.Models.ViewModels;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Cryptography;
using System.Threading;

namespace News.Services.Services
{
    public class FakeUserRepository : IUserRepository
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private List<User> users;

        public FakeUserRepository(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;

            users = new List<User>
            {
                new User()
                {
                    Id = 1,
                    Email = "a@b.com",
                    FullName = "علی صبوری",
                    Password = "db901737c41e490dec8bded913f112e5e7c720c3847558f0e5c65128bdb1b34c",
                    UserType = "user",
                    IsActive = false
                },

                new User()
                {
                    Id = 2,
                    Email = "masoud.brilliant@hotmail.com",
                    FullName = "نگین صبوری",
                    Password = "db901737c41e490dec8bded913f112e5e7c720c3847558f0e5c65128bdb1b34c",
                    UserType = "user",
                    IsActive = true
                },

                new User()
                {
                    Id = 3,
                    Email = "c@b.com",
                    FullName = "مسعود صبوری",
                    Password = "db901737c41e490dec8bded913f112e5e7c720c3847558f0e5c65128bdb1b34c",
                    UserType = "admin",
                    IsActive = true
                },

                new User()
                {
                    Id = 4,
                    Email = "d@b.com",
                    FullName = "مهدی صبوری",
                    Password = "db901737c41e490dec8bded913f112e5e7c720c3847558f0e5c65128bdb1b34c",
                    UserType = "admin",
                    IsActive = false
                }
            };
        }

        public async Task AddUserAsync(User user)
        {
            await Task.Run(() =>
            {
                users.Add(user);
            });
        }

        public async Task<bool> CreateCookieAsync(UserLoginViewModel user, string role)
        {
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, user.Email),
                new Claim(ClaimTypes.Role, role),
            };
            var claimIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties()
            {
                IsPersistent = user.RememberMe,
                ExpiresUtc = DateTimeOffset.Now.AddMinutes(43200)
            };

            await _httpContextAccessor.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimIdentity), authProperties);

            return true;
        }

        public async Task DeleteUserAsync(User user)
        {
            await Task.Run(() => { users.Remove(user); });
        }

        public async Task DeleteUserByIdAsync(int id)
        {
            var user = await GetUserByIdAsync(id);
            await DeleteUserAsync(user);
        }

        public async Task EditUserAsync(User user)
        {
            await DeleteUserAsync(user);
            await AddUserAsync(user);
        }

        public async Task<string> GetHashAsync(string password)
        {
            StringBuilder sb = new StringBuilder();

            await Task.Run(() =>
            {
                SHA256 sha256 = new SHA256Managed();
                byte[] result = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));

                foreach (var t in result)
                {
                    sb.Append(t.ToString("x2"));
                }

                return sb.ToString();
            });

            return sb.ToString();
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            var user = users.FirstOrDefault(a => a.Email.Equals(email));
            await Task.Run(() => { return user; });
            return user;
        }

        public async Task<User> GetUserByIdAsync(int id)
        {
            var user = users.FirstOrDefault(a => a.Id.Equals(id));
            await Task.Run(() => { return user; });
            return user;
        }

        public async Task<User> RegisterUserAsync(UserRegisterViewModel user)
        {
            var newUser = new User
            {
                Password = await GetHashAsync(user.Password),
                FullName = user.FullName,
                Email = user.Email,
                UserType = "user",
                IsActive = false
            };

            users.Add(newUser);
            return newUser;
        }

        public async Task<bool> SaveChangesAsync()
        {
            await Task.Run(() => { return true; });
            return true;
        }

        public async Task<User> GetUserByActivationCode(string activationCode)
        {
            var user = users
                .FirstOrDefault(a => 
                    a.ActivationCode == activationCode);
            await Task.Run(() => { return user; });
            return user;
        }

        public async Task<User> SignInUserAsync(UserLoginViewModel user)
        {
            var password = await GetHashAsync(user.Password);
            return users.FirstOrDefault(a => a.Email.Equals(user.Email) && a.Password.Equals(password));
        }

        public async Task SignOutUserAsync()
        {
            await Task.Run(() =>
            {
                _httpContextAccessor.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            });

        }

        public async Task<User> GetUserByResetPasswordCode(string resetPasswordCode)
        {
            var user = users
                .FirstOrDefault(a =>
                    a.ResetPasswordCode == resetPasswordCode);
            await Task.Run(() => { return user; });
            return user;
        }
    }
}
