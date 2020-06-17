using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using News.Models.DomainModels;
using News.Models.ViewModels;
using News.Services.Repositories;

namespace News.WebApplication.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly IMailRepository _mailRepository;

        public AccountController(IUserRepository userRepository, IMailRepository mailRepository)
        {
            _userRepository = userRepository;
            _mailRepository = mailRepository;
        }

        #region Register

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(UserRegisterViewModel user)
        {
            if (!ModelState.IsValid)
            {
                return View(user);
            }

            var result = await _userRepository.RegisterUserAsync(user);
            if (result == null)
            {
                TempData["Error"] = "مشکلی در ثبت نام پیش آمد! لطفا مجددا امتحان کنید";
                return View(user);
            }

            var emailSent = await _mailRepository.SendActivationLinkAsync(user.Email);
            if (!emailSent)
            {
                //SAGA RollBack Pattern
                var failUser = await _userRepository.GetUserByEmailAsync(user.Email);
                await _userRepository.DeleteUserAsync(failUser);
                TempData["Error"] = "مشکلی در ثبت نام و ارسال ایمیل تایید پیش آمد! لطفا مجددا امتحان کنید";
                return View(user);
            }

            TempData["Success"] = "ثبت نام شما موفقیت آمیز بود! لطفا قبل از ورود به سیستم، ایمیل خود را تایید کنید";
            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> ActivateAccount(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return NotFound();
            }

            var user = await _userRepository.GetUserByActivationCode(id);
            if (user == null)
            {
                return NotFound();
            }

            user.IsActive = true;
            user.ActivationCode = null;
            await _userRepository.EditUserAsync(user);

            var userViewModel = new UserLoginViewModel()
            {
                Email = user.Email,
                Password = user.Password,
                RememberMe = false
            };

            TempData["Success"] = "حساب شما با موفقیت فعال شد";
            await _userRepository.CreateCookieAsync(userViewModel, "user");
            return RedirectToAction("Index", "Home");
        }

        #endregion

        #region Login

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(UserLoginViewModel user)
        {
            if (!ModelState.IsValid)
            {
                return View(user);
            }

            var signedInUser = await _userRepository.SignInUserAsync(user);
            if (signedInUser == null)
            {
                TempData["Error"] = "ایمیل یا رمز عبور اشتباه است";
                return View(user);
            }

            if (signedInUser.IsActive)
            {
                await _userRepository.CreateCookieAsync(user, signedInUser.UserType);
            }
            else
            {
                TempData["Error"] = "حساب کاربری شما غیرفعال است، لطفا نسبت به فعالسازی آن از طریق لینکی که به ایمیل شما ارسال شده است اقدام کنید";
                return View(user);
            }
            TempData["Success"] = $"کاربر محترم، {signedInUser.FullName} ورود شما موفقیت آمیز بود";
            return RedirectToAction("Index", "Home");
        }

        #endregion

        #region ResetPassword

        public IActionResult ForgetPassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgetPassword(ForgetPasswordViewModel user)
        {
            if (!ModelState.IsValid)
            {
                return View(user);
            }

            var resetLinkSent = await _mailRepository.SendResetPasswordLinkAsync(user.Email);
            if (!resetLinkSent)
            {
                TempData["Error"] = "مشکلی در ارسال ایمیل بازیابی پیش آمد! لطفا از صحت آدرس ایمیل خود اطمینان حاصل کرده و مجددا امتحان کنید";
                return View(user);
            }

            TempData["Success"] = $"لینک بازیابی کلمه عبور با موفقیت به آدرس ایمیل {user.Email} ارسال شد";
            return RedirectToAction("Login", "Account");
        }

        public async Task<IActionResult> ResetPassword(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return NotFound();
            }

            var user = await _userRepository.GetUserByResetPasswordCode(id);
            if (user == null)
            {
                return NotFound();
            }

            ResetPasswordViewModel resetPasswordViewModel = new ResetPasswordViewModel()
            {
                ResetCode = id
            };

            return View(resetPasswordViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid) return View(model);
            var user = await _userRepository.GetUserByResetPasswordCode(model.ResetCode);

            if (user != null)
            {
                var hashedPassword = await _userRepository.GetHashAsync(model.NewPassword);
                user.Password = hashedPassword;
                user.ResetPasswordCode = null;
                await _userRepository.EditUserAsync(user);

                TempData["Success"] = "رمز عبور شما با موفقیت تغییر یافت";
                return RedirectToAction("Login");
            }

            TempData["Error"] = "توکن منقضی شده است";
            return View(model);
        }

        #endregion

        #region LogOut

        public async Task<IActionResult> LogOut()
        {
            await _userRepository.SignOutUserAsync();
            TempData["Success"] = $"خروج موفقیت آمیز بود";
            return RedirectToAction("Index", "Home");
        }

        #endregion

        #region EditProfile

        [Authorize]
        public async Task<IActionResult> EditProfile()
        {
            var user = await _userRepository.GetUserByEmailAsync(User.Identity.Name);
            if (user == null)
            {
                return BadRequest();
            }

            var editUserViewModel = new EditUserViewModel()
            {
                Id = user.Id,
                Email = user.Email,
                FullName = user.FullName
            };

            return View(editUserViewModel);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProfile(EditUserViewModel userModel)
        {
            if (!ModelState.IsValid)
            {
                return View(userModel);
            }

            var user = await _userRepository.GetUserByIdAsync(userModel.Id);
            if (user == null)
            {
                return BadRequest();
            }

            if (!string.IsNullOrEmpty(userModel.NewPassword))
            {
                user.Password = await _userRepository.GetHashAsync(userModel.NewPassword);
            }

            if (!userModel.Email.Equals(user.Email))
            {
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

                var userLoginViewModel = new UserLoginViewModel()
                {
                    Email = userModel.Email,
                    RememberMe = false
                };

                await _userRepository.CreateCookieAsync(userLoginViewModel, user.UserType);
            }

            user.Email = userModel.Email;
            user.FullName = userModel.FullName;

            await _userRepository.EditUserAsync(user);

            TempData["Success"] = "اطلاعات شما با موفقیت ویرایش شد";
            return View(userModel);

        }

        #endregion
    }
}