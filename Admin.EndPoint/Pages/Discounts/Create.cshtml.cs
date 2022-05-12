using Admin.EndPoint.Binders;
using Application.Discounts.AddNewDiscountService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Admin.EndPoint.Pages.Discounts
{
    public class CreateModel : PageModel
    {
        private readonly IAddNewDiscountService discountService;

        [ModelBinder(BinderType =  typeof(DiscountEntityBinder))]
        [BindProperty]
        public AddNewDiscountDto Model { get; set; }
        public CreateModel(IAddNewDiscountService discountService)
        {
            this.discountService = discountService;
        }
        public void OnGet()
        {
            
        }

        public void OnPost()
        {
            discountService.Execute(Model);
        }
    }
}
