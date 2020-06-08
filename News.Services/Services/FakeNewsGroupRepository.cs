using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using News.Models.DomainModels;
using News.Models.ViewModels;
using News.Services.Repositories;

namespace News.Services.Services
{
    public class FakeNewsGroupRepository : INewsGroupRepository
    {
        private List<NewsGroup> newsGroups;
        public FakeNewsGroupRepository()
        {
            newsGroups = new List<NewsGroup>();

            newsGroups.Add(new NewsGroup()
            {
                Id = 1,
                GroupTitle = "سیاسی"
            });

            newsGroups.Add(new NewsGroup()
            {
                Id = 2,
                GroupTitle = "اجتماعی"
            });

            newsGroups.Add(new NewsGroup()
            {
                Id = 3,
                GroupTitle = "اقتصادی"
            });
        }
        public async Task<List<NewsGroup>> GetAllNewsGroupsAsync()
        {
            await Task.Run(() => { return newsGroups; });
            return newsGroups;
        }

        public async Task<NewsGroup> GetNewsGroupByIdAsync(int groupId)
        {
            var newsGroup = newsGroups.FirstOrDefault(a => a.Id.Equals(groupId));
            await Task.Run(() => { return newsGroup; });
            return newsGroup;
        }

        public async Task InsertNewsGroupAsync(NewsGroup newsGroup)
        {
            await Task.Run(() => { newsGroups.Add(newsGroup); });
        }

        public async Task UpdateNewsGroupAsync(NewsGroup newsGroup)
        {
            await DeleteNewsGroupAsync(newsGroup);
            await InsertNewsGroupAsync(newsGroup);
        }

        public async Task DeleteNewsGroupAsync(NewsGroup newsGroup)
        {
            await Task.Run(() => newsGroups.Remove(newsGroup));
        }

        public async Task DeleteNewsGroupAsync(int groupId)
        {
            var newsGroup = await GetNewsGroupByIdAsync(groupId);
            await DeleteNewsGroupAsync(newsGroup);
        }

        public async Task<bool> NewsGroupExistsAsync(int newsGroupId)
        {
            var newsGroupExist = newsGroups.Any(a => a.Id.Equals(newsGroupId));
            await Task.Run(() => { return newsGroupExist; });
            return newsGroupExist;
        }

        public async Task<List<ShowGroupsViewModel>> GetListGroupsAsync()
        {
            var newsGroupList = newsGroups.Select(g => new ShowGroupsViewModel()
            {
                GroupId = g.Id,
                GroupTitle = g.GroupTitle,
                NewsCount = 3
            }).ToList();
            await Task.Run(() => { return newsGroupList; });
            return newsGroupList;
        }

        public async Task SaveAsync()
        {
            await Task.Run(() => { Thread.Sleep(1); });
        }
    }
}
