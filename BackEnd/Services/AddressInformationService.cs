using BackEnd.Interfaces;
using BackEnd.Models;

namespace BackEnd.Services
{
    public class AddressInformationService : IAddressInformationService
    {
        private readonly DatabaseContext databaseContext;

        public AddressInformationService(DatabaseContext databaseContext)
        {
            this.databaseContext = databaseContext;
        }

        public async Task<AddressInformation> GetById(string id)
        {
          var result = await databaseContext.AddressInformations.FindAsync(id);
          return result;    
        }
    }
}
