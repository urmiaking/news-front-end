using News.Models.DomainModels;
using News.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace News.Services.Repositories
{
    public interface INewsGroupRepository
    {
        Task<List<NewsGroup>> GetAllNewsGroupsAsync();

        Task<NewsGroup> GetNewsGroupByIdAsync(int groupId);

        Task<NewsGroup> GetNewsGroupByNameAsync(string groupName);

        Task InsertNewsGroupAsync(NewsGroup newsGroup);

        Task UpdateNewsGroupAsync(NewsGroup newsGroup);

        Task<bool> DeleteNewsGroupAsync(NewsGroup newsGroup);

        Task<bool> DeleteNewsGroupAsync(int groupId);

        Task<List<ShowGroupsViewModel>> GetListGroupsAsync();
    }
}
