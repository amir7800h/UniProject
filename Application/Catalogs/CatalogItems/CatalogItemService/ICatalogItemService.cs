using Application.Contexts.Interfaces;
using Application.Dtos;
using AutoMapper;
using Common;
using Domain.Catalogs;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Catalogs.CatalogItems.CatalogItemService
{
    public interface ICatalogItemService
    {
        List<CatalogBrandDto> GetBrand();
        List<ListCatalogTypeDto> GetCatalogType();
        PaginatedItemsDto<CatalogItemListItemDto> GetCatalogList(int page, int pageSize);
        BaseDto RemoveCatalog(int Id);

    }

    public class CatalogItemService : ICatalogItemService
    {
        private readonly IDataBaseContext context;
        private readonly IMapper mapper;

        public CatalogItemService(IDataBaseContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }
        public List<CatalogBrandDto> GetBrand()
        {
            var brands = context.CatalogBrands.OrderBy(p=> p.Brand)
                .Take(500).ToList();
            var result = mapper.Map<List<CatalogBrandDto>>(brands);
            return result;
        }

        public PaginatedItemsDto<CatalogItemListItemDto> GetCatalogList(int page, int pageSize)
        {
            int rowCount = 0;
            var data = context.CatalogItems
                .Include(p => p.CatalogType)
                .Include(p => p.CatalogBrand)
                .ToPaged(page, pageSize, out rowCount)
                
                .OrderByDescending(p => p.Id).ToList() ;

            var result = mapper.Map<List<CatalogItemListItemDto>>(data);

            return new PaginatedItemsDto<CatalogItemListItemDto>(page, pageSize, rowCount, result);
        }

        public List<ListCatalogTypeDto> GetCatalogType()
        {
            var catalogTypes = context.CatalogTypes
                .Include(p=> p.ParentCatalogType)
                .Include(p=> p.ParentCatalogType)
                .ThenInclude(p=> p.ParentCatalogType.ParentCatalogType)
                .Include(p=> p.SubType)
                .Where(p=> p.ParentCatalogTypeId != null)
                .Where(p=> p.SubType.Count == 0)
                .Select(p => new { p.Id, p.Type, p.ParentCatalogType, p.SubType })
                .ToList()
                .Select(p => new ListCatalogTypeDto
                {
                    Id = p.Id,
                    Type = $"{p?.Type ?? ""} - {p?.ParentCatalogType?.Type ?? ""} - {p?.ParentCatalogType?.ParentCatalogType?.Type ?? ""}"
                }).ToList();
            return catalogTypes;
        }

        public BaseDto RemoveCatalog(int Id)
        {
            var catalog = context.CatalogItems.SingleOrDefault(p => p.Id == Id);
            if (catalog != null)
            {
                context.CatalogItems.Remove(catalog);
                context.SaveChanges();
                return new BaseDto(true, new List<string> { "محصول با موفقیت حذف شد" });
            }
            else
            {
                return new BaseDto(false, new List<string> { "محصول پیدا نشد" });
            }             
        }
    }

    public class CatalogBrandDto
    {
        public int Id { get; set; }
        public string Brand { get; set; }
    }
    public class ListCatalogTypeDto
    {
        public int Id { get; set; }
        public string Type { get; set; }
    }

    public class CatalogItemListItemDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
        public string Type { get; set; }
        public string Brand { get; set; }
        public int AvailableStock { get; set; }
        public int RestockThreshold { get; set; }
        public int MaxStockThreshold { get; set; }
    }
}
