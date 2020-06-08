using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using News.Models.ViewModels;
using News.Services.Repositories;

namespace News.WebApplication.Controllers
{
    public class NewsController : Controller
    {
        private readonly INewsRepository _newsRepository;

        public NewsController(INewsRepository newsRepository)
        {
            _newsRepository = newsRepository;
        }

        [Route("News/{newsId}")]
        public async Task<IActionResult> ShowNews(int newsId)
        {
            var news = await _newsRepository.GetNewsById(newsId);
            if (news == null)
            {
                return NotFound();
            }
            news.VisitCount += 1;
            await _newsRepository.UpdateNews(news);
            await _newsRepository.Save();
            return View(news);
        }

        [Route("Group/{groupId}/{title}")]
        public async Task<IActionResult> ShowNewsByGroupId(int groupId, string title)
        {
            ViewData["GroupTitle"] = title;
            return View(await _newsRepository.GetNewsByGroupId(groupId));
        }

        [Route("Search")]
        public async Task<IActionResult> Search(string q)
        {
            var result = new SearchViewModel()
            {
                Query = q,
                Result = await _newsRepository.Search(q)
            };
            return View(result);
        }
    }
}