using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace BulletinBoardWeb.Models
{
    public class AnnouncementEditViewModel
    {
        public int? Id { get; set; }

        [Required, StringLength(200)]
        public string Title { get; set; } = "";

        public string? Description { get; set; }

        [Required]
        public bool Status { get; set; }

        [Required]
        [Display(Name = "Category")]
        public int CategoryId { get; set; }

        [Required]
        [Display(Name = "Sub-Category")]
        public int SubCategoryId { get; set; }

        public IEnumerable<SelectListItem> CategoryList { get; set; }
            = Enumerable.Empty<SelectListItem>();
        public IEnumerable<SelectListItem> SubCategoryList { get; set; }
            = Enumerable.Empty<SelectListItem>();
    }
}
