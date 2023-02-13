using BackEnd.Models;

namespace BackEnd.Interfaces
{
    public interface IAddressInformationService
    {
        Task<AddressInformation> GetById (string id);
    }
}
