using Application.Catalogs.CatalogItems.GetCatalogIItemPLP;
using Application.Catalogs.CatalogItems.GetCatalogItemPDP;
using Application.Dtos;
using Infrastructure.Cachehelper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using System.Text;
using System.Text.Json;
using WebSite.EndPoint.Models.ViewComponents;

namespace WebSite.EndPoint.Controllers
{
    public class ProductController : Controller
    {
        private readonly IGetCatalogIItemPLPService getCatalogIItemPLPService;
        private readonly IGetCatalogItemPDPService getCatalogItemPDPService;
        private readonly IDistributedCache distributedCache;

        public ProductController(IGetCatalogIItemPLPService getCatalogIItemPLPService
            , IGetCatalogItemPDPService getCatalogItemPDPService
            , IDistributedCache distributedCache)
        {
            this.getCatalogIItemPLPService = getCatalogIItemPLPService;
            this.getCatalogItemPDPService = getCatalogItemPDPService;
            this.distributedCache = distributedCache;
        }
        public IActionResult Index(CatlogPLPRequestDto catlogPLPRequestDto)
        {
            var data = new PaginatedItemsDto<CatalogPLPDto>(catlogPLPRequestDto.pageIndex, catlogPLPRequestDto.pageSize, 0, null);
            var key = CacheHelper.GenerateCatalogItemChacheKey(catlogPLPRequestDto.pageIndex,
                catlogPLPRequestDto.pageSize, catlogPLPRequestDto?.CatalogTypeId, catlogPLPRequestDto.brandId
                , catlogPLPRequestDto.AvailableStock, catlogPLPRequestDto.SearchKey, catlogPLPRequestDto.SortType);
            var CatalogPlpCache = distributedCache.GetAsync(key).Result;

            if (CatalogPlpCache != null)
            {
                data = JsonSerializer.Deserialize<PaginatedItemsDto<CatalogPLPDto>>(CatalogPlpCache);
            }
            else
            {
                data = getCatalogIItemPLPService.Execute(catlogPLPRequestDto);
                string jsonData = JsonSerializer.Serialize(data);
                byte[] encodedJson = Encoding.UTF8.GetBytes(jsonData);
                var options = new DistributedCacheEntryOptions()
                    .SetSlidingExpiration(CacheHelper.DefaultCacheDuration);

                distributedCache.SetAsync(key, encodedJson, options);
            }
            //var data = getCatalogIItemPLPService.Execute(catlogPLPRequestDto);
            return View(data);
        }

        public IActionResult Details(int Id, string slug)
        {
            var data = getCatalogItemPDPService.Execute(Id);
            return View(data);
        }
    }
}
