using BulletinBoardApi.Models;

namespace BulletinBoardApi.Data
{
    public interface IAnnouncementRepository
    {
        public Task<IEnumerable<Announcement>> GetAnnouncementsAsync(CancellationToken ct);
        public Task<Announcement?> GetAnnouncementByIdAsync(int id, CancellationToken ct);
        public Task CreateAnnouncementAsync(Announcement announcement, CancellationToken ct);
        public Task UpdateAnnouncementAsync(Announcement announcement, CancellationToken ct);
        public Task DeleteAnnouncementAsync(int id, CancellationToken ct);

    }
}
