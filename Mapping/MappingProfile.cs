using AutoMapper;
using RestApiExample.Models;
using RestApiExample.DTOs;
using RestApiExample.Models.Embeddables;

namespace RestApiExample.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Product, ProductDto>();
            CreateMap<CreateProductDto, Product>();
            CreateMap<UpdateProductDto, Product>();

            CreateMap<Pricing, PricingDto>();
            CreateMap<PricingDto, Pricing>();

            CreateMap<Inventory, InventoryDto>();
            CreateMap<InventoryDto, Inventory>();

            CreateMap<ProductMetadata, ProductMetadataDto>();
            CreateMap<ProductMetadataDto, ProductMetadata>();
        }
    }
}
