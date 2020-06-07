using System;
using System.Collections.Generic;
using System.Text;

namespace News.Models.ViewModels
{
    public class IndexNewsViewModel
    {
        public IndexNewsViewModel(IEnumerable<DomainModels.News> sliderNews, IEnumerable<DomainModels.News> latestNews)
        {
            SliderNews = sliderNews ?? new List<DomainModels.News>();
            LatestNews = latestNews ?? new List<DomainModels.News>();
        }

        public IndexNewsViewModel()
        {
            SliderNews = new List<DomainModels.News>();
            LatestNews = new List<DomainModels.News>();
        }
        public IEnumerable<DomainModels.News> SliderNews { get; set; }
        public IEnumerable<DomainModels.News> LatestNews { get; set; }
    }
}
