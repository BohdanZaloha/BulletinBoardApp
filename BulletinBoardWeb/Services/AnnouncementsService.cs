using BulletinBoardWeb.Models;

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
            response.EnsureSuccessStatusCode();
        }



        public async Task<List<Announcement>> GetAnnouncementsAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<Announcement>>("api/Announcement") ?? new List<Announcement>();
        }

        public async Task<HttpResponseMessage> UpdateAnnouncementAsync(int announcementId, Announcement announcement)
        {
            return await _httpClient.PutAsJsonAsync($"api/Announcement/UpdateAnnouncement/{announcementId}", announcement).ConfigureAwait(false);
        }
        public async Task<HttpResponseMessage> DeleteAnnouncementAsync(int announcementId)
        {
            return await _httpClient.DeleteAsync($"api/Announcement/DeleteAnnouncement/{announcementId}").ConfigureAwait(false);
        }

        public async Task<Announcement> GetAnnouncementsByIdAsync(int announcementId)
        {
            return await _httpClient.GetFromJsonAsync<Announcement>($"api/Announcement/GetAnnouncementById/{announcementId}") ?? new Announcement();
        }

        public async Task<List<Category>> GetCategoriesAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<Category>>("api/Category/GetCategories") ?? new List<Category>();
        }

        public async Task<List<SubCategory>> GetSubCategoriesByCategoryAsync(int categoryid)
        {
            return await _httpClient.GetFromJsonAsync<List<SubCategory>>($"api/SubCategory/GetSubCategoriesByCategoryId/{categoryid}") ?? new List<SubCategory>();
        }

       
    }
    
}
