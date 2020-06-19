using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using News.Models.DomainModels;
using News.Models.ViewModels;
using News.Services.Repositories;
using Newtonsoft.Json;

namespace News.Services.Services
{
    public class RealUserRepository : IUserRepository
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly int port;
        private readonly string server;

        public IConfiguration Configuration { get; }

        public RealUserRepository(IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
        {
            _httpContextAccessor = httpContextAccessor;
            Configuration = configuration;
            port = int.Parse(Configuration["UsersMS.Port"]);
            server = Configuration["UsersMS.Server"];
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

        public async Task<User> SignInUserAsync(UserLoginViewModel user)
        {
            var password = await GetHashAsync(user.Password);
            var dbUser = await GetUserByEmailAsync(user.Email);
            if (dbUser.Password.Equals(password))
            {
                return dbUser;
            }

            return null;
        }

        public async Task SignOutUserAsync()
        {
            await Task.Run(() =>
            {
                _httpContextAccessor.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
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

        public async Task<User> RegisterUserAsync(UserRegisterViewModel user)
        {
            var result = new User();

            var newUser = new User
            {
                Password = await GetHashAsync(user.Password),
                FullName = user.FullName,
                Email = user.Email,
                UserType = "user",
                IsActive = false
            };

            using (var client = new HttpClient())
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(newUser), Encoding.UTF8, "application/json");
                using (var response = await client.PostAsync($"http://{server}:{port}/api/Users/PostUsers", content))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    result = JsonConvert.DeserializeObject<User>(apiResponse);
                }
            }

            return result;
        }

        public async Task<User> GetUserByIdAsync(int id)
        {
            var user = new User();
            using (var client = new HttpClient())
            {
                using (var response = await client.GetAsync($"http://{server}:{port}/api/Users/GetUsers/{id}"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    user = JsonConvert.DeserializeObject<User>(apiResponse);
                }
            }

            return user;
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            var user = new User();
            using (var client = new HttpClient())
            {
                using (var response = await client.GetAsync($"http://{server}:{port}/api/Users/GetUsersByEmail/{email}"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    user = JsonConvert.DeserializeObject<User>(apiResponse);
                }
            }

            return user;
        }

        public async Task<User> GetUserByActivationCode(string activationCode)
        {
            var user = new User();
            using (var client = new HttpClient())
            {
                using (var response = await client.GetAsync($"http://{server}:{port}/api/Users/GetUsersByActivationCode/{activationCode}"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    user = JsonConvert.DeserializeObject<User>(apiResponse);
                }
            }

            if (user.Id == 0)
            {
                return null;
            }

            return user;
        }

        public async Task<User> GetUserByResetPasswordCode(string resetPasswordCode)
        {
            var user = new User();
            using (var client = new HttpClient())
            {
                using (var response = await client.GetAsync($"http://{server}:{port}/api/Users/GetUsersByResetPasswordCode/{resetPasswordCode}"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    user = JsonConvert.DeserializeObject<User>(apiResponse);
                }
            }

            return user;
        }

        public async Task DeleteUserAsync(User user)
        {
            var result = new User();
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.DeleteAsync($"http://{server}:{port}/api/Users/DeleteUsers/{user.Id}"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    result = JsonConvert.DeserializeObject<User>(apiResponse);
                }
            }
        }

        public async Task DeleteUserByIdAsync(int id)
        {
            var result = new User();
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.DeleteAsync($"http://{server}:{port}/api/Users/DeleteUsers/{id}"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    result = JsonConvert.DeserializeObject<User>(apiResponse);
                }
            }
        }

        public async Task EditUserAsync(User user)
        {
            using (var httpClient = new HttpClient())
            {
                StringContent newContent = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");

                using (var response = await httpClient.PutAsync($"http://{server}:{port}/api/Users/PutUsers/{user.Id}", newContent))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                }
            }
        }

        public async Task AddUserAsync(User user)
        {
            var result = new User();
            using (var client = new HttpClient())
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");
                using (var response = await client.PostAsync($"http://{server}:{port}/api/Users/PostUsers", content))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    result = JsonConvert.DeserializeObject<User>(apiResponse);
                }
            }
        }
    }
}
