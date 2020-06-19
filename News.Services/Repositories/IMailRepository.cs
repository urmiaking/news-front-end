using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using News.Models.MetaModels;

namespace News.Services.Repositories
{
    public interface IMailRepository
    {
        Task<bool> SendEmail(Mail mail);

        Task<bool> SendActivationLinkAsync(string userEmail);

        Task<bool> SendResetPasswordLinkAsync(string userEmail);
    }
}
