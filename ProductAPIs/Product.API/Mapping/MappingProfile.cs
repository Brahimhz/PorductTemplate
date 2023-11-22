using AutoMapper;
using Product.API.AppService.Dtos.Product;
using ProductEntity = Product.Core.Models;

namespace Product.API.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<ProductEntity.Product, ProductOutPut>();
            CreateMap<ProductEntity.Product, ProductOutPutList>();
            CreateMap<ProductInPut, ProductEntity.Product>();
        }
    }
}
