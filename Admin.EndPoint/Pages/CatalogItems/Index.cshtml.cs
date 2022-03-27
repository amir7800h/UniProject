using Application.Catalogs.CatalogItems.CatalogItemService;
using Application.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Admin.EndPoint.Pages.CatalogItems
{
    [IgnoreAntiforgeryToken(Order = 2000)]
    public class IndexModel : PageModel
    {
        private readonly ICatalogItemService catalogItemService;

        public IndexModel(ICatalogItemService catalogItemService)
        {
            this.catalogItemService = catalogItemService;
        }
        public PaginatedItemsDto<CatalogItemListItemDto> CatalogItems { get; set; }
        public void OnGet(int page = 1, int pageSize = 100)
        {
            CatalogItems = catalogItemService.GetCatalogList(page, pageSize);
        }

        [HttpPost]
        public JsonResult OnPost(int Id)
        {
            return new JsonResult(catalogItemService.RemoveCatalog(Id));
        }
    }
}
