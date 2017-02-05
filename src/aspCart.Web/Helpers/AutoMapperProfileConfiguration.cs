using aspCart.Core.Domain.Catalog;
using aspCart.Web.Areas.Admin.Models.Catalog;
using aspCart.Web.Models;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace aspCart.Web.Helpers
{
    public class AutoMapperProfileConfiguration : Profile
    {
        public AutoMapperProfileConfiguration()
        {
            // category mappings
            CreateMap<Category, CategoryListModel>();
            CreateMap<Category, CategoryCreateOrUpdateModel>()
                .ReverseMap();

            // manufacturer mappings
            CreateMap<Manufacturer, ManufacturerListModel>();
            CreateMap<Manufacturer, ManufacturerCreateOrUpdateModel>()
                .ReverseMap();

            // product mappings
            CreateMap<Product, ProductListModel>();
            CreateMap<Product, ProductModel>();
            CreateMap<Product, ProductCreateOrUpdateModel>()
                .ForMember(dest => dest.Images, opt => opt.Ignore())
                .ReverseMap();
        }
    }
}
