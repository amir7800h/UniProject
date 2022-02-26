using Admin.EndPoint.ViewModels.Catalogs;
using Application.Catalogs.CatalogTypes.CrudService;
using AutoMapper;

namespace Admin.EndPoint.MappingProfile
{
    public class CatalogVMMappingProfile:Profile
    {
        public CatalogVMMappingProfile()
        {
            CreateMap<CatalogTypeDto, CatalogTypeViewModel>().ReverseMap();
        }

    }
}
