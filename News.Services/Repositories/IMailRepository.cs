using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace News.Services.Repositories
{
    public interface IMailRepository
    {
        bool SendEmail(string emailAddress, string body, string subject);

        Task<bool> SendActivationLinkAsync(string userEmail);

        Task<bool> SendResetPasswordLinkAsync(string userEmail);
    }
}
