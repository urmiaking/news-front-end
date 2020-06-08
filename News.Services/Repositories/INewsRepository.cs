using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using News.Models.DomainModels;

namespace News.Services.Repositories
{
    public interface INewsRepository
    {
        Task<IEnumerable<Models.DomainModels.News>> GetAllNewsAsync();

        Task<IEnumerable<Models.DomainModels.News>> GetTopNewsAsync(int take = 4);

        Task<IEnumerable<Models.DomainModels.News>> GetLatestNewsAsync(int take = 3);

        Task<IEnumerable<Models.DomainModels.News>> GetSliderNewsAsync();

        Task<IEnumerable<Models.DomainModels.News>> GetNewsByGroupIdAsync(int groupId);

        Task<IEnumerable<Models.DomainModels.News>> SearchAsync(string q);

        Task<Models.DomainModels.News> GetNewsByIdAsync(int newsId);

        Task InsertNewsAsync(Models.DomainModels.News news);

        Task UpdateNewsAsync(Models.DomainModels.News news);

        Task DeleteNewsAsync(Models.DomainModels.News news);

        Task DeleteNewsAsync(int newsId);

        Task<bool> NewsExistsAsync(int newsId);

        Task Save();
    }
}
