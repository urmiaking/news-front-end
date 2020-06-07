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
        Task<List<NewsGroup>> GetAllNewsGroups();
        Task<NewsGroup> GetNewsGroupById(int groupId);
        Task InsertNewsGroup(NewsGroup newsGroup);
        Task UpdateNewsGroup(NewsGroup newsGroup);
        Task DeleteNewsGroup(NewsGroup newsGroup);
        Task DeleteNewsGroup(int groupId);
        Task<bool> NewsGroupExists(int newsGroupId);
        Task<List<ShowGroupsViewModel>> GetListGroups();
        Task Save();
    }
}
