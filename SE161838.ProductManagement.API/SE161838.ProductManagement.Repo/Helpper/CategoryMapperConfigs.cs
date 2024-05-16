using AutoMapper;
using Microsoft.Data.SqlClient;
using SE161838.ProductManagement.Repo.Models;
using SE161838.ProductManagement.Repo.ViewModels.Category;
using SE161838.ProductManagement.Repo.ViewModels.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SE161838.ProductManagement.Repo.Mappers
{
    public partial class MapperConfigs : Profile
    {
        partial void CategoryMapperConfigs()
        {
            CreateMap<Product, ProductViewModels>()
                .ForMember(c => c.CategoryName, opt => opt.MapFrom(src => src.Category.CategoryName)).ReverseMap();
            CreateMap<Product, ProductCreateModels>().ReverseMap();
            CreateMap<Product, ProductUpdateModels>().ReverseMap();
        }
    }
}
