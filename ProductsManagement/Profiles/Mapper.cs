using AutoMapper;
using Microsoft.Win32;
using ProductsManagement.Models.DTOs;
using ProductsManagement.Models.Entities;
using ProductsManagement.Models.Requests;

namespace ProductsManagement.Profiles
{
    public class Mapper: Profile
    {
        public Mapper()
        {
            CreateMap<CategoryRequest, Category>();
            CreateMap<Category, CategoryDTO>();
            CreateMap<AttributeRequest, Models.Entities.Attribute>();
            CreateMap<Models.Entities.Attribute, AttributeDTO>();
            CreateMap<RegisterRequest, User>().ForMember(dest => dest.Password, opt => opt.Ignore());
            CreateMap<User, UserDTO>();
            CreateMap<ProductRequest, Product>();
            CreateMap<AttributeValueRequest, AttributeValue>();
            CreateMap<Product, ProductDTO>();
            CreateMap<AttributeValue, AttributeValueDTO>();
        }
    }
}
