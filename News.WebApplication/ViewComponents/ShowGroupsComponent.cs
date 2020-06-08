using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using News.Services.Repositories;

namespace News.WebApplication.ViewComponents
{
    public class ShowGroupsComponent : ViewComponent
    {
        private INewsGroupRepository _newsGroupRepository;

        public ShowGroupsComponent(INewsGroupRepository newsGroupRepository)
        {
            _newsGroupRepository = newsGroupRepository;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            return await Task.FromResult((IViewComponentResult) View("ShowGroupsComponent",
                await _newsGroupRepository.GetListGroupsAsync()));
        }
    }
}
