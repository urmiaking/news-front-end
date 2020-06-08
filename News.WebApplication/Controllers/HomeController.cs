using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using News.Models.ViewModels;
using News.Services.Repositories;

namespace News.WebApplication.Controllers
{
    public class HomeController : Controller
    {
        private readonly INewsRepository _newsRepository;

        public HomeController(INewsRepository newsRepository)
        {
            _newsRepository = newsRepository;
        }
        public async Task<IActionResult> Index()
        {
            var indexViewModel = new IndexNewsViewModel(await _newsRepository.GetSliderNewsAsync(), await _newsRepository.GetLatestNewsAsync());
            return View(indexViewModel);
        }
    }
}
