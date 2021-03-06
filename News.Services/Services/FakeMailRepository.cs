﻿using News.Services.Repositories;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using News.Models.MetaModels;

namespace News.Services.Services
{
    public class FakeMailRepository : IMailRepository
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private MailServer _mailServer;
        private readonly IUserRepository _userRepository;
        public FakeMailRepository(IHttpContextAccessor httpContextAccessor, IUserRepository userRepository)
        {
            _httpContextAccessor = httpContextAccessor;
            _userRepository = userRepository;

            _mailServer = new MailServer()
            {
                Id = 1,
                Email = "masoud.xpress@gmail.com",
                Host = "smtp.gmail.com",
                Password = "MASOUD7559",
                Port = 587,
                ServerType = "gmail"
            };
        }
        public async Task<bool> SendEmail(Mail mail)
        {
            var server = _mailServer;

            await Task.Run(() => { });

            if (server == null)
            {
                return false;
            }
            try
            {
                using (MailMessage mm = new MailMessage(server.Email, mail.To))
                {
                    mm.Subject = mail.Subject;
                    mm.Body = mail.Body;

                    mm.IsBodyHtml = true;
                    SmtpClient smtp = new SmtpClient();
                    smtp.Host = server.Host;
                    smtp.EnableSsl = true;

                    NetworkCredential networkCredential = new NetworkCredential(server.Email, server.Password);
                    smtp.Credentials = networkCredential;
                    smtp.Port = server.Port;
                    smtp.Send(mm);

                    return true;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
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
