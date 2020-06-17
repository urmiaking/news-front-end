using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using News.Models.DomainModels;

namespace News.Services.Repositories
{
    public interface ICommentRepository
    {
        Task AddCommentForNewsAsync(Comment comment, User user, Models.DomainModels.News news);
    }
}
