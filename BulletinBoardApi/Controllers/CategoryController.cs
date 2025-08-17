using BulletinBoardApi.Data;
using BulletinBoardApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace BulletinBoardApi.Controllers
{

    /// <summary>
    /// Provides endpoints to retrieve <see cref="Category"/> entities.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController(ICategoryRepository _repository, ILogger<CategoryController> _logger) : ControllerBase
    {
        /// <summary>
        /// Retrieves all categories.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken ct)
        {
            var categories = await _repository.GetAllCategoriesAsync(ct);
            _logger.LogInformation("Retrieved {Count} categories", categories.Count());
            return Ok(categories);
        }
    }
}
