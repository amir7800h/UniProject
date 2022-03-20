using Application.Contexts.Interfaces;
using AutoMapper;
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
}
