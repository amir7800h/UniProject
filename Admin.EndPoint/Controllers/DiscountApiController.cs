using Application.Discounts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Admin.EndPoint.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DiscountApiController : ControllerBase
    {
        private readonly IDiscountService discountService;

        public DiscountApiController(IDiscountService discountService)
        {
            this.discountService = discountService;
        }

        [HttpGet]
        [Route("SearchCatalogItem")]
        public IActionResult SearchCatalogItem(string? term)
        {
            return Ok(discountService.GetCatalogItems(term));
        }


    }
}
