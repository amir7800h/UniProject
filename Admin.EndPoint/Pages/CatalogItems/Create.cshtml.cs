using Application.Catalogs.CatalogItems.AddNewCatalogItem;
using Application.Catalogs.CatalogItems.CatalogItemService;
using Application.Dtos;
using Infrastructure.ExternalApi;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Caching.Distributed;

namespace Admin.EndPoint.Pages.CatalogItems
{
    public class CreateModel : PageModel
    {
        private readonly IAddNewCatalogItemService addNewCatalogItemService;
        private readonly ICatalogItemService catalogItemService;
        private readonly IImageUploadService imageUpload;
        private readonly IDistributedCache distributedCache;

        public CreateModel(IAddNewCatalogItemService addNewCatalogItemService
            , ICatalogItemService catalogItemService, IImageUploadService imageUpload
            , IDistributedCache distributedCache)
        {
            this.addNewCatalogItemService = addNewCatalogItemService;
            this.catalogItemService = catalogItemService;
            this.imageUpload = imageUpload;
            this.distributedCache = distributedCache;
        }

        public SelectList Categories { get; set; }
        public SelectList Brands { get; set; }

        [BindProperty]
        public AddNewCatalogItemDto Data { get; set; }

        [BindProperty]
        public List<IFormFile> Files { get; set; }
        public void OnGet()
        {
            Categories = new SelectList(catalogItemService.GetCatalogType(), "Id", "Type");
            Brands = new SelectList(catalogItemService.GetBrand(), "Id", "Brand");
        }

        public JsonResult OnPost()
        {
            //if (!ModelState.IsValid)
            //{
            //    var allErrors = ModelState.Values.SelectMany(v => v.Errors);
            //    return new JsonResult(new BaseDto<int>
            //        (false, allErrors.Select(p => p.ErrorMessage).ToList(), 0));
            //}

            for (int i = 0; i < Request.Form.Files.Count(); i++)
            {
                var file = Request.Form.Files[i];
                Files.Add(file);
            }
            List<AddNewCatalogItemImage_Dto> images = new List<AddNewCatalogItemImage_Dto>();

            if (Files.Count > 0)
            {
                var result = imageUpload.Upload(Files);
                foreach(var item in result)
                {
                    images.Add(new AddNewCatalogItemImage_Dto { Src = item });
                }
                
            }
            Data.Images = images;
            var resultService = addNewCatalogItemService.Execute(Data);
            return new JsonResult(resultService);
        }
    }
}
