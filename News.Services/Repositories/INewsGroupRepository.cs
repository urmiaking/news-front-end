﻿using News.Models.DomainModels;
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
        Task DeleteNewsGroupAsync(NewsGroup newsGroup);
        Task DeleteNewsGroupAsync(int groupId);
        Task<bool> NewsGroupExistsAsync(int newsGroupId);
        Task<List<ShowGroupsViewModel>> GetListGroupsAsync();
    }
}
