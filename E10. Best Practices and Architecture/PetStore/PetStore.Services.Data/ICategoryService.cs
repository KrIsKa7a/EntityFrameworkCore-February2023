namespace PetStore.Services.Data
{
    using Web.ViewModels.Categories;

    public interface ICategoryService
    {
        Task CreateAsync(CreateCategoryInputModel inputModel);

        Task<IEnumerable<ListCategoryViewModel>> GetAllAsync();

        Task<IEnumerable<ListCategoryViewModel>> GetAllWithPaginationAsync(int pageNumber);

        Task<EditCategoryViewModel> GetByIdAndPrepareForEditAsync(int id);

        Task EditCategoryAsync(EditCategoryViewModel inputModel);

        Task<bool> ExistsAsync(int id);
    }
}
