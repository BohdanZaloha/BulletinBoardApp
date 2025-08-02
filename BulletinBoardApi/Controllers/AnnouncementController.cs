using BulletinBoardApi.Data;
using BulletinBoardApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace BulletinBoardApi.Controllers
{
    /// <summary>
    /// Provides CRUD operations for <see cref="Announcement"/> entities.
    /// Exposes endpoints to create, read, update, and delete announcements.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class AnnouncementController(IAnnouncementRepository _repository) : ControllerBase
    {
        /// <summary>
        /// Creates a new announcement.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Announcement announcement, CancellationToken ct)
        {
            await _repository.CreateAnnouncementAsync(announcement, ct);
            return CreatedAtAction(nameof(GetById), new { id = announcement.Id }, announcement);
        }

        /// <summary>
        /// Retrieves all announcements.
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Announcement>>> GetAll(CancellationToken ct)
        {
            var announcements = await _repository.GetAnnouncementsAsync(ct);
            return Ok(announcements);
        }

        /// <summary>
        /// Retrieves a specific announcement by its identifier.
        /// </summary>
        [HttpGet("GetAnnouncementById/{id:int}")]
        public async Task<ActionResult<Announcement>> GetById(int id, CancellationToken ct)
        {
            var announcement = await _repository.GetAnnouncementByIdAsync(id, ct);
            if (announcement == null)
            {
                return NotFound($"Announcement with ID {id} not found.");
            }
            return Ok(announcement);

        }

        /// <summary>
        /// Updates an existing announcement.
        /// </summary>
        [HttpPut("UpdateAnnouncement/{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] Announcement announcement, CancellationToken ct)
        {
            if (id != announcement.Id)
            {
                return BadRequest("ID in URL and payload do not match.");
            }
            await _repository.UpdateAnnouncementAsync(announcement, ct);
            return NoContent();
        }

        /// <summary>
        /// Deletes an announcement by its identifier.
        /// </summary>
        [HttpDelete("DeleteAnnouncement/{id:int}")]
        public async Task<IActionResult> Delete(int id, CancellationToken ct)
        {
            var announcement = await _repository.GetAnnouncementByIdAsync(id, ct );
            if (announcement == null)
            {
                return NotFound($"Announcement with ID {id} not found.");
            }
            await _repository.DeleteAnnouncementAsync(id, ct);
            return NoContent();

        }
    }
}
