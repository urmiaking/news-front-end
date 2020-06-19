using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using News.Models.DomainModels;
using News.Services.Repositories;
using Newtonsoft.Json;

namespace News.Services.Services
{
    public class RealCommentRepository : ICommentRepository
    {
        private IConfiguration Configuration { get; set; }
        private readonly int port;
        private readonly string server;

        public RealCommentRepository(IConfiguration configuration)
        {
            Configuration = configuration;
            port = int.Parse(Configuration["NewsMS.Port"]);
            server = Configuration["NewsMS.Server"];
        }
        public async Task AddCommentForNewsAsync(Comment comment, User user, Models.DomainModels.News news)
        {
            using (var client = new HttpClient())
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(comment), Encoding.UTF8, "application/json");
                using (var response = await client.PostAsync($"http://{server}:{port}/api/Comments/PostComment", content))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                }
            }
        }

        public async Task<IEnumerable<Comment>> GetAllCommentsByNewsIdAsync(int newsId)
        {
            var newsGroups = new List<Comment>();
            using (var client = new HttpClient())
            {
                using (var response = await client.GetAsync($"http://{server}:{port}/api/Comments/GetCommentByNewsId/{newsId}"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    newsGroups = JsonConvert.DeserializeObject<List<Comment>>(apiResponse);
                }
            }

            return newsGroups;
        }
    }
}
