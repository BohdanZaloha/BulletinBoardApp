using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BulletinBoardWeb.Models
{
    [Table("Announcements")]
    public class Announcement
    {
        [Key]
        public int Id { get; set; }

        [Required, StringLength(200)]
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }

        [Required]
        public bool Status { get; set; }

        [Required(ErrorMessage = "Please select a category.")]
        [Range(1, int.MaxValue, ErrorMessage = "Please select a category.")]
        public int CategoryId { get; set; }
        [Required(ErrorMessage = "Please select a sub‐category.")]
        [Range(1, int.MaxValue, ErrorMessage = "Please select a sub‐category.")]
        public int SubCategoryId { get; set; }

        public string? CategoryName { get; set; }
        public string? SubCategoryName { get; set; }

    }
}
