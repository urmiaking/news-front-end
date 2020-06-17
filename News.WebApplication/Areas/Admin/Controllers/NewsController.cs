using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using News.Models.MetaModels;
using News.Models.ViewModels;
using News.Services.Repositories;

namespace News.WebApplication.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "admin")]
    public class NewsController : Controller
    {
        private readonly INewsRepository _newsRepository;
        private readonly INewsGroupRepository _newsGroupRepository;

        public NewsController(INewsRepository newsRepository, INewsGroupRepository newsGroupRepository)
        {
            _newsRepository = newsRepository;
            _newsGroupRepository = newsGroupRepository;
        }

        public async Task<IActionResult> NewsList(int? page)
        {
            var allNews = await _newsRepository.GetAllNewsAsync();
            List<NewsListViewModel> newsModel = new List<NewsListViewModel>();
            foreach (var news in allNews)
            {
                var newsList = new NewsListViewModel()
                {
                    Id = news.Id,
                    CreateDate = news.CreateDate,
                    ImageName = news.ImageName,
                    Title = news.Title,
                    VisitCount = news.VisitCount,
                    NewsGroup = news.NewsGroup
                };
                newsModel.Add(newsList);
            }

            int pageSize = 5;
            int pageNumber = (page ?? 1);

            return View(PaginatedList<NewsListViewModel>.Create(newsModel, pageNumber, pageSize));
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Models.DomainModels.News news, IFormFile imageFile, string newsGroupName)
        {
            if (!ModelState.IsValid)
            {
                return View(news);
            }

            if (imageFile == null)
            {
                ModelState.AddModelError("ImageName", "عکسی برای خبر آپلود کنید");
                return View(news);
            }

            if (imageFile.Length > 500000)
            {
                ModelState.AddModelError("ImageName", "حجم عکس بیش از ۵۰۰ کیلوبایت می باشد");
                return View(news);
            }

            var newsGroup = await _newsGroupRepository.GetNewsGroupByNameAsync(newsGroupName);

            if (newsGroup == null)
            {
                ModelState.AddModelError("NewsGroup.GroupTitle", "گروه خبری موجود نمی باشد");
                return View(news);
            }

            news.CreateDate = DateTime.Now;
            news.ImageName = Guid.NewGuid() + Path.GetExtension(imageFile.FileName);
            news.VisitCount = 0;
            news.NewsGroupId = newsGroup.Id;
            news.NewsGroup = newsGroup;

            try
            {
                string savePath = Path.Combine(
                    Directory.GetCurrentDirectory(), "wwwroot/img/news-images",
                    news.ImageName
                );
                await using var stream = new FileStream(savePath, FileMode.Create);
                await imageFile.CopyToAsync(stream);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            await _newsRepository.InsertNewsAsync(news);

            TempData["Success"] = "خبر با موفقیت افزوده شد";
            return RedirectToAction("NewsList", "News");
        }
    }
}