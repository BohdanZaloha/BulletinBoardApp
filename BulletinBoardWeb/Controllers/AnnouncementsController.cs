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
        private readonly AnnouncementsService _announcementsService;

        public AnnouncementsController(AnnouncementsService announcementsService)
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
            List<SubCategory> allSubCategories;
            if (categoryId.HasValue && categoryId.Value > 0)
            {
                allSubCategories = await _announcementsService
                    .GetSubCategoriesByCategoryAsync(categoryId.Value);
            }
            else
            {
                allSubCategories = new List<SubCategory>();
                // allSubCategories = await _announcementsService.GetAllSubCategoriesAsync();
            }

            ViewBag.CategoryList = new SelectList(allCategories, "CategoryId", "Name", categoryId ?? 0);
            ViewBag.SubCategoryList = new SelectList(allSubCategories, "SubCategoryId", "Name", subCategoryId ?? 0);
        }

        /// <summary>
        /// Displays a list of announcements, optionally filtered by category and/or subcategory.
        /// </summary>
        public async Task<IActionResult> Index(int? categoryId, int? subCategoryId)
        {
            List<Announcement> announcements = await _announcementsService.GetAnnouncementsAsync();
           

            if (categoryId.HasValue && categoryId.Value > 0)
            {
                announcements = announcements
                    .Where(a => a.CategoryId == categoryId.Value)
                    .ToList();
            }

            if (subCategoryId.HasValue && subCategoryId.Value > 0)
            {
                announcements = announcements
                    .Where(a => a.SubCategoryId == subCategoryId.Value)
                    .ToList();
            }

            await LoadCategoryData(categoryId, subCategoryId);

            return View(announcements);
        }

        /// <summary>
        /// Shows the form for creating a new announcement, optionally preselecting a category.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Create(int? categoryId)
        {
            if (!ModelState.IsValid)
            {
                await LoadCategoryData(categoryId);
                return BadRequest(ModelState);
            }

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

            await _announcementsService.CreateAnnouncementAsync(announcement);

            return RedirectToAction(nameof(Index));
        }
        

        /// <summary>
        /// Displays the details of a specific announcement.
        /// </summary>
        public async Task<IActionResult> Details(int Id)
        {
            Announcement announcementDetails = await _announcementsService.GetAnnouncementsByIdAsync(Id);

            return View(announcementDetails);
        }

        /// <summary>
        /// Updates an existing announcement via the API.
        /// </summary>
        public async Task<IActionResult> UpdateAnnouncementDetails(int Id, Announcement announcement)
        {
            if (!ModelState.IsValid)
            {
                return View(announcement);
            }
                

            var response = await _announcementsService.UpdateAnnouncementAsync(Id, announcement);
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    return View("ErrorPage");
                }
            
        }

        /// <summary>
        /// Displays the confirmation view for deleting an announcement.
        /// </summary>
        public async Task<IActionResult> Delete(int Id)
        {
            Announcement announcementDetails = await _announcementsService.GetAnnouncementsByIdAsync(Id);

            await LoadCategoryData(announcementDetails.CategoryId, announcementDetails.SubCategoryId);
            return View(announcementDetails);
        }

        /// <summary>
        /// Deletes an announcement via the API.
        /// </summary>
        public async Task<IActionResult> DeleteAnnouncement(int Id)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction(nameof(Delete), new { Id });
            }

            var response = await _announcementsService.DeleteAnnouncementAsync(Id);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            else
            {
                return View("ErrorPage");
            }
        }
        public IActionResult ErrorPage()
        {
            return View();
        }
    }
}
