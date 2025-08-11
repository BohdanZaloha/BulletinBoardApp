using BulletinBoardApi.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace BulletinBoardApi.Data
{
    /// <summary>
    /// Repository implementation for managing <see cref="Category"/> entities
    /// via stored procedures in the SQL database.
    /// </summary>
    public class CategoryRepository(IConfiguration config) : ICategoryRepository
    {
        private readonly string connectionString = config.GetConnectionString("DefaultConnection");

        /// <summary>
        /// Retrieves all categories from the database.
        /// </summary>
        async Task<IEnumerable<Category>> ICategoryRepository.GetAllCategoriesAsync(CancellationToken ct)
        {
            try
            {
                var categories = new List<Category>();
                using var connection = new SqlConnection(connectionString);
                using var command = new SqlCommand("GetAllCategories", connection) //dbo.
                {
                    CommandType = CommandType.StoredProcedure
                };
                await connection.OpenAsync(ct);
                using var reader = await command.ExecuteReaderAsync(ct);
                while (await reader.ReadAsync(ct))
                {
                    categories.Add(new Category()
                    {
                        CategoryId = (int)reader["CategoryId"],
                        Name = reader["Name"].ToString()!.Trim(),
                    });
                }
                return categories;
            }
            catch (SqlException ex)
            {
                throw new StoredProcException(ex.Number, $"Error when getting categories (Code={ex.Number}): {ex.Message}", ex);
            }
        }

    }
}
