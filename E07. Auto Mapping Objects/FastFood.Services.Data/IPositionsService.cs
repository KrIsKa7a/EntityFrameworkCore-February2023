namespace FastFood.Services.Data
{
    using Web.ViewModels.Positions;

    public interface IPositionsService
    {
        Task CreateAsync(CreatePositionInputModel inputModel);

        Task<IEnumerable<PositionsAllViewModel>> GetAllAsync();
    }
}
