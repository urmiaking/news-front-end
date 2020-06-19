using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using News.Models.DomainModels;
using News.Services.Repositories;

namespace News.WebApplication.Controllers
{
    public class CommentController : Controller
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IUserRepository _userRepository;
        private readonly INewsRepository _newsRepository;

        public CommentController(ICommentRepository commentRepository, IUserRepository userRepository, INewsRepository newsRepository)
        {
            _commentRepository = commentRepository;
            _userRepository = userRepository;
            _newsRepository = newsRepository;
        }

        [HttpPost]
        public async Task<IActionResult> AddComment(string content, string userEmail, int newsId = 0)
        {
            if (newsId == 0)
            {
                return BadRequest();
            }

            var news = await _newsRepository.GetNewsByIdAsync(newsId);

            if (news == null)
            {
                return BadRequest();
            }

            var user = await _userRepository.GetUserByEmailAsync(userEmail);

            if (user == null)
            {
                return StatusCode(403);
            }

            var comment = new Comment()
            {
                Content = content,
                DateTime = DateTime.Now,
                IsConfirmed = true,
                NewsId = newsId,
                UserId = user.Id
            };
            await _commentRepository.AddCommentForNewsAsync(comment, user, news);

            TempData["Success"] = "نظر شما با موفقیت افزوده شد";

            return RedirectToAction("ShowNews", "News", new {newsId});
        }
    }
}