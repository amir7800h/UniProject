using Application.Contexts.Interfaces;
using Application.Dtos;
using Domain.Discounts;
using Domain.Users;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Discounts
{
    public interface IDiscountService
    {
        List<CatlogItemDto> GetCatalogItems(string searchKey);
        bool ApplyDiscountInBasket(string CoponCode, int BasketId);
        string GetCoponCode(int BasketId);
        bool RemoveDiscountFromBasket(int BasketId);
        BaseDto IsDiscountValid(string couponCode, User user);

    }

    public class DiscountService : IDiscountService
    {
        private readonly IDataBaseContext context;
        private readonly IDiscountHistoryService discountHistoryService;

        public DiscountService(IDataBaseContext context, IDiscountHistoryService discountHistoryService)
        {
            this.context = context;
            this.discountHistoryService = discountHistoryService;
        }

        public bool ApplyDiscountInBasket(string CoponCode, int BasketId)
        {
            var basket = context.Baskets
           .Include(p => p.Items)
           .Include(p => p.AppliedDiscount)
           .FirstOrDefault(p => p.Id == BasketId);


            var discount = context.Discounts
                .Where(p => p.CouponCode.Equals(CoponCode))
                .FirstOrDefault();

            basket.ApplyDisCountCode(discount);
            context.SaveChanges();
            return true;
        }

        /// <summary>
        /// My Descriptions
        /// </summary>
        /// <param name="searchKey"></param>
        /// <returns></returns>
        public List<CatlogItemDto> GetCatalogItems(string searchKey)
        {
            var result = new List<CatlogItemDto>();

            if (!string.IsNullOrEmpty(searchKey))
            {
                result = context.CatalogItems
                    .Include(x=> x.CatalogType)
                    .Where(x => x.Name.Contains(searchKey) || x.CatalogType.Type.Contains(searchKey))
                    .Select(x => new CatlogItemDto
                    {
                        Id = x.Id,
                        Name = x.Name,
                    }).ToList();

                return result;
            }

            result = context.CatalogItems
               .OrderByDescending(x => x.Id)
               .Take(10)
               .Select(x => new CatlogItemDto
               {
                   Id = x.Id,
                   Name = x.Name,
               }).ToList();

            return result;
        }

        public BaseDto IsDiscountValid(string couponCode, User user)
        {
            var discount = context.Discounts
               .Where(p => p.CouponCode.Equals(couponCode)).FirstOrDefault();

            if (discount == null)
            {
                return new BaseDto(IsSuccess: false,
                    Message: new List<string> { "کد تخفیف معتبر نمی باشد" });
            }

            var now = DateTime.Now;
            if (discount.StartDate.HasValue)
            {
                var startDate = DateTime.SpecifyKind(discount.StartDate.Value, DateTimeKind.Utc);
                if(startDate.CompareTo(now) > 0)
                {
                    if (startDate.CompareTo(now) > 0)
                        return new BaseDto(false, new List<string>
                    { "هنوز زمان استفاده از این کد تخفیف فرا نرسیده است" });
                }
            }
            if (discount.EndDate.HasValue)
            {
                var endDate = DateTime.SpecifyKind(discount.EndDate.Value, DateTimeKind.Utc);
                if (endDate.CompareTo(now) < 0)
                    return new BaseDto(false, new List<string> { "کد تخفیف منقضی شده است" });
            }

            var checkLimit = CheckDiscountLimitations(discount, user);

            if (checkLimit.IsSuccess == false)
                return checkLimit;

            return new BaseDto(true, null);

        }

        private BaseDto CheckDiscountLimitations(Discount discount, User user)
        {
            switch (discount.discountLimitation)
            {
                case DiscountLimitationType.Unlimited:
                    {
                        return new BaseDto(true, null);
                    }

                case DiscountLimitationType.NTimesOnly:
                    {
                        var totalUsage = discountHistoryService.GetAllDiscountUsageHistory(discount.Id, null, 0, 1).Data.Count();

                        if (totalUsage < discount.LimitationTimes)
                            return new BaseDto(true, null);
                        else
                            return new BaseDto(false, new List<string> { "تعدادی که شما مجاز به استفاده از این تخفیف بوده اید به پایان رسیده است" });
                    }
                case DiscountLimitationType.NTimesPerCustomer:
                    {
                        if (user != null)
                        {
                            var totalUsage = discountHistoryService.GetAllDiscountUsageHistory(discount.Id, user.Id, 0, 1).Data.Count();

                            if (totalUsage < discount.LimitationTimes)
                                return new BaseDto(true, null);
                            else
                                return new BaseDto(false, new List<string> { "تعدادی که شما مجاز به استفاده از این تخفیف بوده اید به پایان رسیده است" });

                        }
                        else
                            return new BaseDto(false, new List<string> { "برای استفاده از این کد تخفیف باید وارد شوید" });
                    }
                    
                default:
                    break;
            }
            return new BaseDto(true, null);
        }


        public bool RemoveDiscountFromBasket(int BasketId)
        {
            var basket = context.Baskets.Find(BasketId);

            basket.RemoveDescount();
            context.SaveChanges();
            return true;
        }

        public string GetCoponCode(int BasketId)
        {
            var baskt = context.Baskets
                .Include(p => p.AppliedDiscount)
                .FirstOrDefault(p => p.Id == BasketId);
            string coponCode = baskt?.AppliedDiscount?.CouponCode;
            return coponCode;
        }
    }

    public class CatlogItemDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
