namespace PetStore.Web.ViewModels.Categories
{
    using Data.Models;
    using Services.Mapping;

    public class ListCategoryViewModel : IMapFrom<Category>
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;
    }
}
