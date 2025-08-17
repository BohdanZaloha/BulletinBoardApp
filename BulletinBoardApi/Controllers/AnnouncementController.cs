using BulletinBoardApi.Data;
using BulletinBoardApi.Models;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace BulletinBoardApi.Controllers
{
    /// <summary>
    /// Provides CRUD operations for <see cref="Announcement"/> entities.
    /// Exposes endpoints to create, read, update, and delete announcements.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class AnnouncementController(IAnnouncementRepository _repository, ILogger<AnnouncementController> _logger) : ControllerBase
    {
        /// <summary>
        /// Creates a new announcement.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Announcement announcement, CancellationToken ct)
        {
            await _repository.CreateAnnouncementAsync(announcement, ct);
            _logger.LogInformation("Created announcement {AnnouncementId} with title {Title}", announcement.Id, announcement.Title);
            return CreatedAtAction(nameof(GetById), new { id = announcement.Id }, announcement);
        }

        /// <summary>
        /// Retrieves all announcements.
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Announcement>>> GetAll(CancellationToken ct)
        {
            var announcements = await _repository.GetAnnouncementsAsync(ct);
            _logger.LogInformation("Retrieved {Count} announcements", announcements.Count());
            return Ok(announcements);
        }

        /// <summary>
        /// Retrieves a specific announcement by its identifier.
        /// </summary>
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Announcement>> GetById(int id, CancellationToken ct)
        {
            var announcement = await _repository.GetAnnouncementByIdAsync(id, ct);
            if (announcement == null)
            {
                _logger.LogWarning("Announcement with ID {AnnouncementId} not found", id);
                return NotFound($"Announcement with ID {id} not found.");
            }
            _logger.LogInformation("Retrieved announcement {AnnouncementId}", id);
            return Ok(announcement);

        }

        /// <summary>
        /// Updates an existing announcement.
        /// </summary>
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] Announcement announcement, CancellationToken ct)
        {
            if (id != announcement.Id)
            {
                _logger.LogWarning("Update failed: route ID {RouteId} did not match payload ID {PayloadId}", id, announcement.Id);
                return BadRequest("ID in URL and payload do not match.");
            }
            await _repository.UpdateAnnouncementAsync(announcement, ct);
            _logger.LogInformation("Updated announcement {AnnouncementId}", id);
            return NoContent();
        }

        /// <summary>
        /// Deletes an announcement by its identifier.
        /// </summary>
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id, CancellationToken ct)
        {
            var announcement = await _repository.GetAnnouncementByIdAsync(id, ct);
            if (announcement == null)
            {
                _logger.LogWarning("Delete failed: announcement {AnnouncementId} not found", id);
                return NotFound($"Announcement with ID {id} not found.");
            }
            await _repository.DeleteAnnouncementAsync(id, ct);
            _logger.LogInformation("Deleted announcement {AnnouncementId}", id);
            return NoContent();

        }
    }
}
