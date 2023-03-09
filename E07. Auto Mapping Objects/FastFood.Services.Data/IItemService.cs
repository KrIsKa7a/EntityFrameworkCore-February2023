namespace FastFood.Services.Data
{
    using Web.ViewModels.Items;

    public interface IItemService
    {
        Task CreateAsync(CreateItemInputModel model);

        Task<IEnumerable<ItemsAllViewModel>> GetAllAsync();

        Task<IEnumerable<CreateItemViewModel>> GetAllAvailableCategoriesAsync();
    }
}
