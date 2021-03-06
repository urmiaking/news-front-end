﻿using System;
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

        public async Task<NewsGroup> GetNewsGroupByNameAsync(string groupName)
        {
            var newsGroup = newsGroups.FirstOrDefault(a => a.GroupTitle.Equals(groupName));
            await Task.Run(() => { return newsGroup; });
            return newsGroup;
        }

        public async Task InsertNewsGroupAsync(NewsGroup newsGroup)
        {
            var rnd = new Random();
            newsGroup.Id = rnd.Next();
            await Task.Run(() => { newsGroups.Add(newsGroup); });
        }

        public async Task UpdateNewsGroupAsync(NewsGroup newsGroup)
        {
            var myNewsGroup = await GetNewsGroupByIdAsync(newsGroup.Id);
            myNewsGroup.GroupTitle = newsGroup.GroupTitle;
        }

        public async Task<bool> DeleteNewsGroupAsync(NewsGroup newsGroup)
        {
            if (newsGroup.News == null)
            {
                await Task.Run(() => newsGroups.Remove(newsGroup));
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> DeleteNewsGroupAsync(int groupId)
        {
            var newsGroup = await GetNewsGroupByIdAsync(groupId);
            var result = await DeleteNewsGroupAsync(newsGroup);
            return result;

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
    }
}
