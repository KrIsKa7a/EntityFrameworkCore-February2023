namespace PetStore.Web.ViewModels.Categories
{
    using Data.Models;
    using Services.Mapping;

    public class ListAllCategoriesViewModel
    {
        public IEnumerable<ListCategoryViewModel> AllCategories { get; set; }

        public int PageCount { get; set; }

        public int ActivePage { get; set; }
    }
}
