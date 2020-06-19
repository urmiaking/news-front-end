using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using News.Models.DomainModels;
using News.Services.Repositories;
using Newtonsoft.Json;

namespace News.Services.Services
{
    public class RealNewsRepository : INewsRepository
    {
        public IConfiguration Configuration { get; set; }
        private readonly int port;
        private readonly string server;

        public RealNewsRepository(IConfiguration configuration)
        {
            Configuration = configuration;
            port = int.Parse(Configuration["NewsMS.Port"]);
            server = Configuration["NewsMS.Server"];
        }

        public async Task<IEnumerable<Models.DomainModels.News>> GetAllNewsAsync()
        {
            var newsList = new List<Models.DomainModels.News>();
            using (var client = new HttpClient())
            {
                using (var response = await client.GetAsync($"http://{server}:{port}/api/News/GetNews"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    newsList = JsonConvert.DeserializeObject<List<Models.DomainModels.News>>(apiResponse);
                }
            }

            return newsList;
        }

        public async Task<Models.DomainModels.News> GetNewsByIdAsync(int newsId)
        {
            var newsGroup = new Models.DomainModels.News();
            using (var client = new HttpClient())
            {
                using (var response = await client.GetAsync($"http://{server}:{port}/api/News/GetNewsById/{newsId}"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    newsGroup = JsonConvert.DeserializeObject<Models.DomainModels.News>(apiResponse);
                }
            }

            return newsGroup;
        }

        public async Task<IEnumerable<Models.DomainModels.News>> GetSliderNewsAsync()
        {
            var sliderNews = await GetAllNewsAsync();
            if (sliderNews != null)
            {
                sliderNews = sliderNews.Where(a => a.ShowInSlider).ToList();
                return sliderNews;
            }

            return new List<Models.DomainModels.News>();
        }

        public async Task<IEnumerable<Models.DomainModels.News>> GetTopNewsAsync(int take = 4)
        {
            var topNews = await GetAllNewsAsync();
            if (topNews != null)
            {
                topNews = topNews.OrderByDescending(a => a.VisitCount).Take(take).ToList();
                return topNews;
            }

            return new List<Models.DomainModels.News>();
        }

        public async Task<IEnumerable<Models.DomainModels.News>> GetLatestNewsAsync(int take = 3)
        {
            var latestNews = await GetAllNewsAsync();
            if (latestNews != null)
            {
                latestNews = latestNews.OrderByDescending(a => a.CreateDate).Take(take).ToList();
                return latestNews;
            }

            return new List<Models.DomainModels.News>();
        }

        public async Task<IEnumerable<Models.DomainModels.News>> GetNewsByGroupIdAsync(int groupId)
        {
            var news = await GetAllNewsAsync();
            if (news != null)
            {
                news = news.Where(a => a.NewsGroupId.Equals(groupId)).OrderByDescending(a => a.CreateDate).ToList();
                return news;
            }

            return new List<Models.DomainModels.News>();
        }

        public async Task<IEnumerable<Models.DomainModels.News>> SearchAsync(string q)
        {
            var news = await GetAllNewsAsync();
            if (news != null)
            {
                news = news.Where(p =>
                    p.Title.Contains(q) || p.ShortDescription.Contains(q) || p.Description.Contains(q) ||
                    p.Tags.Contains(q)).ToList().Distinct().ToList();
                return news;
            }

            return new List<Models.DomainModels.News>();
        }

        public async Task InsertNewsAsync(Models.DomainModels.News news)
        {
            var result = new Models.DomainModels.News();
            using (var client = new HttpClient())
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(news), Encoding.UTF8, "application/json");
                using (var response = await client.PostAsync($"http://{server}:{port}/api/News/PostNews", content))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    result = JsonConvert.DeserializeObject<Models.DomainModels.News>(apiResponse);
                }
            }
        }

        public async Task<bool> NewsExistsAsync(int newsId)
        {
            var result = await GetNewsByIdAsync(newsId);
            if (result == null)
            {
                return false;
            }

            return true;
        }

        public async Task UpdateNewsAsync(Models.DomainModels.News news)
        {
            using (var httpClient = new HttpClient())
            {
                StringContent newContent = new StringContent(JsonConvert.SerializeObject(news), Encoding.UTF8, "application/json");

                using (var response = await httpClient.PutAsync($"http://{server}:{port}/api/News/PutNews/{news.Id}", newContent))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                }
            }
        }

        public async Task DeleteNewsAsync(Models.DomainModels.News news)
        {
            var result = new Models.DomainModels.News();
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.DeleteAsync($"http://{server}:{port}/api/News/DeleteNews/{news.Id}"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    result = JsonConvert.DeserializeObject<Models.DomainModels.News>(apiResponse);
                }
            }
        }

        public async Task DeleteNewsAsync(int newsId)
        {
            var result = new Models.DomainModels.News();
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.DeleteAsync($"http://{server}:{port}/api/News/DeleteNews/{newsId}"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    result = JsonConvert.DeserializeObject<Models.DomainModels.News>(apiResponse);
                }
            }
        }
    }
}
