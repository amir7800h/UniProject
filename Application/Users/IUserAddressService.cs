using Application.Contexts.Interfaces;
using AutoMapper;
using Domain.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Users
{
    public interface IUserAddressService
    {
        List<UserAddressDto> GetAddress(string userId);
        void AddnewAddress (AddUserAddressDto address);
    }


    public class UserAddressService : IUserAddressService
    {
        private readonly IDataBaseContext context;
        private readonly IMapper mapper;

        public UserAddressService(IDataBaseContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }
        public void AddnewAddress(AddUserAddressDto address)
        {
            var userAddress = mapper.Map<UserAddress>(address);
            //UserAddress userAddress = new UserAddress(address.City, address.State, address.ZipCode,
            //address.PostalAddress, address.UserId, address.ReciverName);
            context.UserAddresses.Add(userAddress);
            context.SaveChanges();
        }

        public List<UserAddressDto> GetAddress(string userId)
        {
            var userAddresses = context.UserAddresses.Where(x => x.UserId == userId).ToList();
            var result = mapper.Map<List<UserAddressDto>>(userAddresses);           
            return result;
            
        }
    }

    public class UserAddressDto
    {
        public int Id { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string PostalAddress { get; set; }
        public string ReciverName { get; set; }
    }

    public class AddUserAddressDto
    {
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string PostalAddress { get; set; }
        public string ReciverName { get;  set; }
        public string UserId { get; set; }
    }
}
