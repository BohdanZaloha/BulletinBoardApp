using BulletinBoardApi.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BulletinBoardApi.Controllers
{
    /// <summary>
    /// Provides endpoints to retrieve <see cref="Category"/> entities.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class SubCategoryController(ISubCategoryRepository repository) : ControllerBase
    {
        /// <summary>
        /// Retrieves all SubCategories.
        /// </summary>
        [HttpGet("GetSubCategories")]
        public async Task<IActionResult> GetAllSubCategories(CancellationToken ct)
        {
            var subCategories = await repository.GetAllSubCategoriesAsync(ct);
            return Ok(subCategories);
        }

        /// <summary>
        /// Retrieves SubCategory by Id.
        /// </summary>
        [HttpGet("GetSubCategoriesByCategoryId/{id:int}")]
        public async Task<IActionResult> GetByCategory(int id, CancellationToken ct)
        {
            var subCategories = await repository.GetSubCategoriesByCategoryIdAsync(id, ct);
            return Ok(subCategories);
        }
    }
}
