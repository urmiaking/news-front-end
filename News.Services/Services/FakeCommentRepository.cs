using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using News.Models.DomainModels;
using News.Services.Repositories;

namespace News.Services.Services
{
    public class FakeCommentRepository : ICommentRepository
    {
        private List<Comment> comments;
        private readonly INewsRepository _newsRepository;

        public FakeCommentRepository(INewsRepository newsRepository)
        {
            _newsRepository = newsRepository;
            comments = new List<Comment>();
        }

        public async Task AddCommentForNewsAsync(Comment comment, User user, News.Models.DomainModels.News news)
        {
            comment.User = user;
            comment.News = news;
            comments.Add(comment);

            var dbNews = await _newsRepository.GetNewsByIdAsync(news.Id);
            if (dbNews.Comments == null)
            {
                dbNews.Comments = new List<Comment>();
            }
            dbNews.Comments.Add(comment);
            await _newsRepository.UpdateNewsAsync(dbNews);
        }
    }
}
