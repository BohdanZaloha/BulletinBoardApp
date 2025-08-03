using BulletinBoardWeb.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace BulletinBoardWeb.Services
{
    public class AnnouncementsService : IAnnouncementsService
    {
        private readonly HttpClient _httpClient;

        public AnnouncementsService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task CreateAnnouncementAsync(Announcement announcement)
        {
            var response = await _httpClient.PostAsJsonAsync("api/Announcement", announcement);
            if (!response.IsSuccessStatusCode)
            {
                var problemDetails = await response.Content.ReadFromJsonAsync<ProblemDetails>();
                throw new ServiceException(problemDetails?.Detail ?? "Error creating announcement.", response.StatusCode);
            }
        }

        public async Task<List<Announcement>> GetAnnouncementsAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<Announcement>>("api/Announcement") ?? new List<Announcement>();
        }

        public async Task<HttpResponseMessage> UpdateAnnouncementAsync(int announcementId, Announcement announcement)
        {
            var response = await _httpClient.PutAsJsonAsync($"api/Announcement/UpdateAnnouncement/{announcementId}", announcement);
            if (!response.IsSuccessStatusCode)
            {
                var problemDetails = await response.Content.ReadFromJsonAsync<ProblemDetails>();
                throw new ServiceException(problemDetails?.Detail ?? "Error updating announcement.", response.StatusCode);
            }
            return response;
        }
        public async Task<HttpResponseMessage> DeleteAnnouncementAsync(int announcementId)
        {
           var response = await _httpClient.DeleteAsync($"api/Announcement/DeleteAnnouncement/{announcementId}").ConfigureAwait(false);
            if(!response.IsSuccessStatusCode)
            {
                var problemDetails = await response.Content.ReadFromJsonAsync<ProblemDetails>();
                throw new ServiceException(problemDetails?.Detail ?? "Error when deleting announcement.", response.StatusCode);
            }
            return response;
        }

        public async Task<Announcement> GetAnnouncementsByIdAsync(int announcementId)
        {
            var response = await _httpClient.GetAsync($"api/Announcement/GetAnnouncementById/{announcementId}");

            if (!response.IsSuccessStatusCode)
            {
                var problemDetails = await response.Content.ReadFromJsonAsync<ProblemDetails>();
                throw new ServiceException(problemDetails?.Detail ?? $"Error loading announcement {announcementId}.",response.StatusCode);
            }

            var announcement = await response.Content.ReadFromJsonAsync<Announcement>();
            if (announcement is null)
            {
                throw new ServiceException($"Announcement {announcementId} was not found in the response.",HttpStatusCode.NoContent);
            }

            return announcement;
        }

        public async Task<List<Category>> GetCategoriesAsync()
        {
            var response = await _httpClient.GetAsync("api/Category/GetCategories");

            if (!response.IsSuccessStatusCode)
            {
                var problemDetails = await response.Content.ReadFromJsonAsync<ProblemDetails>();
                throw new ServiceException(problemDetails?.Detail ?? "Error loading categories.",response.StatusCode);
            }

            var list = await response.Content.ReadFromJsonAsync<List<Category>>();

            return list ?? new List<Category>();
        }

        public async Task<List<SubCategory>> GetSubCategoriesByCategoryAsync(int categoryId)
        {
            var response = await _httpClient.GetAsync($"api/SubCategory/GetSubCategoriesByCategoryId/{categoryId}");

            if (!response.IsSuccessStatusCode)
            {
                var problemDetails = await response.Content.ReadFromJsonAsync<ProblemDetails>();
                throw new ServiceException(problemDetails?.Detail ?? $"Error loading subcategories for category {categoryId}.",response.StatusCode);
            }

            var list = await response.Content.ReadFromJsonAsync<List<SubCategory>>();
            return list ?? new List<SubCategory>();
        }


    }
    
}
