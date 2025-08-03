using BulletinBoardWeb.Models;
using BulletinBoardWeb.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Reflection;

namespace BulletinBoardWeb.Controllers
{
    /// <summary>
    /// Handles CRUD operations and filtering for announcements in the web UI,
    /// communicating with the back-end API.
    /// </summary>
    public class AnnouncementsController : Controller
    {
        private readonly IAnnouncementsService _announcementsService;

        public AnnouncementsController(IAnnouncementsService announcementsService)
        {
            _announcementsService = announcementsService;
        }

        /// <summary>
        /// Loads all categories and (optionally filtered) subcategories into the <see cref="ViewBag"/>
        /// for use in cascading dropdowns.
        /// </summary>
        private async Task LoadCategoryData(int? categoryId = null, int? subCategoryId = null)
        {
            var allCategories = await _announcementsService.GetCategoriesAsync();
            var allSubCategories = categoryId.HasValue && categoryId.Value > 0 ? await _announcementsService.GetSubCategoriesByCategoryAsync(categoryId.Value) : new List<SubCategory>(); //_announcementsService.GetAllSubCategoriesAsync();

            ViewBag.CategoryList = new SelectList(allCategories, "CategoryId", "Name", categoryId ?? 0);
            ViewBag.SubCategoryList = new SelectList(allSubCategories, "SubCategoryId", "Name", subCategoryId ?? 0);
        }

        /// <summary>
        /// Displays a list of announcements, optionally filtered by category and/or subcategory.
        /// </summary>
        public async Task<IActionResult> Index(int? categoryId, int? subCategoryId)
        {
            try
            {
                List<Announcement> announcements = await _announcementsService.GetAnnouncementsAsync();
                if (categoryId > 0)
                {
                    announcements = announcements.Where(a => a.CategoryId == categoryId.Value).ToList();
                }
                if (subCategoryId > 0)
                {
                    announcements = announcements.Where(a => a.SubCategoryId == subCategoryId.Value).ToList();
                }

                await LoadCategoryData(categoryId, subCategoryId);

                return View(announcements);
            }
            catch (ServiceException ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction(nameof(ErrorPage));
            }
        }

        /// <summary>
        /// Shows the form for creating a new announcement, optionally preselecting a category.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Create(int? categoryId)
        {
            await LoadCategoryData(categoryId);
            var model = new Announcement
            {
                CategoryId = categoryId.GetValueOrDefault(),
                SubCategoryId = 0
            };
            return View(model);
        }

        /// <summary>
        /// Handles the POST of a new announcement to the API.
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateAnnouncement(Announcement announcement)
        {
            if (!ModelState.IsValid)
            {
                await LoadCategoryData(announcement.CategoryId, announcement.SubCategoryId);
                return View(nameof(Create), announcement);
            }

            try
            {
                await _announcementsService.CreateAnnouncementAsync(announcement);
                return RedirectToAction(nameof(Index));
            }
            catch (ServiceException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                await LoadCategoryData(announcement.CategoryId, announcement.SubCategoryId);
                return View(nameof(Create), announcement);
            }
        }

        /// <summary>
        /// Displays the details of a specific announcement.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Edit(int Id, int? categoryId, int? subCategoryId)
        {
            try
            {
                Announcement announcementDetails = await _announcementsService.GetAnnouncementsByIdAsync(Id);
                if (categoryId.HasValue)
                {
                    announcementDetails.CategoryId = categoryId.Value;
                }

                if (subCategoryId.HasValue)
                {
                    announcementDetails.SubCategoryId = subCategoryId.Value;
                }

                await LoadCategoryData(announcementDetails.CategoryId,announcementDetails.SubCategoryId);

                return View(announcementDetails);

            }
            catch (ServiceException ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction(nameof(ErrorPage));
            }
        }

        /// <summary>
        /// Updates an existing announcement via the API.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Edit(int id, Announcement announcement)
        {
            if (id != announcement.Id)
            {
                return BadRequest();
            }
            if (!ModelState.IsValid)
            {
                await LoadCategoryData(announcement.CategoryId, announcement.SubCategoryId);
                return View(announcement);
            }

            try
            {
                await _announcementsService.UpdateAnnouncementAsync(id, announcement);
                return RedirectToAction(nameof(Index));
            }
            catch (ServiceException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                await LoadCategoryData(announcement.CategoryId, announcement.SubCategoryId);
                return View(announcement);
            }
        }

        /// <summary>
        /// Displays the confirmation view for deleting an announcement.
        /// </summary>
        public async Task<IActionResult> Delete(int Id)
        {
            try
            {
                Announcement announcementDetails = await _announcementsService.GetAnnouncementsByIdAsync(Id);
                await LoadCategoryData(announcementDetails.CategoryId, announcementDetails.SubCategoryId);
                return View(announcementDetails);
            }
            catch (ServiceException ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction(nameof(ErrorPage));
            }

        }

        /// <summary>
        /// Deletes an announcement via the API.
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteAnnouncement(int id)
        {

            try
            {
                await _announcementsService.DeleteAnnouncementAsync(id);
                return RedirectToAction(nameof(Index));
            }
            catch (ServiceException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);

                var item = await _announcementsService.GetAnnouncementsByIdAsync(id);
                await LoadCategoryData(item.CategoryId, item.SubCategoryId);
                return View(nameof(Delete), item);
            }
        }
        public IActionResult ErrorPage()
        {
            ViewBag.ErrorMessage = TempData["ErrorMessage"] ?? "An unexpected error occurred.";
            return View();
        }
    }
}
