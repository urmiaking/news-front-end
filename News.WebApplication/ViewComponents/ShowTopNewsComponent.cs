using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using News.Services.Repositories;

namespace News.WebApplication.ViewComponents
{
    public class ShowTopNewsComponent : ViewComponent
    {
        private INewsRepository _newsRepository;

        public ShowTopNewsComponent(INewsRepository newsRepository)
        {
            _newsRepository = newsRepository;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            return await Task.FromResult((IViewComponentResult) View("ShowTopNewsComponent",
                await _newsRepository.GetTopNews()));
        }
    }
}
