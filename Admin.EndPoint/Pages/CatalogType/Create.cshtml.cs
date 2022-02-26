using Admin.EndPoint.ViewModels.Catalogs;
using Application.Catalogs.CatalogTypes.CrudService;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Admin.EndPoint.Pages.CatalogType
{
    public class CreateModel : PageModel
    {
        private readonly ICatalogTypeService catalogTypeService;
        private readonly IMapper mapper;

        public CreateModel(ICatalogTypeService catalogTypeService, IMapper mapper)
        {
            this.catalogTypeService = catalogTypeService;
            this.mapper = mapper;
        }
        [BindProperty]
        public CatalogTypeViewModel CatalogType { get; set; } = new CatalogTypeViewModel { };
        public List<String> Message { get; set; } = new List<string>();
        public void OnGet(int? parentId)
        {
            CatalogType.ParentCatalogTypeId = parentId;
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            var model = mapper.Map<CatalogTypeDto>(CatalogType);
            var result = catalogTypeService.Add(model);
            if (result.IsSuccess)
            {
                return RedirectToPage("index", new { parentid = CatalogType.ParentCatalogTypeId });
            }
            Message = result.Message;
            return Page();
        }
    }
}
