using Application.Contexts.Interfaces;
using Application.Dtos;
using Common;
using Domain.Discounts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Discounts
{
    public interface IDiscountHistoryService
    {
        void InsertDiscountUsageHistory(int DiscountId, int OrderId);

        DiscountUsageHistory GetDiscountUsageHistoryById(int discountUsageHistoryId);
        PaginatedItemsDto<DiscountUsageHistory> GetAllDiscountUsageHistory(int? discountId,
        string? userId, int pageIndex, int pageSize);
    }


    public class DiscountHistoryService : IDiscountHistoryService
    {
        private readonly IDataBaseContext context;

        public DiscountHistoryService(IDataBaseContext context)
        {
            this.context = context;
        }

        public PaginatedItemsDto<DiscountUsageHistory> GetAllDiscountUsageHistory(int? discountId, string? userId, int pageIndex, int pageSize)
        {
            var query = context.DiscountUsageHistory.AsQueryable();

            if (discountId.HasValue && discountId.Value > 0)
                query = query.Where(p => p.DiscountId == discountId.Value);

            if (!string.IsNullOrEmpty(userId))
                query = query.Where(p => p.Order != null && p.Order.UserId == userId);

            query = query.OrderByDescending(c => c.CreatedOn);
            var pagedItems = query.PagedResult(pageIndex, pageSize, out int rowCount);
            return new PaginatedItemsDto<DiscountUsageHistory>(pageIndex, pageSize, rowCount, query.ToList());
        }

        public DiscountUsageHistory GetDiscountUsageHistoryById(int discountUsageHistoryId)
        {
            if (discountUsageHistoryId == 0)
                return null;

            var discountUsage = context.DiscountUsageHistory.Find(discountUsageHistoryId);

            return discountUsage;
        }

        public void InsertDiscountUsageHistory(int DiscountId, int OrderId)
        {
            var order = context.Orders.Find(OrderId);
            var discount = context.Discounts.Find(DiscountId);

            DiscountUsageHistory discountUsageHistory = new DiscountUsageHistory()
            {
                CreatedOn = DateTime.Now,
                Discount = discount,
                DiscountId = discount.Id,
                Order = order,
            };

            context.DiscountUsageHistory.Add(discountUsageHistory);
            context.SaveChanges();
        }
    }
}
