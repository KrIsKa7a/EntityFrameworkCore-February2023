namespace FastFood.Services.Data
{
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using FastFood.Data;
    using Microsoft.EntityFrameworkCore;
    using Models;
    using Web.ViewModels.Positions;

    public class PositionsService : IPositionsService
    {
        // Dependency Injection
        private readonly IMapper mapper;
        private readonly FastFoodContext context;

        public PositionsService(IMapper mapper, FastFoodContext context)
        {
            this.mapper = mapper;
            this.context = context;
        }

        public async Task CreateAsync(CreatePositionInputModel inputModel)
        {
            Position position = this.mapper.Map<Position>(inputModel);

            await this.context.Positions.AddAsync(position);
            await this.context.SaveChangesAsync();
        }

        public async Task<IEnumerable<PositionsAllViewModel>> GetAllAsync()
            => await this.context.Positions
                .ProjectTo<PositionsAllViewModel>(this.mapper.ConfigurationProvider)
                .ToArrayAsync();
    }
}