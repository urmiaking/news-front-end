using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using News.Models.DomainModels;
using News.Models.ViewModels;

namespace News.Services.Repositories
{
    public interface IUserRepository
    {
        Task<string> GetHashAsync(string password);

        Task<User> SignInUserAsync(UserLoginViewModel user);

        Task<bool> SendActivationLinkAsync(string userEmail);

        Task<bool> SendResetPasswordLinkAsync(string userEmail);

        Task<bool> CreateCookieAsync(UserLoginViewModel user);

        Task<User> RegisterUserAsync(UserRegisterViewModel user);

        Task<User> GetUserByIdAsync(int id);

        Task<User> GetUserByEmailAsync(string email);

        Task DeleteUserAsync(User user);
        
        Task DeleteUserByIdAsync(int id);

        Task EditUserAsync(User user);

        Task AddUserAsync(User user);

        Task SignOutUserAsync();

    }
}
