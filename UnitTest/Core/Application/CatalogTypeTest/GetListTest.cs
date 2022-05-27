using Application.Catalogs.CatalogTypes.CrudService;
using AutoMapper;
using Infrastructure.MappingProfile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnitTest.Builders;
using Xunit;

namespace UnitTest.Core.Application.CatalogTypeTest
{

    public class GetListTest
    {
        /// <summary>
        /// برای اجرای صحیح تست باید بخش 
        /// DataBaseContextSeed.CatalogSeed(builder);
        /// به حالت کامنت در آورد DataBaseContext در
        /// </summary>
        [Fact]
        public void List_should_return_list_of_catalogTypes()
        {
            //Arrange
            var dataBasebuilder = new DatabaseBuilder();
            var dbContext = dataBasebuilder.GetDbContext();

            var mockMapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new CatalogMappingProfile());
            });
            var mapper = mockMapper.CreateMapper();

            var service = new CatalogTypeService(dbContext, mapper);

            service.Add(new CatalogTypeDto
            {
                Id = 1,
                Type = "Samsung"
            });

            service.Add(new CatalogTypeDto
            {
                Id = 2,
                Type = "Phone"
            });

            var result = service.GetList(null, 1, 20);

            //Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.Equal(1, result.Data.FirstOrDefault().Id);

        }
    }
}
