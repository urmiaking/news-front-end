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
        public async Task<List<NewsGroup>> GetAllNewsGroups()
        {
            await Task.Run(() => { return newsGroups; });
            return newsGroups;
        }

        public async Task<NewsGroup> GetNewsGroupById(int groupId)
        {
            var newsGroup = newsGroups.FirstOrDefault(a => a.Id.Equals(groupId));
            await Task.Run(() => { return newsGroup; });
            return newsGroup;
        }

        public async Task InsertNewsGroup(NewsGroup newsGroup)
        {
            await Task.Run(() => { newsGroups.Add(newsGroup); });
        }

        public async Task UpdateNewsGroup(NewsGroup newsGroup)
        {
            await DeleteNewsGroup(newsGroup);
            await InsertNewsGroup(newsGroup);
        }

        public async Task DeleteNewsGroup(NewsGroup newsGroup)
        {
            await Task.Run(() => newsGroups.Remove(newsGroup));
        }

        public async Task DeleteNewsGroup(int groupId)
        {
            var newsGroup = await GetNewsGroupById(groupId);
            await DeleteNewsGroup(newsGroup);
        }

        public async Task<bool> NewsGroupExists(int newsGroupId)
        {
            var newsGroupExist = newsGroups.Any(a => a.Id.Equals(newsGroupId));
            await Task.Run(() => { return newsGroupExist; });
            return newsGroupExist;
        }

        public async Task<List<ShowGroupsViewModel>> GetListGroups()
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

        public async Task Save()
        {
            await Task.Run(() => { Thread.Sleep(1); });
        }
    }
}
