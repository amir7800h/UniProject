using Application.Catalogs.CatalogItems.UriComposer;
using Application.Contexts.Interfaces;
using Application.Discounts;
using Application.Services.Exceptions;
using AutoMapper;
using Domain.Orders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Orders
{
    public interface IOrderService
    {
        int CreateOrder(int BasketId, int UserAddressId, PaymentMethod PaymentMethod);
    }


    public class OrderService : IOrderService
    {
        private readonly IDataBaseContext context;
        private readonly IMapper mapper;
        private readonly IUriComposerService uriComposerService;
        private readonly IDiscountHistoryService discountHistoryService;

        public OrderService(IDataBaseContext context
            , IMapper mapper
            , IUriComposerService uriComposerService
            , IDiscountHistoryService discountHistoryService)
        {
            this.context = context;
            this.mapper = mapper;
            this.uriComposerService = uriComposerService;
            this.discountHistoryService = discountHistoryService;
        }
        public int CreateOrder(int BasketId, int UserAddressId, PaymentMethod PaymentMethod)
        {
            var basket = context.Baskets
                .Include(p=> p.Items)
                .Include(p=> p.AppliedDiscount)
                .SingleOrDefault(b=> b.Id == BasketId);

            if (basket == null)
                throw new NotFoundExceptions(nameof(basket), BasketId);

            int[] ids = basket.Items.Select(b=>b.CatalogItemId).ToArray();
            var catalogItems = context.CatalogItems
                .Include(p => p.CatalogItemImages)
                .Where(p => ids.Contains(p.Id));

            var orderItems = basket.Items.Select(basketItem =>
            {
                var catalogItem = catalogItems.First(c => c.Id == basketItem.CatalogItemId);

                var orderItem = new OrderItem(catalogItem.Id,
                   catalogItem.Name,
                   uriComposerService.ComposeImageUri
                   (catalogItem?.CatalogItemImages?.FirstOrDefault()?.Src ?? ""),
                   catalogItem.Price, basketItem.Quantity);
                return orderItem;

            }).ToList();

            var addressEntity = context.UserAddresses.SingleOrDefault(p => p.Id == UserAddressId);
            Address address = mapper.Map<Address>(addressEntity); 

            var newOrder = new Order(basket.BuyerId, address, orderItems, PaymentMethod);
            context.Orders.Add(newOrder);
            context.SaveChanges();


            
            context.SaveChanges();

            if (basket.AppliedDiscount != null)
            {
                discountHistoryService.InsertDiscountUsageHistory(basket.AppliedDiscount.Id, newOrder.Id);
            }

            return newOrder.Id;
        }
    }

    public class OrderDto
    {
        public string UserId { get; set; }
        public DateTime OrderDate { get;  set; } = DateTime.Now;
        public Address Address { get;  set; }
        public PaymentMethod PaymentMethod { get;  set; }
        public List<OrderItem> orderItems { get;  set; }
    }


}
