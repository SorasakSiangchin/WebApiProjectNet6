using BackEnd.DTOS.Address;
using BackEnd.Models;

namespace BackEnd.Interfaces
{
    public interface IAddressService
    {
        Task<IEnumerable<Address>> GetAll(string idCustomer);
        Task<Address> GetByID(string idAddress);
        Task<string> CreateAddressInformation(AddressInformation addressInformation);
        Task EditAddressStatus(Address address);
        Task CreateAddress(Address address);

    }
}
