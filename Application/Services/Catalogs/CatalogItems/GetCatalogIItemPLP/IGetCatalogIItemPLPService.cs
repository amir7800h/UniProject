using Application.Catalogs.CatalogItems.UriComposer;
using Application.Contexts.Interfaces;
using Application.Dtos;
using AutoMapper;
using Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Catalogs.CatalogItems.GetCatalogIItemPLP
{
    public interface IGetCatalogIItemPLPService
    {
        PaginatedItemsDto<CatalogPLPDto> Execute(CatlogPLPRequestDto request);
    }

    public class GetCatalogIItemPLPService : IGetCatalogIItemPLPService
    {
        private readonly IDataBaseContext context;
        private readonly IUriComposerService uriComposerService;
        private readonly IMapper mapper;

        public GetCatalogIItemPLPService(IDataBaseContext context
            , IUriComposerService uriComposerService)
        {
            this.context = context;
            this.uriComposerService = uriComposerService;

        }
        public PaginatedItemsDto<CatalogPLPDto> Execute(CatlogPLPRequestDto request)
        {

            var query = context.CatalogItems
                .Include(p => p.Discounts)
                .Include(p => p.CatalogItemImages)
                .OrderByDescending(p => p.Id)
                .AsQueryable();

            if (request.CatalogTypeId != null)
            {
                query = query
                    .Include(p=> p.CatalogType)
                    .Include(p=> p.CatalogType)
                    .ThenInclude(p=> p.ParentCatalogType)
                    .Include(p=> p.CatalogType)
                    .ThenInclude(p=> p.ParentCatalogType)
                    .Where(p => p.CatalogTypeId == request.CatalogTypeId || p.CatalogType.ParentCatalogType.Id == request.CatalogTypeId
                 || p.CatalogType.ParentCatalogType.ParentCatalogType.Id == request.CatalogTypeId);
            }
      

            if (request.brandId != null)
                query = query.Where(p => request.brandId.Any(b => b == p.CatalogBrandId));

            if (!string.IsNullOrEmpty(request.SearchKey))
                query = query.Where(p => request.CatalogTypeId == request.CatalogTypeId);

            if(request.SearchKey != null)
            {
                query = query.Include(p => p.CatalogBrand).Where(p => p.Name.Contains(request.SearchKey)
                 || p.CatalogBrand.Brand.Contains(request.SearchKey));
            }

            if (request.AvailableStock == true)
            {
                query = query.Where(p => p.AvailableStock > 0);
            }

            switch (request.SortType)
            {
                case SortType.MostVisited:
                    query = query.OrderByDescending(p => p.VisitCount);
                    break;

                case SortType.newest:
                    query = query.OrderByDescending(p => p.Id);
                    break;

                case SortType.cheapest:
                    query = query.OrderBy(p => p.Price);
                    break;

                case SortType.mostExpensive:
                    query = query.OrderByDescending((p) => p.Price);
                    break;

                case SortType.Bestselling:
                    query = query.Include(p => p.OrderItems)
                        .OrderByDescending(p => p.OrderItems.Count());
                    break;

                case SortType.MostPopular:
                    query = query.Include(p => p.CatalogItemFavourites)
                        .OrderByDescending(p => p.CatalogItemFavourites.Count());
                    break;
            }

            int rowCount = 0;
            var data = query.PagedResult(request.pageIndex, request.pageSize, out rowCount)
                .ToList()
                .Select(p=> new CatalogPLPDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Slug = p.Slug,
                    Price = p.Price,
                    AvailableStock = p.AvailableStock,
                    Rate = 4,
                    Image = uriComposerService
                   .ComposeImageUri(p?.CatalogItemImages?.FirstOrDefault()?.Src ?? ""),
                }).ToList();

            return new PaginatedItemsDto<CatalogPLPDto>(request.pageIndex, request.pageSize, rowCount, data);
        }
    }

    public class CatlogPLPRequestDto
    {
        public int pageIndex { get; set; } = 1;
        public int pageSize { get; set; } = 10;
        public int? CatalogTypeId { get; set; }
        public int[]? brandId { get; set; }
        public bool AvailableStock { get; set; }
        public string? SearchKey { get; set; }
        public SortType SortType { get; set; }
    }

    public enum SortType
    {
        /// <summary>
        /// بدونه مرتب سازی
        /// </summary>
        None = 0,
        /// <summary>
        /// پربازدیدترین
        /// </summary>
        MostVisited = 1,
        /// <summary>
        /// پرفروش‌ترین
        /// </summary>
        Bestselling = 2,
        /// <summary>
        /// محبوب‌ترین
        /// </summary>
        MostPopular = 3,
        /// <summary>
        ///  ‌جدیدترین
        /// </summary>
        newest = 4,
        /// <summary>
        /// ارزان‌ترین
        /// </summary>
        cheapest = 5,
        /// <summary>
        /// گران‌ترین
        /// </summary>
        mostExpensive = 6,
    }

    public class CatalogPLPDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Slug { get; set; }
        public int Price { get; set; }
        public int AvailableStock { get; set; }
        public string Image { get; set; }
        public byte Rate { get; set; }
    }
}
