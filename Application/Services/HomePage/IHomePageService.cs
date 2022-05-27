using Application.Catalogs.CatalogItems.GetCatalogIItemPLP;
using Application.Catalogs.CatalogItems.UriComposer;
using Application.Contexts.Interfaces;
using Domain.Banners;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.HomePage
{
    public interface IHomePageService
    {
        HomePageDto GetData();
    }

    public class HomePageService : IHomePageService
    {
        private readonly IDataBaseContext context;
        private readonly IUriComposerService uriComposerService;
        private readonly IGetCatalogIItemPLPService getCatalogIItemPLPService;

        public HomePageService(IDataBaseContext context
            , IUriComposerService uriComposerService
            , IGetCatalogIItemPLPService getCatalogIItemPLPService)
        {
            this.context = context;
            this.uriComposerService = uriComposerService;
            this.getCatalogIItemPLPService = getCatalogIItemPLPService;
        }
        public HomePageDto GetData()
        {
            var banners = context.Banners.Where(x=> x.IsActive == true)
                .OrderBy(x=> x.Priority)
                .ThenByDescending(x=> x.Id)
                .Select(x => new BannerDto
                {
                    Id = x.Id,
                    Image = uriComposerService.ComposeImageUri(x.Image),
                    Link = x.Link,
                    Position = x.Position,
                }).ToList();


            var MostPopular = getCatalogIItemPLPService.Execute(new CatlogPLPRequestDto
            {
                AvailableStock = true,
                pageIndex = 1,
                pageSize = 20,
                SortType = SortType.MostPopular
            }).Data.ToList();

            var Bestselling = getCatalogIItemPLPService.Execute(new CatlogPLPRequestDto
            {
                AvailableStock = true,
                pageIndex = 1,
                pageSize = 20,
                SortType = SortType.Bestselling
            }).Data.ToList();

          

            return new HomePageDto
            {
                Banners = banners,
                bestSellers = Bestselling,
                MostPopular = MostPopular,
            };
        }
    }

    public class HomePageDto
    {
        public List<BannerDto> Banners { get; set; }
        public List<CatalogPLPDto> MostPopular { get; set; }
        public List<CatalogPLPDto> bestSellers { get; set; }

    }

    public class BannerDto
    {
        public int Id { get; set; }
        public string Image { get; set; }
        public string Link { get; set; }
        public Position Position { get; set; }
    }
}
