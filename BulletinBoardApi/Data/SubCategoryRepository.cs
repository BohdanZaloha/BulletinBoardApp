using BulletinBoardApi.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace BulletinBoardApi.Data
{
    /// <summary>
    /// Repository implementation for managing <see cref="SubCategory"/> entities
    /// via stored procedures in the SQL database.
    /// </summary>
    public class SubCategoryRepository(IConfiguration config) : ISubCategoryRepository
    {
        private readonly string connectionString = config.GetConnectionString("DefaultConnection");

        /// <summary>
        /// Retrieves all SubCategories from the database.
        /// </summary>
        async Task<IEnumerable<SubCategory>> ISubCategoryRepository.GetAllSubCategoriesAsync(CancellationToken ct)
        {
            try
            {
                var subCategories = new List<SubCategory>();
                using var connection = new SqlConnection(connectionString);
                using var command = new SqlCommand("GetAllSubCategories", connection) //dbo.
                {
                    CommandType = CommandType.StoredProcedure
                };
                await connection.OpenAsync(ct);
                using var reader = await command.ExecuteReaderAsync(ct);

                while (await reader.ReadAsync(ct))
                {
                    subCategories.Add(new SubCategory()
                    {
                        SubCategoryId = (int)reader["SubCategoryId"],
                        CategoryId = (int)reader["CategoryId"],
                        Name = reader["Name"].ToString()!.Trim(),
                    });
                }
                return subCategories;
            }
            catch (SqlException ex)
            {
                throw new StoredProcException(ex.Number, $"Error when getting subCategories (Code={ex.Number}): {ex.Message}", ex);
            }

        }

        /// <summary>
        /// Retrieves SubCategory By Id from the database.
        /// </summary>
        async Task<IEnumerable<SubCategory>> ISubCategoryRepository.GetSubCategoriesByCategoryIdAsync(int categoryId, CancellationToken ct)
        {
            try
            {
                var subCategories = new List<SubCategory>();
                using var connection = new SqlConnection(connectionString);
                using var command = new SqlCommand("GetSubCategoriesByCategoryId", connection) //dbo.
                {
                    CommandType = CommandType.StoredProcedure
                };
                command.Parameters.AddWithValue("@CategoryId", categoryId);
                await connection.OpenAsync(ct);
                using var reader = await command.ExecuteReaderAsync(ct);

                while (await reader.ReadAsync(ct))
                {
                    subCategories.Add(new SubCategory()
                    {
                        SubCategoryId = (int)reader["SubCategoryId"],
                        CategoryId = (int)reader["CategoryId"],
                        Name = reader["Name"].ToString()!.Trim(),
                    });
                }
                return subCategories;
            }
            catch (SqlException ex)
            {
                throw new StoredProcException(ex.Number, $"Error when getting subCategory by Id (Code={ex.Number}): {ex.Message}", ex);
            }

        }
    }
}
