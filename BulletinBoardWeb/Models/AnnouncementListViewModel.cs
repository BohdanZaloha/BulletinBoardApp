using Microsoft.AspNetCore.Mvc.Rendering;

namespace BulletinBoardWeb.Models
{
    public class AnnouncementListViewModel
    {
        public IEnumerable<AnnouncementItemViewModel> Announcements { get; set; } = Enumerable.Empty<AnnouncementItemViewModel>();

        public int? SelectedCategoryId {  get; set; }
        public int? SelectedSubCategoryId { get; set; }
        public IEnumerable<SelectListItem> CategoryList { get; set; } = Enumerable.Empty<SelectListItem>();
        public IEnumerable<SelectListItem> SubCategoryList { get; set; } = Enumerable.Empty<SelectListItem>();

    }
}
