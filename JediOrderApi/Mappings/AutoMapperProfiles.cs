using AutoMapper;
using Core.Models.Domain;
using Core.Models.DTO;

namespace JediOrderApi.Mappings
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Product, ProductsResponse>().ReverseMap();
            CreateMap<AddProductRequest, Product>().ReverseMap();
            CreateMap<Review, ReviewResponse>().ReverseMap();
            CreateMap<AddReviewsRequest, Review>().ReverseMap();
            CreateMap<RegisterRequestDto, AppUser>().ReverseMap();
        }
    }
}
