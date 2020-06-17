using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using News.Models.ViewModels;
using News.Models.MetaModels;
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
            var news = await _newsRepository.GetNewsByIdAsync(newsId);
            if (news == null)
            {
                return NotFound();
            }
            news.VisitCount += 1;
            await _newsRepository.UpdateNewsAsync(news);
            return View(news);
        }

        [Route("Group/{groupId}/{title}")]
        public async Task<IActionResult> ShowNewsByGroupId(int groupId, string title, int? page)
        {
            var news = await _newsRepository.GetNewsByGroupIdAsync(groupId);

            int pageSize = 5;
            int pageNumber = (page ?? 1);

            ViewData["GroupTitle"] = title;

            return View(PaginatedList<Models.DomainModels.News>.Create(news, pageNumber, pageSize));
        }

        [Route("Search")]
        public async Task<IActionResult> Search(string q)
        {
            var result = new SearchViewModel()
            {
                Query = q,
                Result = await _newsRepository.SearchAsync(q)
            };
            return View(result);
        }

        public async Task<IActionResult> Archive(int? page)
        {
            var news = await _newsRepository.GetAllNewsAsync();

            int pageSize = 5;
            int pageNumber = (page ?? 1);

            return View(PaginatedList<Models.DomainModels.News>.Create(news, pageNumber, pageSize));
        }
    }
}