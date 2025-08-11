using BulletinBoardApi.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace BulletinBoardApi.Data
{
    /// <summary>
    /// Repository implementation for managing <see cref="Announcement"/> entities
    /// via stored procedures in the SQL database.
    /// </summary>
    public class AnnouncementRepository(IConfiguration config) : IAnnouncementRepository
    {
        private readonly string connectionString = config.GetConnectionString("DefaultConnection");

        /// <summary>
        /// Inserts a new announcement into the database.
        /// </summary>
        public async Task CreateAnnouncementAsync(Announcement announcement, CancellationToken ct)
        {
            try
            {
                using var connection = new SqlConnection(connectionString);
                using var command = new SqlCommand("AddAnnouncement", connection) //dbo.
                {
                    CommandType = CommandType.StoredProcedure
                };
                command.Parameters.Add(new SqlParameter("@Title", SqlDbType.NVarChar, 200) { Value = announcement.Title });
                command.Parameters.Add(new SqlParameter("@Description", SqlDbType.NVarChar, -1) { Value = (object?)announcement.Description ?? DBNull.Value });
                command.Parameters.Add(new SqlParameter("@Status", SqlDbType.Bit) { Value = announcement.Status });
                command.Parameters.Add(new SqlParameter("@CategoryId", SqlDbType.Int) { Value = announcement.CategoryId });
                command.Parameters.Add(new SqlParameter("@SubCategoryId", SqlDbType.Int) { Value = announcement.SubCategoryId });
                await connection.OpenAsync(ct);
                var scalar = await command.ExecuteScalarAsync(ct);
                announcement.Id = Convert.ToInt32(scalar);
            }
            catch (SqlException ex)
            {
                throw new StoredProcException(ex.Number, $"Error when creating announcement (Code={ex.Number}): {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Removes an existing announcement by its identifier.
        /// </summary>
        public async Task DeleteAnnouncementAsync(int id, CancellationToken ct)
        {
            try
            {
                using var connection = new SqlConnection(connectionString);
                using var command = new SqlCommand("DeleteAnnouncementById", connection) //dbo.
                {
                    CommandType = CommandType.StoredProcedure
                };
                command.Parameters.Add(new SqlParameter("@Id", SqlDbType.Int) { Value = id });
                await connection.OpenAsync(ct);
                var rows = await command.ExecuteNonQueryAsync(ct);
                if (rows == 0)
                {
                    throw new EntityNotFoundException($"Announcement with ID {id} not found.");
                }

            }
            catch (SqlException ex)
            {
                throw new StoredProcException(ex.Number, $"Error when deleting announcement (Code={ex.Number}): {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Retrieves a single announcement by its identifier.
        /// </summary>
        public async Task<Announcement?> GetAnnouncementByIdAsync(int id, CancellationToken ct)
        {
            try
            {
                using var connection = new SqlConnection(connectionString);
                using var command = new SqlCommand("GetAnnouncementById", connection) //dbo.
                {
                    CommandType = CommandType.StoredProcedure
                };
                command.Parameters.Add(new SqlParameter("@Id", SqlDbType.Int) { Value = id });
                await connection.OpenAsync(ct);

                using var reader = await command.ExecuteReaderAsync(ct);
                if (!await reader.ReadAsync(ct))
                    return null;

                Announcement announcement = new Announcement()
                {
                    Id = reader.GetInt32(reader.GetOrdinal(nameof(announcement.Id))),
                    Title = reader.GetString(reader.GetOrdinal(nameof(announcement.Title))),
                    Description = reader.GetString(reader.GetOrdinal(nameof(announcement.Description))),
                    CreatedDate = reader.GetDateTime(reader.GetOrdinal(nameof(announcement.CreatedDate))),
                    Status = reader.GetBoolean(reader.GetOrdinal(nameof(announcement.Status))),
                    CategoryId = reader.GetInt32(reader.GetOrdinal(nameof(announcement.CategoryId))),
                    SubCategoryId = reader.GetInt32(reader.GetOrdinal(nameof(announcement.SubCategoryId))),
                    CategoryName = reader.GetString(reader.GetOrdinal(nameof(announcement.SubCategoryName))),
                    SubCategoryName = reader.GetString(reader.GetOrdinal(nameof(announcement.SubCategoryName)))
                };

                return announcement;

            }
            catch (SqlException ex)
            {
                throw new StoredProcException(ex.Number, $"Error when getting announcement by id (Code={ex.Number}): {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Retrieves all announcements from the database.
        /// </summary>
        public async Task<IEnumerable<Announcement>> GetAnnouncementsAsync(CancellationToken ct)
        {
            try
            {
                var announcements = new List<Announcement>();
                using var connection = new SqlConnection(connectionString);
                using var command = new SqlCommand("GetAllAnnouncements", connection) //dbo.
                {
                    CommandType = CommandType.StoredProcedure
                };
                await connection.OpenAsync(ct);
                using var reader = await command.ExecuteReaderAsync(ct);
                Announcement announcement = new Announcement();
                while (await reader.ReadAsync(ct))
                {
                    announcements.Add(new Announcement()
                    {
                        Id = reader.GetInt32(reader.GetOrdinal(nameof(announcement.Id))),
                        Title = reader.GetString(reader.GetOrdinal(nameof(announcement.Title))),
                        Description = reader.GetString(reader.GetOrdinal(nameof(announcement.Description))),
                        CreatedDate = reader.GetDateTime(reader.GetOrdinal(nameof(announcement.CreatedDate))),
                        Status = reader.GetBoolean(reader.GetOrdinal(nameof(announcement.Status))),
                        CategoryId = reader.GetInt32(reader.GetOrdinal(nameof(announcement.CategoryId))),
                        SubCategoryId = reader.GetInt32(reader.GetOrdinal(nameof(announcement.SubCategoryId))),
                        CategoryName = reader.GetString(reader.GetOrdinal(nameof(announcement.SubCategoryName))).Trim(),
                        SubCategoryName = reader.GetString(reader.GetOrdinal(nameof(announcement.SubCategoryName))).Trim()
                    });
                }
                return announcements;
            }
            catch (SqlException ex)
            {
                throw new StoredProcException(ex.Number, $"Error when getting announcements (Code={ex.Number}): {ex.Message}", ex);
            }

        }

        /// <summary>
        /// Updates an existing announcement in the database.
        /// </summary>
        public async Task UpdateAnnouncementAsync(Announcement announcement, CancellationToken ct)
        {
            try
            {
                using var connection = new SqlConnection(connectionString);
                using var command = new SqlCommand("UpdateAnnouncement", connection) //dbo.
                {
                    CommandType = CommandType.StoredProcedure
                };
                command.Parameters.Add(new SqlParameter("@Id", SqlDbType.Int) { Value = announcement.Id });
                command.Parameters.Add(new SqlParameter("@Title", SqlDbType.NVarChar, 200) { Value = announcement.Title });
                command.Parameters.Add(new SqlParameter("@Description", SqlDbType.NVarChar, -1) { Value = (object?)announcement.Description ?? DBNull.Value });
                command.Parameters.Add(new SqlParameter("@Status", SqlDbType.Bit) { Value = announcement.Status });
                command.Parameters.Add(new SqlParameter("@CategoryId", SqlDbType.Int) { Value = announcement.CategoryId });
                command.Parameters.Add(new SqlParameter("@SubCategoryId", SqlDbType.Int) { Value = announcement.SubCategoryId });
                await connection.OpenAsync(ct);
                var rows = await command.ExecuteNonQueryAsync(ct);
                if (rows == 0)
                {
                    throw new EntityNotFoundException($"Announcement with ID {announcement.Id} not found.");
                }

            }
            catch (SqlException ex)
            {
                throw new StoredProcException(ex.Number, $"Error when updating announcement (Code={ex.Number}): {ex.Message}", ex);
            }
        }
    }
}
