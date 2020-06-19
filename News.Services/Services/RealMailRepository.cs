using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using News.Models.DomainModels;
using News.Models.MetaModels;
using News.Services.Repositories;
using Newtonsoft.Json;

namespace News.Services.Services
{
    public class RealMailRepository : IMailRepository
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUserRepository _userRepository;
        private readonly int port;
        private readonly string server;
        public IConfiguration Configuration { get; }

        public RealMailRepository(IHttpContextAccessor httpContextAccessor, IUserRepository userRepository, IConfiguration configuration)
        {
            _httpContextAccessor = httpContextAccessor;
            _userRepository = userRepository;
            Configuration = configuration;
            port = int.Parse(Configuration["MailMS.Port"]);
            server = Configuration["MailMS.Server"];
        }

        public async Task<bool> SendEmail(Mail mail)
        {
            string apiResponse;
            using (var client = new HttpClient())
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(mail), Encoding.UTF8, "application/json");
                using (var response = await client.PostAsync($"http://{server}:{port}/api/Mail/SendMail", content))
                {
                    apiResponse = await response.Content.ReadAsStringAsync();
                }
            }

            if (!string.IsNullOrEmpty(apiResponse))
            {
                return false;
            }

            return true;
        }

        public async Task<bool> SendActivationLinkAsync(string userEmail)
        {
            string activationCode = Guid.NewGuid().ToString();

            var link = string.Concat(
                _httpContextAccessor.HttpContext.Request.Scheme,
                "://",
                _httpContextAccessor.HttpContext.Request.Host.ToUriComponent(),
                $"/Account/ActivateAccount/{activationCode}");

            var user = await _userRepository.GetUserByEmailAsync(userEmail);
            if (user == null)
            {
                return false;
            }

            user.ActivationCode = activationCode;
            await _userRepository.EditUserAsync(user);

            const string subject = "فعال سازی حساب کاربری | سایت خبری خاص";
            var body = "سلام " + user.FullName + ", <br/> لطفا برای فعالسازی حساب کاربری خود در وب سایت خبری خاص، روی لینک زیر کلیک کنید. " +

                       " <br/><br/><a href='" + link + "'>" + link + "</a> <br/><br/>" +
                       "اگر شما در این وبسایت حسابی باز نکردید لطفا این پیغام را نادیده بگیرید.<br/><br/> با تشکر";

            var mail = new Mail()
            {
                Body = body,
                Subject = subject,
                To = user.Email
            };

            var isSucceed = await SendEmail(mail);

            return isSucceed;
        }

        public async Task<bool> SendResetPasswordLinkAsync(string userEmail)
        {
            var user = await _userRepository.GetUserByEmailAsync(userEmail);
            if (user == null)
            {
                return false;
            }
            string resetCode = Guid.NewGuid().ToString();
            var link = string.Concat(
                _httpContextAccessor.HttpContext.Request.Scheme,
                "://",
                _httpContextAccessor.HttpContext.Request.Host.ToUriComponent(),
                $"/Account/ResetPassword/{resetCode}");

            user.ResetPasswordCode = resetCode;

            const string subject = "درخواست فراموشی رمز عبور | سایت خبری خاص";
            var body = "سلام " + user.FullName + ", <br/> شما اخیرا درخواست تغییر رمز عبور خود را در صفحه ورود به وبسایت خبری خاص نموده اید. لطفا روی لینک زیر کلیک کنید و رمز جدید خود را وارد نمایید. " +

                       " <br/><br/><a href='" + link + "'>" + link + "</a> <br/><br/>" +
                       "اگر شما درخواست تغییر رمز عبور را نداده اید لطفا این ایمیل را نادیده بگیرید<br/><br/> با تشکر";

            var mail = new Mail()
            {
                Body = body,
                Subject = subject,
                To = user.Email
            };


            await SendEmail(mail);
            return true;
        }
    }
}
