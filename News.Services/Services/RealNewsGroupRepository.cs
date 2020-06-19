using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using News.Models.DomainModels;
using News.Models.ViewModels;
using News.Services.Repositories;
using Newtonsoft.Json;

namespace News.Services.Services
{
    public class RealNewsGroupRepository : INewsGroupRepository
    {
        public IConfiguration Configuration { get; set; }
        private readonly int port;
        private readonly string server;
        private INewsRepository _newsRepository;

        public RealNewsGroupRepository(IConfiguration configuration, INewsRepository newsRepository)
        {
            _newsRepository = newsRepository;
            Configuration = configuration;
            port = int.Parse(Configuration["NewsMS.Port"]);
            server = Configuration["NewsMS.Server"];
        }


        public async Task<List<NewsGroup>> GetAllNewsGroupsAsync()
        {
            var newsGroups = new List<NewsGroup>();
            using (var client = new HttpClient())
            {
                using (var response = await client.GetAsync($"http://{server}:{port}/api/NewsGroups/GetNewsGroups"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    newsGroups = JsonConvert.DeserializeObject<List<NewsGroup>> (apiResponse);
                }
            }

            return newsGroups;
        }

        public async Task<NewsGroup> GetNewsGroupByIdAsync(int groupId)
        {
            var newsGroup = new NewsGroup();
            using (var client = new HttpClient())
            {
                using (var response = await client.GetAsync($"http://{server}:{port}/api/NewsGroups/GetNewsGroupById/{groupId}"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    newsGroup = JsonConvert.DeserializeObject<NewsGroup>(apiResponse);
                }
            }

            return newsGroup;
        }

        public async Task<NewsGroup> GetNewsGroupByNameAsync(string groupName)
        {
            var newsGroup = new NewsGroup();
            using (var client = new HttpClient())
            {
                using (var response = await client.GetAsync($"http://{server}:{port}/api/NewsGroups/GetNewsGroupByName/{groupName}"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    newsGroup = JsonConvert.DeserializeObject<NewsGroup>(apiResponse);
                }
            }

            return newsGroup;
        }

        public async Task InsertNewsGroupAsync(NewsGroup newsGroup)
        {
            var result = new NewsGroup();
            using (var client = new HttpClient())
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(newsGroup), Encoding.UTF8, "application/json");
                using (var response = await client.PostAsync($"http://{server}:{port}/api/NewsGroups/PostNewsGroup", content))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    result = JsonConvert.DeserializeObject<NewsGroup>(apiResponse);
                }
            }
        }

        public async Task UpdateNewsGroupAsync(NewsGroup newsGroup)
        {
            using (var httpClient = new HttpClient())
            {
                StringContent newContent = new StringContent(JsonConvert.SerializeObject(newsGroup), Encoding.UTF8, "application/json");

                using (var response = await httpClient.PutAsync($"http://{server}:{port}/api/NewsGroups/PutNewsGroup/{newsGroup.Id}", newContent))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                }
            }
        }

        public async Task<bool> DeleteNewsGroupAsync(NewsGroup newsGroup)
        {
            var result = new NewsGroup();
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.DeleteAsync($"http://{server}:{port}/api/NewsGroups/DeleteNewsGroup/{newsGroup.Id}"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    result = JsonConvert.DeserializeObject<NewsGroup>(apiResponse);
                }
            }

            if (result.Id == 0)
            {
                return false;
            }

            return true;
        }

        public async Task<bool> DeleteNewsGroupAsync(int groupId)
        {
            var result = new NewsGroup();
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.DeleteAsync($"http://{server}:{port}/api/NewsGroups/DeleteNewsGroup/{groupId}"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    result = JsonConvert.DeserializeObject<NewsGroup>(apiResponse);
                }
            }

            if (result.Id == 0)
            {
                return false;
            }

            return true;
        }

        public async Task<List<ShowGroupsViewModel>> GetListGroupsAsync()
        {
            var newsGroups = await GetAllNewsGroupsAsync();
            
            foreach (var newsGroup in newsGroups)
            {
                var news = await _newsRepository.GetNewsByGroupIdAsync(newsGroup.Id);
                if (news != null)
                {
                    newsGroup.News = news.ToList();
                }
            }

            var newsGroupList = newsGroups.Select(g => new ShowGroupsViewModel()
            {
                GroupId = g.Id,
                GroupTitle = g.GroupTitle,
                NewsCount = g.News.Count
            }).ToList();
            await Task.Run(() => { return newsGroupList; });
            return newsGroupList;
        }
    }
}
