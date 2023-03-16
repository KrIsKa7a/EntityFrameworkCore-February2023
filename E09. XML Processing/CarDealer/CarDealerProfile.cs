namespace CarDealer
{
    using System.Globalization;

    using AutoMapper;

    using DTOs.Export;
    using DTOs.Import;
    using Models;

    public class CarDealerProfile : Profile
    {
        public CarDealerProfile()
        {
            // Supplier
            this.CreateMap<ImportSupplierDto, Supplier>();

            // Part
            this.CreateMap<ImportPartDto, Part>()
                .ForMember(d => d.SupplierId,
                    opt => opt.MapFrom(s => s.SupplierId!.Value));
            this.CreateMap<Part, ExportCarPartDto>();

            // Car
            this.CreateMap<ImportCarDto, Car>()
                .ForSourceMember(s => s.Parts, opt => opt.DoNotValidate());
            this.CreateMap<Car, ExportCarDto>();
            this.CreateMap<Car, ExportBmwCarDto>();
            this.CreateMap<Car, ExportCarWithPartsDto>()
                .ForMember(d => d.Parts,
                    opt => opt.MapFrom(s => 
                        s.PartsCars
                            .Select(pc => pc.Part)
                            .OrderByDescending(p => p.Price)
                            .ToArray()));

            // Customer
            this.CreateMap<ImportCustomerDto, Customer>()
                .ForMember(d => d.BirthDate,
                    opt => opt.MapFrom(s => DateTime.Parse(s.BirthDate, CultureInfo.InvariantCulture)));

            // Sale
            this.CreateMap<ImportSaleDto, Sale>()
                .ForMember(d => d.CarId,
                    opt => opt.MapFrom(s => s.CarId.Value));
        }
    }
}
