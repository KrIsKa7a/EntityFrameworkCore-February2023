namespace ProductShop
{
    using AutoMapper;
    using DTOs.Export;
    using DTOs.Import;
    using Models;

    public class ProductShopProfile : Profile
    {
        public ProductShopProfile() 
        {
            // User
            this.CreateMap<ImportUserDto, User>();

            // Product
            this.CreateMap<ImportProductDto, Product>();
            this.CreateMap<Product, ExportProductInRangeDto>()
                .ForMember(d => d.ProductName,
                    opt => opt.MapFrom(s => s.Name))
                .ForMember(d => d.ProductPrice,
                    opt => opt.MapFrom(s => s.Price))
                .ForMember(d => d.SellerName,
                    opt => opt.MapFrom(s => $"{s.Seller.FirstName} {s.Seller.LastName}"));

            // Category
            this.CreateMap<ImportCategoryDto, Category>();

            // CategoryProduct
            this.CreateMap<ImportCategoryProductDto, CategoryProduct>();
        }
    }
}
