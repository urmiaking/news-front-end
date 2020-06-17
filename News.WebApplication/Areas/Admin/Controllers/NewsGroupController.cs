using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using News.Models.DomainModels;
using News.Services.Repositories;

namespace News.WebApplication.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "admin")]
    public class NewsGroupController : Controller
    {
        private readonly INewsGroupRepository _newsGroupRepository;

        public NewsGroupController(INewsGroupRepository newsGroupRepository)
        {
            _newsGroupRepository = newsGroupRepository;
        }

        public async Task<IActionResult> NewsGroupList()
        {
            var newsGroups = await _newsGroupRepository.GetAllNewsGroupsAsync();
            return View(newsGroups);
        }

        #region Create

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(NewsGroup newsGroup)
        {
            if (!ModelState.IsValid)
            {
                return View(newsGroup);
            }

            await _newsGroupRepository.InsertNewsGroupAsync(newsGroup);

            TempData["Success"] = $"دسته بندی {newsGroup.GroupTitle} با موفقیت افزوده شد";
            return RedirectToAction("NewsGroupList");
        }

        #endregion

        #region Edit

        public async Task<IActionResult> Edit(int id = 0)
        {
            if (id == 0)
            {
                return NotFound();
            }

            var newsGroup = await _newsGroupRepository.GetNewsGroupByIdAsync(id);

            if (newsGroup == null)
            {
                return BadRequest();
            }

            return View(newsGroup);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(NewsGroup newsGroup)
        {
            if (!ModelState.IsValid)
            {
                return View(newsGroup);
            }

            await _newsGroupRepository.UpdateNewsGroupAsync(newsGroup);
            TempData["Success"] = "دسته بندی خبر با موفقیت ویرایش یافت";
            return RedirectToAction("NewsGroupList");
        }

        #endregion

        #region Delete

        [HttpPost]
        public async Task<IActionResult> Delete(int id = 0)
        {
            if (id == 0)
            {
                return StatusCode(404);
            }

            var isSucceeded = await _newsGroupRepository.DeleteNewsGroupAsync(id);

            if (!isSucceeded)
            {
                return StatusCode(403);
            }

            return StatusCode(200);
        }

        #endregion

    }
}