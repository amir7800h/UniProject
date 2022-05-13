using Application.Contexts.Interfaces;
using Application.Dtos;
using AutoMapper;
using Domain.Catalogs;
using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Catalogs.CatalogTypes.CrudService
{
    public interface ICatalogTypeService
    {
        BaseDto<CatalogTypeDto> Add(CatalogTypeDto catalogType);
        BaseDto<CatalogTypeDto> Edit(CatalogTypeDto catalogType);
        BaseDto<CatalogTypeDto> FindById(int id);
        PaginatedItemsDto<CatalogTypeListDto> GetList(int? parentId, int page, int pageSize);
        BaseDto Remove(int Id);
   
    }

    public class CatalogTypeService : ICatalogTypeService
    {
        private readonly IDataBaseContext context;
        private readonly IMapper mapper;

        public CatalogTypeService(IDataBaseContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }


        public BaseDto<CatalogTypeDto> Add(CatalogTypeDto catalogType)
        {
            var model = mapper.Map<CatalogType>(catalogType);
            context.CatalogTypes.Add(model);
            context.SaveChanges();
            return new BaseDto<CatalogTypeDto>
                (true, new List<string> { $"تایپ {model.Type} با موفقیت افزوده شد" },
                mapper.Map<CatalogTypeDto>(model));           
                     
        }

        public BaseDto<CatalogTypeDto> Edit(CatalogTypeDto catalogType)
        {
            var model = context.CatalogTypes.SingleOrDefault(p => p.Id == catalogType.Id);
            mapper.Map(catalogType, model);
            context.SaveChanges();
            return new BaseDto<CatalogTypeDto>(true, new List<string> { $"تایپ {model.Type} با موفقیت ویرایش شد" },
                mapper.Map<CatalogTypeDto>(model));
        }

        public BaseDto<CatalogTypeDto> FindById(int id)
        {
            var model = context.CatalogTypes.SingleOrDefault(p => p.Id == id);
            var result = mapper.Map<CatalogTypeDto>(model);
            return new BaseDto<CatalogTypeDto>(true, null, result);
        }
        public PaginatedItemsDto<CatalogTypeListDto> GetList(int? parentId, int page, int pageSize)
        {
            int totalCount = 0;
            var model = context.CatalogTypes
                .Where(p=> p.ParentCatalogTypeId == parentId)
                .PagedResult(page, pageSize, out totalCount);
            var result = mapper.ProjectTo<CatalogTypeListDto>(model).ToList();
            return new PaginatedItemsDto<CatalogTypeListDto>(page, pageSize,
                totalCount, result);
        }

        public BaseDto Remove(int Id)
        {
            var catalogType = context.CatalogTypes.Find(Id);
            context.CatalogTypes.Remove(catalogType);
            context.SaveChanges();
            return new BaseDto
            (
             true,
             new List<string> { $"ایتم با موفقیت حذف شد" }
             );
        }
    }

    public class CatalogTypeDto
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public int? ParentCatalogTypeId { get; set; }
    }

    public class CatalogTypeListDto
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public int SubTypeCount { get; set; }
    }
}
