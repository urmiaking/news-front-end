using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using News.Models.DomainModels;
using News.Services.Repositories;

namespace News.Services.Services
{
    public class FakeNewsRepository : INewsRepository
    {
        private List<Models.DomainModels.News> news;
        private List<NewsGroup> newsGroups;

        public FakeNewsRepository()
        {
            news = new List<Models.DomainModels.News>();
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

            news.Add(new Models.DomainModels.News()
            {
                Id = 1,
                Description = "لورم ایپسوم متن ساختگی با تولید سادگی نامفهوم از صنعت چاپ، و با استفاده از طراحان گرافیک است، چاپگرها و متون بلکه روزنامه و مجله در ستون و سطرآنچنان که لازم است، و برای شرایط فعلی تکنولوژی مورد نیاز، و کاربردهای متنوع با هدف بهبود ابزارهای کاربردی می باشد، کتابهای زیادی در شصت و سه درصد گذشته حال و آینده، شناخت فراوان جامعه و متخصصان را می طلبد، تا با نرم افزارها شناخت بیشتری را برای طراحان رایانه ای علی الخصوص طراحان خلاقی، و فرهنگ پیشرو در زبان فارسی ایجاد کرد، در این صورت می توان امید داشت که تمام و دشواری موجود در ارائه راهکارها، و شرایط سخت تایپ به پایان رسد و زمان مورد نیاز شامل حروفچینی دستاوردهای اصلی، و جوابگوی سوالات پیوسته اهل دنیای موجود طراحی اساسا مورد استفاده قرار گیرد.",
                Title = "عنوان خبر تست ۱",
                CreateDate = DateTime.Now.AddMinutes(5),
                ShortDescription = "ایپسوم متن ساختگی با تولید سادگی نامفهوم از صنعت چاپ، و با استفاده از طراحان گرافیک است، چاپگرها و متون بلکه روزنامه و مجله در ستون و سطرآنچنان که لازم است، و برای شرایط فعلی تکنولوژی مورد نیاز",
                Tags = "تست۱,تست۲,تست۳",
                VisitCount = 10,
                ImageName = "thumb.png",
                ShowInSlider = false,
                NewsGroupId = 1,
                NewsGroup = newsGroups.FirstOrDefault(a => a.Id.Equals(1))
            });

            news.Add(new Models.DomainModels.News()
            {
                Id = 2,
                Description = "لورم ایپسوم متن ساختگی با تولید سادگی نامفهوم از صنعت چاپ، و با استفاده از طراحان گرافیک است، چاپگرها و متون بلکه روزنامه و مجله در ستون و سطرآنچنان که لازم است، و برای شرایط فعلی تکنولوژی مورد نیاز، و کاربردهای متنوع با هدف بهبود ابزارهای کاربردی می باشد، کتابهای زیادی در شصت و سه درصد گذشته حال و آینده، شناخت فراوان جامعه و متخصصان را می طلبد، تا با نرم افزارها شناخت بیشتری را برای طراحان رایانه ای علی الخصوص طراحان خلاقی، و فرهنگ پیشرو در زبان فارسی ایجاد کرد، در این صورت می توان امید داشت که تمام و دشواری موجود در ارائه راهکارها، و شرایط سخت تایپ به پایان رسد و زمان مورد نیاز شامل حروفچینی دستاوردهای اصلی، و جوابگوی سوالات پیوسته اهل دنیای موجود طراحی اساسا مورد استفاده قرار گیرد.",
                Title = "عنوان خبر تست ۲",
                CreateDate = DateTime.Now.AddDays(4),
                ShortDescription = "ایپسوم متن ساختگی با تولید سادگی نامفهوم از صنعت چاپ، و با استفاده از طراحان گرافیک است، چاپگرها و متون بلکه روزنامه و مجله در ستون و سطرآنچنان که لازم است، و برای شرایط فعلی تکنولوژی مورد نیاز",
                Tags = "تست۱,تست۲,تست۳",
                VisitCount = 10,
                ImageName = "thumb.png",
                ShowInSlider = true,
                NewsGroupId = 1,
                NewsGroup = newsGroups.FirstOrDefault(a => a.Id.Equals(1))
            });

            news.Add(new Models.DomainModels.News()
            {
                Id = 3,
                Description = "لورم ایپسوم متن ساختگی با تولید سادگی نامفهوم از صنعت چاپ، و با استفاده از طراحان گرافیک است، چاپگرها و متون بلکه روزنامه و مجله در ستون و سطرآنچنان که لازم است، و برای شرایط فعلی تکنولوژی مورد نیاز، و کاربردهای متنوع با هدف بهبود ابزارهای کاربردی می باشد، کتابهای زیادی در شصت و سه درصد گذشته حال و آینده، شناخت فراوان جامعه و متخصصان را می طلبد، تا با نرم افزارها شناخت بیشتری را برای طراحان رایانه ای علی الخصوص طراحان خلاقی، و فرهنگ پیشرو در زبان فارسی ایجاد کرد، در این صورت می توان امید داشت که تمام و دشواری موجود در ارائه راهکارها، و شرایط سخت تایپ به پایان رسد و زمان مورد نیاز شامل حروفچینی دستاوردهای اصلی، و جوابگوی سوالات پیوسته اهل دنیای موجود طراحی اساسا مورد استفاده قرار گیرد.",
                Title = "عنوان خبر تست ۳",
                CreateDate = DateTime.Now.AddDays(2),
                ShortDescription = "ایپسوم متن ساختگی با تولید سادگی نامفهوم از صنعت چاپ، و با استفاده از طراحان گرافیک است، چاپگرها و متون بلکه روزنامه و مجله در ستون و سطرآنچنان که لازم است، و برای شرایط فعلی تکنولوژی مورد نیاز",
                Tags = "تست۱,تست۲,تست۳",
                VisitCount = 10,
                ImageName = "thumb.png",
                ShowInSlider = true,
                NewsGroupId = 2,
                NewsGroup = newsGroups.FirstOrDefault(a => a.Id.Equals(2))
            });

            news.Add(new Models.DomainModels.News()
            {
                Id = 4,
                Description = "لورم ایپسوم متن ساختگی با تولید سادگی نامفهوم از صنعت چاپ، و با استفاده از طراحان گرافیک است، چاپگرها و متون بلکه روزنامه و مجله در ستون و سطرآنچنان که لازم است، و برای شرایط فعلی تکنولوژی مورد نیاز، و کاربردهای متنوع با هدف بهبود ابزارهای کاربردی می باشد، کتابهای زیادی در شصت و سه درصد گذشته حال و آینده، شناخت فراوان جامعه و متخصصان را می طلبد، تا با نرم افزارها شناخت بیشتری را برای طراحان رایانه ای علی الخصوص طراحان خلاقی، و فرهنگ پیشرو در زبان فارسی ایجاد کرد، در این صورت می توان امید داشت که تمام و دشواری موجود در ارائه راهکارها، و شرایط سخت تایپ به پایان رسد و زمان مورد نیاز شامل حروفچینی دستاوردهای اصلی، و جوابگوی سوالات پیوسته اهل دنیای موجود طراحی اساسا مورد استفاده قرار گیرد.",
                Title = "عنوان خبر تست ۴",
                CreateDate = DateTime.Now,
                ShortDescription = "ایپسوم متن ساختگی با تولید سادگی نامفهوم از صنعت چاپ، و با استفاده از طراحان گرافیک است، چاپگرها و متون بلکه روزنامه و مجله در ستون و سطرآنچنان که لازم است، و برای شرایط فعلی تکنولوژی مورد نیاز",
                Tags = "تست۱,تست۲,تست۳",
                VisitCount = 10,
                ImageName = "thumb.png",
                ShowInSlider = true,
                NewsGroupId = 3,
                NewsGroup = newsGroups.FirstOrDefault(a => a.Id.Equals(3))
            });

            news.Add(new Models.DomainModels.News()
            {
                Id = 5,
                Description = "لورم ایپسوم متن ساختگی با تولید سادگی نامفهوم از صنعت چاپ، و با استفاده از طراحان گرافیک است، چاپگرها و متون بلکه روزنامه و مجله در ستون و سطرآنچنان که لازم است، و برای شرایط فعلی تکنولوژی مورد نیاز، و کاربردهای متنوع با هدف بهبود ابزارهای کاربردی می باشد، کتابهای زیادی در شصت و سه درصد گذشته حال و آینده، شناخت فراوان جامعه و متخصصان را می طلبد، تا با نرم افزارها شناخت بیشتری را برای طراحان رایانه ای علی الخصوص طراحان خلاقی، و فرهنگ پیشرو در زبان فارسی ایجاد کرد، در این صورت می توان امید داشت که تمام و دشواری موجود در ارائه راهکارها، و شرایط سخت تایپ به پایان رسد و زمان مورد نیاز شامل حروفچینی دستاوردهای اصلی، و جوابگوی سوالات پیوسته اهل دنیای موجود طراحی اساسا مورد استفاده قرار گیرد.",
                Title = "عنوان خبر تست ۵",
                CreateDate = DateTime.Now.AddHours(1),
                ShortDescription = "ایپسوم متن ساختگی با تولید سادگی نامفهوم از صنعت چاپ، و با استفاده از طراحان گرافیک است، چاپگرها و متون بلکه روزنامه و مجله در ستون و سطرآنچنان که لازم است، و برای شرایط فعلی تکنولوژی مورد نیاز",
                Tags = "تست۱,تست۲,تست۳",
                VisitCount = 10,
                ImageName = "thumb.png",
                ShowInSlider = false,
                NewsGroupId = 3,
                NewsGroup = newsGroups.FirstOrDefault(a => a.Id.Equals(3))
            });

            news.Add(new Models.DomainModels.News()
            {
                Id = 6,
                Description = "لورم ایپسوم متن ساختگی با تولید سادگی نامفهوم از صنعت چاپ، و با استفاده از طراحان گرافیک است، چاپگرها و متون بلکه روزنامه و مجله در ستون و سطرآنچنان که لازم است، و برای شرایط فعلی تکنولوژی مورد نیاز، و کاربردهای متنوع با هدف بهبود ابزارهای کاربردی می باشد، کتابهای زیادی در شصت و سه درصد گذشته حال و آینده، شناخت فراوان جامعه و متخصصان را می طلبد، تا با نرم افزارها شناخت بیشتری را برای طراحان رایانه ای علی الخصوص طراحان خلاقی، و فرهنگ پیشرو در زبان فارسی ایجاد کرد، در این صورت می توان امید داشت که تمام و دشواری موجود در ارائه راهکارها، و شرایط سخت تایپ به پایان رسد و زمان مورد نیاز شامل حروفچینی دستاوردهای اصلی، و جوابگوی سوالات پیوسته اهل دنیای موجود طراحی اساسا مورد استفاده قرار گیرد.",
                Title = "عنوان خبر تست ۶",
                CreateDate = DateTime.Now.AddHours(2),
                ShortDescription = "ایپسوم متن ساختگی با تولید سادگی نامفهوم از صنعت چاپ، و با استفاده از طراحان گرافیک است، چاپگرها و متون بلکه روزنامه و مجله در ستون و سطرآنچنان که لازم است، و برای شرایط فعلی تکنولوژی مورد نیاز",
                Tags = "تست۱,تست۲,تست۳",
                VisitCount = 10,
                ImageName = "thumb.png",
                ShowInSlider = false,
                NewsGroupId = 3,
                NewsGroup = newsGroups.FirstOrDefault(a => a.Id.Equals(3))
            });
        }


        public async Task<IEnumerable<Models.DomainModels.News>> GetAllNewsAsync()
        {
            await Task.Run(() => { return news.OrderByDescending(a => a.CreateDate); });
            return news.OrderByDescending(a => a.CreateDate); ;
        }

        public async Task<IEnumerable<Models.DomainModels.News>> GetTopNewsAsync(int take = 4)
        {
            var topNews = news.OrderByDescending(a => a.VisitCount).Take(take).ToList();
            await Task.Run(() => { return topNews; });
            return topNews;
        }

        public async Task<IEnumerable<Models.DomainModels.News>> GetLatestNewsAsync(int take = 3)
        {
            var latestNews = news.OrderByDescending(a => a.CreateDate).Take(take).ToList();
            await Task.Run(() => { return latestNews; });
            return latestNews;
        }

        public async Task<IEnumerable<Models.DomainModels.News>> GetSliderNewsAsync()
        {
            var sliderNews = news.Where(a => a.ShowInSlider).ToList();
            await Task.Run(() => { return sliderNews; });
            return sliderNews;
        }

        public async Task<IEnumerable<Models.DomainModels.News>> GetNewsByGroupIdAsync(int groupId)
        {
            var newsInGroup = news.Where(a => a.NewsGroupId.Equals(groupId)).OrderByDescending(a => a.CreateDate).ToList();
            await Task.Run(() => { return newsInGroup; });
            return newsInGroup;
        }

        public async Task<IEnumerable<Models.DomainModels.News>> SearchAsync(string q)
        {
            var list = news.Where(p =>
                p.Title.Contains(q) || p.ShortDescription.Contains(q) || p.Description.Contains(q) ||
                p.Tags.Contains(q)).ToList().Distinct().ToList();
            await Task.Run(() => { return list; });
            return list;
        }

        public async Task<Models.DomainModels.News> GetNewsByIdAsync(int newsId)
        {
            var newsItem = news.FirstOrDefault(a => a.Id.Equals(newsId));
            await Task.Run(() => { return newsItem; });
            return newsItem;
        }

        public async Task InsertNewsAsync(Models.DomainModels.News addedNews)
        {
            await Task.Run(() => { news.Add(addedNews); });
        }

        public async Task UpdateNewsAsync(Models.DomainModels.News incomingNews)
        {
            await DeleteNewsAsync(incomingNews.Id);
            await InsertNewsAsync(incomingNews);
        }

        public async Task DeleteNewsAsync(Models.DomainModels.News incomingNews)
        {
            bool result;
            await Task.Run(() =>
            {
                result = news.Remove(incomingNews);
                return result;
            });
        }

        public async Task DeleteNewsAsync(int newsId)
        {
            var deletedNews = await GetNewsByIdAsync(newsId);
            await DeleteNewsAsync(deletedNews);
        }

        public async Task<bool> NewsExistsAsync(int newsId)
        {
            var newsExist = news.Any(a => a.Id.Equals(newsId));
            await Task.Run(() => { return newsExist; });
            return newsExist;
        }
    }
}
