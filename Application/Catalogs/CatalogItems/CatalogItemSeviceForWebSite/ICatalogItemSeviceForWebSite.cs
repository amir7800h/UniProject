using Application.Catalogs.CatalogItems.UriComposer;
using Application.Contexts.Interfaces;
using Application.Dtos;
using Common;
using Domain.Catalogs;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Catalogs.CatalogItems.CatalogItemSeviceForWebSite
{
    public interface ICatalogItemSeviceForWebSite
    {
        void SoldCatalogItem(List<SoldCatalogItemDto> soldCatalogItems);
        void AddToMyFavourite(int CatalogItemId, string UserId);
        PaginatedItemsDto<FavouriteCatalogItemDto> GetMyFavourite(string UserId, int page = 1, int pageSize = 20);
    }

    public class CatalogItemSeviceForWebSite : ICatalogItemSeviceForWebSite
    {
        private readonly IDataBaseContext context;
        private readonly IUriComposerService uriComposerService;

        public CatalogItemSeviceForWebSite(IDataBaseContext context, IUriComposerService uriComposerService)
        {
            this.context = context;
            this.uriComposerService = uriComposerService;
        }

        public void AddToMyFavourite(int CatalogItemId, string UserId)
        {           

            var catalogItemFavourite = new CatalogItemFavourite
            {
                CatalogItemId = CatalogItemId,
                UserId = UserId
            };

            context.CatalogItemFavourites.Add(catalogItemFavourite);
            context.SaveChanges();
        }

        public PaginatedItemsDto<FavouriteCatalogItemDto> GetMyFavourite(string UserId, int page = 1, int pageSize = 20)
        {

            //var result = context.CatalogItems
            //    .Include(p=> p.CatalogItemFeatures)
            //    .

            //int rowCount = 0;
            //var data = result?.PagedResult(page, pageSize, out rowCount)
            //    .ToList()
            //    .Select(x => new FavouriteCatalogItemDto
            //    {
            //        Id = x.Id,
            //        AvailableStock = x.CatalogItems.AvailableStock,
            //        Name = x.CatalogItems.Name,
            //        Price = x.Price,
            //        Rate = 4,
            //        Image = uriComposerService
            //        .ComposeImageUri(x?.CatalogItemImages?.SingleOrDefault()?.Src ?? "")

            //    }).ToList();

            return new PaginatedItemsDto<FavouriteCatalogItemDto>
                (page, pageSize, null, data);


        }

        public void SoldCatalogItem(List<SoldCatalogItemDto> soldCatalogItems)
        {
            foreach (var item in soldCatalogItems)
            {
                var items = context.CatalogItems
                    .SingleOrDefault(p => p.Id == item.Id);
                items.AvailableStock -= item.Quantity;
            }
            context.SaveChanges();
        }
    }


    public class SoldCatalogItemDto
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
    }

    public class FavouriteCatalogItemDto
    {
        public int Id { get; set; }
        public int Price { get; set; }
        public int Rate { get; set; }
        public int AvailableStock { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
    }
}
