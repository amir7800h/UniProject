using Application.Contexts.Interfaces;
using Application.Dtos;
using AutoMapper;
using Domain.Catalogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Catalogs.CatalogItems.AddNewCatalogItem
{
    public interface IAddNewCatalogItemService
    {
        BaseDto<int> Execute(AddNewCatalogItemDto request);
    }


    public class AddNewCatalogItemService : IAddNewCatalogItemService
    {
        private readonly IDataBaseContext context;
        private readonly IMapper mapper;
        public AddNewCatalogItemService(IDataBaseContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }
        public BaseDto<int> Execute(AddNewCatalogItemDto request)
        {            
            var catalogItem = mapper.Map<CatalogItem>(request);

            if (request.Slug != null)
                catalogItem.Slug = request.Slug.Replace(' ', '-');

            context.CatalogItems.Add(catalogItem);
            context.SaveChanges();

            return new BaseDto<int>
                (true, new List<string> { "محصول با موفقیت ثبت شد" }, catalogItem.Id);
        }


        //private string SlugFormat(string Slug)
        //{
        //    حذف فاصله‌ی یک رشته
        //    string[] words = Slug.Split(' ');
        //    string result = "";
        //    for (int i = 0; i < words.Length; i++)
        //    {
        //        result += words[i].Trim();
        //    }

        //    return result;
        //}
    }

    public class AddNewCatalogItemImage_Dto
    {
        public string Src { get; set; }
    }

    public class AddNewCatalogItemFeature_dto
    {
        public string Group { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }

    }
}
