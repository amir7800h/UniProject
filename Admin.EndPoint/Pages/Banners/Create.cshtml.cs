using Application.Services.Banners;
using Infrastructure.Cachehelper;
using Infrastructure.ExternalApi;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Caching.Distributed;

namespace Admin.EndPoint.Pages.Banners
{
    public class CreateModel : PageModel
    {
        private readonly IBannersService banners;
        private readonly IImageUploadService imageUploadService;
        private readonly IDistributedCache distributedCache;

        public CreateModel(IBannersService banners,
            IImageUploadService imageUploadService
            ,IDistributedCache distributedCache)
        {
            this.banners = banners;
            this.imageUploadService = imageUploadService;
            this.distributedCache = distributedCache;
        }

        [BindProperty]
        public BannerDto Banner { get; set; }
        [BindProperty]
        public IFormFile BannerImage { get; set; }

        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {

            

            var result = imageUploadService.Upload(new List<IFormFile> { BannerImage });
            if(result.Count > 0)
            {
                Banner.Image = result.FirstOrDefault();
                banners.AddBanner(Banner);

                distributedCache.Remove(CacheHelper.GenerateHomePageCacheKey());

            }
            return RedirectToPage("Index");
        }
    }
}
