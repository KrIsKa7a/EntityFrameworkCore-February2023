namespace FastFood.Services.Mapping
{
    using AutoMapper;

    using Models;
    using Web.ViewModels.Categories;
    using Web.ViewModels.Items;
    using Web.ViewModels.Positions;

    public class FastFoodProfile : Profile
    {
        public FastFoodProfile()
        {
            //Positions
            this.CreateMap<CreatePositionInputModel, Position>()
                .ForMember(d => d.Name, 
                    opt => opt.MapFrom(s => s.PositionName));

            this.CreateMap<Position, PositionsAllViewModel>()
                .ForMember(d => d.Name, 
                    opt => opt.MapFrom(s => s.Name));

            // Categories
            this.CreateMap<CreateCategoryInputModel, Category>()
                .ForMember(d => d.Name, 
                    opt => opt.MapFrom(s => s.CategoryName));

            this.CreateMap<Category, CategoryAllViewModel>();

            // Items
            this.CreateMap<Category, CreateItemViewModel>()
                .ForMember(d => d.CategoryId, 
                    opt => opt.MapFrom(s => s.Id))
                .ForMember(d => d.CategoryName, 
                    opt => opt.MapFrom(s => s.Name));

            this.CreateMap<CreateItemInputModel, Item>();
            this.CreateMap<Item, ItemsAllViewModel>()
                .ForMember(d => d.Category, 
                    opt => opt.MapFrom(s => s.Category.Name));
        }
    }
}
