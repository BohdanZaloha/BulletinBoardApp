using BulletinBoardWeb.Models;

namespace BulletinBoardWeb.Services
{
    public interface IAnnouncementsService
    {
        Task<List<Category>> GetCategoriesAsync();
        Task<List<SubCategory>> GetSubCategoriesByCategoryAsync(int categoryId);
        
        Task<List<Announcement>> GetAnnouncementsAsync();


        Task CreateAnnouncementAsync(Announcement announcement);
        Task<Announcement> GetAnnouncementsByIdAsync(int announcementId);
        Task<HttpResponseMessage> UpdateAnnouncementAsync(int announcementId, Announcement announcement);

        Task<HttpResponseMessage> DeleteAnnouncementAsync(int announcementId);
    }

}
