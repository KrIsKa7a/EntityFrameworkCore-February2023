namespace FastFood.Services.Data
{
    using AutoMapper;
    using AutoMapper.QueryableExtensions;

    using FastFood.Data;
    using Microsoft.EntityFrameworkCore;
    using Models;
    using Web.ViewModels.Categories;

    public class CategoryService : ICategoryService
    {
        private readonly IMapper mapper;
        private readonly FastFoodContext context;

        public CategoryService(IMapper mapper, FastFoodContext context)
        {
            this.mapper = mapper;
            this.context = context;
        }

        public async Task CreateAsync(CreateCategoryInputModel model)
        {
            Category category = this.mapper.Map<Category>(model);

            await this.context.Categories.AddAsync(category);
            await this.context.SaveChangesAsync();
        }

        public async Task<IEnumerable<CategoryAllViewModel>> GetAllAsync()
            => await this.context.Categories
                .ProjectTo<CategoryAllViewModel>(this.mapper.ConfigurationProvider)
                .ToArrayAsync();
    }
}
