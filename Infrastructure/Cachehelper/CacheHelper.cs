using Application.Catalogs.CatalogItems.GetCatalogIItemPLP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Cachehelper
{
    public static class CacheHelper
    {
        public static readonly TimeSpan DefaultCacheDuration = TimeSpan.FromSeconds(20);
        private static readonly string _itemsKeyTemplate = "items-{0}-{1}-{2}-{3}-{4}-{5}-{6}";

        public static string GenerateCatalogItemChacheKey(int pageIndex, int ItemsPage, int? typeId
            , int[] brandId, bool AvailableStock, string SeaarchKey, SortType sortType)
        {
            string brands = "";
            if(brandId != null)
            {
                for (int i = 0; i < brandId.Length; i++)
                {
                    brands += brandId[i].ToString();
                }
            }            
            return string.Format(_itemsKeyTemplate, pageIndex, ItemsPage, typeId.ToString()
                , brands, AvailableStock, SeaarchKey, sortType);
        }
        public static string GenerateHomePageCacheKey()
        {
            return "HomePage";
        }
    }
}
