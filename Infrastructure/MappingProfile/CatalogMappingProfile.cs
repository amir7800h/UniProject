using Application.Catalogs.CatalogItems.AddNewCatalogItem;
using Application.Catalogs.CatalogItems.CatalogItemService;
using Application.Catalogs.CatalogItems.GetCatalogIItemPLP;
using Application.Catalogs.CatalogTypes.CrudService;
using Application.Catalogs.GetMenuItem;
using AutoMapper;
using Domain.Catalogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.MappingProfile
{
    public class CatalogMappingProfile:Profile
    {
        public CatalogMappingProfile()
        {
            CreateMap<CatalogType, CatalogTypeDto>().ReverseMap();


            CreateMap<CatalogItem, CatalogItemListItemDto>()
                .ForMember(dest => dest.Type, opt =>
                 opt.MapFrom(src => src.CatalogType.Type))
                .ForMember(dest => dest.Brand, opt =>
                opt.MapFrom(src => src.CatalogBrand.Brand));


          


            CreateMap<CatalogType, CatalogTypeListDto>()
                .ForMember(dest => dest.SubTypeCount, option =>
                 option.MapFrom(src => src.SubType.Count()));

            CreateMap<CatalogType, MenuItemDto>()
                .ForMember(dest => dest.Name, opt =>
                opt.MapFrom(src => src.Type))
                .ForMember(dest => dest.ParentId, opt =>
                 opt.MapFrom(src => src.ParentCatalogTypeId))
                .ForMember(dest => dest.SubMenu, opt =>
                opt.MapFrom(src => src.SubType));


            CreateMap<CatalogItemFeature, AddNewCatalogItemFeature_dto>().ReverseMap();
            CreateMap<CatalogItemImage, AddNewCatalogItemImage_Dto>().ReverseMap();

            CreateMap<CatalogItem, AddNewCatalogItemDto>()
                .ForMember(dest => dest.Features, opt =>
                 opt.MapFrom(src => src.CatalogItemFeatures))
                .ForMember(dest => dest.Images, opt =>
                 opt.MapFrom(src => src.CatalogItemImages)).ReverseMap();

            CreateMap<CatalogBrand, CatalogBrandDto>().ReverseMap();
            CreateMap<CatalogType, CatalogTypeDto>().ReverseMap();
        }
    }
}
