using BulletinBoardApi.Models;

namespace BulletinBoardApi.Data
{
    public interface ISubCategoryRepository
    {
        Task<IEnumerable<SubCategory>> GetAllSubCategoriesAsync(CancellationToken ct);
        Task<IEnumerable<SubCategory>> GetSubCategoriesByCategoryIdAsync(int categoryId, CancellationToken ct);
    }
}
