using Application.Contexts.Interfaces;
using Domain.Discounts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Discounts.AddNewDiscountService
{
    public interface IAddNewDiscountService
    {
        void Execute(AddNewDiscountDto discount);
    }

    public class AddNewDiscountService : IAddNewDiscountService
    {
        private readonly IDataBaseContext context;
        public AddNewDiscountService(IDataBaseContext context)
        {
            this.context = context;
        }

        /// <summary>
        ///  افزودن تخفیف جدید
        /// </summary>
        /// <param name="discount"></param>
        public void Execute(AddNewDiscountDto discount)
        {
            var newdiscount = new Discount()
            {
                Name = discount.Name,
                CouponCode = discount.CouponCode,
                DiscountAmount = discount.DiscountAmount,
                DiscountLimitationId = discount.DiscountLimitationId,
                DiscountPercentage = discount.DiscountPercentage,
                LimitationTimes = discount.LimitationTimes,
                DiscountTypeId = discount.DiscountTypeId,
                EndDate = discount.EndDate,
                RequiresCouponCode = discount.RequiresCouponCode,
                StartDate = discount.StartDate,
                UsePercentage = discount.UsePercentage,
            };

            if(discount.appliedToCatalogItem != null)
            {
                var catalogItems = context.CatalogItems
                    .Where(p => discount.appliedToCatalogItem.Contains(p.Id)).ToList();
                newdiscount.CatalogItems = catalogItems;
            }

            if (discount.appliedToCatalogType != null)
            {
                newdiscount.CatalogTypes = context.CatalogTypes
                    .Where(p => discount.appliedToCatalogType.Equals(p.Id)).ToList();
            }

            context.Discounts.Add(newdiscount);            
            context.SaveChanges();

        }
    }




        public class AddNewDiscountDto
    {
        [Display(Name = "نام تخفیف")]
        public string Name { get; set; }
        [Display(Name = "استفاده از درصد؟")]
        public bool UsePercentage { get; set; } = true;
        [Display(Name = "درصد تخفیف")]
        public int DiscountPercentage { get; set; } = 0;
        [Display(Name = "مبلغ تخفیف")]
        public int DiscountAmount { get; set; } = 0;
        [Display(Name = "زمان شروع")]
        public DateTime? StartDate { get; set; }
        [Display(Name = "زمان انقضا")]
        public DateTime? EndDate { get; set; }
        [Display(Name = "استفاده از کوپن")]
        public bool RequiresCouponCode { get; set; }
        [Display(Name = "کد کوپن")]
        public string CouponCode { get; set; }
        [Display(Name = "نوع تخفیف")]
        public int DiscountTypeId { get; set; }
        [Display(Name = "محدودیت تخفیف")]
        public int DiscountLimitationId { get; set; }

        [Display(Name = "تعداد کد تخفیف")]
        public int LimitationTimes { get; set; } = 0;
        [Display(Name = "اعمال برای محصول")]
        public List<int> appliedToCatalogItem { get; set; }  
        [Display(Name = "اعمال برای دسته بندی")]
        public List<int> appliedToCatalogType { get; set; }


    }

}
