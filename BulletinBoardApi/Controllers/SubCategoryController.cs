using BulletinBoardApi.Data;
using Microsoft.AspNetCore.Mvc;

namespace BulletinBoardApi.Controllers
{
    /// <summary>
    /// Provides endpoints to retrieve <see cref="Category"/> entities.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class SubCategoryController(ISubCategoryRepository repository, ILogger<SubCategoryController> _logger) : ControllerBase
    {
        /// <summary>
        /// Retrieves all SubCategories.
        /// </summary>
        [HttpGet()]
        public async Task<IActionResult> GetAllSubCategories(CancellationToken ct)
        {
            var subCategories = await repository.GetAllSubCategoriesAsync(ct);
            _logger.LogInformation("Retrieved {Count} subcategories", subCategories.Count());
            return Ok(subCategories);
        }

        /// <summary>
        /// Retrieves SubCategory by Id.
        /// </summary>
        [HttpGet("{categoryId:int}")]
        public async Task<IActionResult> GetByCategory(int categoryId, CancellationToken ct)
        {
            var subCategories = await repository.GetSubCategoriesByCategoryIdAsync(categoryId, ct);
            _logger.LogInformation("Retrieved {Count} subcategories by category id {Id}", subCategories.Count(), categoryId);
            return Ok(subCategories);
        }
    }
}
