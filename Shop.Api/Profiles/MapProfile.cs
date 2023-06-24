using AutoMapper;
using Shop.Api.Dtos.BrandDtos;
using Shop.Api.Dtos.ProductDtos;
using Shop.Core.Entities;

namespace Shop.Api.Profiles
{
    public class MapProfile : Profile
    {
        public MapProfile()
        {
            CreateMap<Product, ProductGetDto>();
            CreateMap<ProductPostDto, Product>();
            CreateMap<Product, ProductGetAllItem>()
                .ForMember(dest => dest.HasDiscount, m => m.MapFrom(x => x.DiscountPercent > 0));

            CreateMap<BrandPostDto, Brand>();
            CreateMap<Brand, BrandGetAllItemDto>();
            CreateMap<Brand, BrandInProductsDto>();
        }
    }
}
