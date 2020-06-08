using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using News.Models.DomainModels;
using News.Models.ViewModels;
using News.Services.Repositories;

namespace News.WebApplication.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserRepository _userRepository;

        public AccountController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
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

            TempData["Success"] = "ثبت نام شما موفقیت آمیز بود! لطفا قبل از ورود به سیستم، ایمیل خود را تایید کنید";
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(UserLoginViewModel user)
        {
            if (!ModelState.IsValid)
            {
                return View(user);
            }

            var result = await _userRepository.SignInUserAsync(user);
            if (result == null)
            {
                TempData["Error"] = "ایمیل یا رمز عبور اشتباه است";
                return View(user);
            }

            await _userRepository.CreateCookieAsync(user);
            TempData["Success"] = $"کاربر محترم، {result.FullName} ورود شما موفقیت آمیز بود";
            return RedirectToAction("Index", "Home");
        }

        public IActionResult ForgetPassword()
        {
            return View();
        }

        public IActionResult ResetPassword()
        {
            return View();
        }

        public async Task<IActionResult> LogOut()
        {
            await _userRepository.SignOutUserAsync();
            TempData["Success"] = $"خروج موفقیت آمیز بود";
            return RedirectToAction("Index", "Home");
        }
    }
}