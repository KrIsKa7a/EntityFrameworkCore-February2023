namespace FastFood.Services.Data
{
    using Web.ViewModels.Categories;

    public interface ICategoryService
    {
        Task CreateAsync(CreateCategoryInputModel model);

        Task<IEnumerable<CategoryAllViewModel>> GetAllAsync();
    }
}
