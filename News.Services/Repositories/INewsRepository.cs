using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using News.Models.DomainModels;

namespace News.Services.Repositories
{
    public interface INewsRepository
    {
        Task<IEnumerable<Models.DomainModels.News>> GetAllNews();

        Task<IEnumerable<Models.DomainModels.News>> GetTopNews(int take = 4);

        Task<IEnumerable<Models.DomainModels.News>> GetLatestNews(int take = 3);

        Task<IEnumerable<Models.DomainModels.News>> GetSliderNews();

        Task<IEnumerable<Models.DomainModels.News>> GetNewsByGroupId(int groupId);

        Task<IEnumerable<Models.DomainModels.News>> Search(string q);

        Task<Models.DomainModels.News> GetNewsById(int newsId);

        Task InsertNews(Models.DomainModels.News news);

        Task UpdateNews(Models.DomainModels.News news);

        Task DeleteNews(Models.DomainModels.News news);

        Task DeleteNews(int newsId);

        Task<bool> NewsExists(int newsId);

        Task Save();
    }
}
