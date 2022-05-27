using Domain.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTest.Builders
{
    internal class OrderBuilder
    {
        private Order _order;
        public string TestBuyerId => "58968";
        public int TestCatalogItemId => 234;
        public string TestProductName => "Test Product Name";
        public string TestPictureUri => "https://dkstatics-public.digikala.com/digikala-products/c2551f6698b83be20d9c6886fb5205a4a975a208_1621429935.jpg?x-oss-process=image/resize,m_lfit,h_800,w_800/quality,q_90";
        public int TestUnitPrice = 1000;
        public int TestUnits = 3;


        public Order CreateOrderWithDefaultValues()
        {
            List<OrderItem> testOrderItem = new List<OrderItem>()
            {
                new OrderItem(TestCatalogItemId,TestProductName,TestPictureUri,TestUnitPrice,TestUnits)
            };

            return new Order(TestBuyerId, new AddressBuilder().Build(), testOrderItem, PaymentMethod.OnlinePaymnt);
        }
    }
}
