using BackEnd.DTOS.Address;
using BackEnd.Interfaces;
using BackEnd.Models;
using Microsoft.EntityFrameworkCore;

namespace BackEnd.Services
{
    public class AddressService : IAddressService
    {
        private readonly DatabaseContext databaseContext;

        public AddressService(DatabaseContext databaseContext)
        {
            this.databaseContext = databaseContext;
        }
        public async Task CreateAddress(Address address)
        {
            if (string.IsNullOrEmpty(address.ID))
            {
                address.ID = GenerateIDAddress();
            }
            if (address.StatusAddressID == 0)
            {
                var addressStatus = databaseContext.Addresses.Where(x => x.CustomerID == address.CustomerID).FirstOrDefault();
                if (addressStatus == null)
                {
                    address.StatusAddressID = 1;
                }
                else
                {
                    address.StatusAddressID = 2;
                }
            }
           
            await databaseContext.Addresses.AddAsync(address);
            await databaseContext.SaveChangesAsync();
        }

        public async Task<string> CreateAddressInformation(AddressInformation addressInformation)
        {
            if (string.IsNullOrEmpty(addressInformation.ID))
            {
                addressInformation.ID = await GenerateIDAddressInformation();
            }
            await databaseContext.AddressInformations.AddAsync(addressInformation);
            await databaseContext.SaveChangesAsync();
            return addressInformation.ID;
        }

        public async Task<IEnumerable<Address>> GetAll(string idCustomer)
        {
            return await databaseContext.Addresses.Include(e => e.AddressInformation).Include(e => e.StatusAddress).Include(e => e.Customer).Where(e => e.CustomerID == idCustomer).ToListAsync();
        }

        public Task<Address> GetByID(string idAddress)
        {
            throw new NotImplementedException();
        }

        public async Task<string> GenerateIDAddressInformation()
        {
            Random randomNumber = new Random();
            string IdProductListr = "";
            // while คือ roobที่ไมมีที่สิ้นสุดจนกว่าเราจะสั่งให้หยุด
            while (true)
            {
                int num = randomNumber.Next(1000000);

                IdProductListr = DateTime.Now.ToString("yyyy-MM-ddTHH-mm-ss") + "-" + num;

                var result = await databaseContext.AddressInformations.SingleOrDefaultAsync(x => x.ID == IdProductListr);

                if (result == null)
                {
                    break;
                };
            }
            return IdProductListr;
        }

        public string GenerateIDAddress()
        {
            var result = databaseContext.Addresses.OrderByDescending(a => a.ID).FirstOrDefault();
            if (result != null)
            {
                int ID = Convert.ToInt32(result.ID);
                return (ID + 1).ToString();
            }
            return "1";
        }

        public async Task EditAddressStatus(Address address)
        {
            var result = await databaseContext.Addresses.Where(e => e.CustomerID == address.CustomerID && e.ID != address.ID).ToListAsync();
            if (result != null)
            {
                for (int i = 0; i < result.Count; i++)
                {
                    result[i].StatusAddressID = 2;
                    
                }
               
            }
           
            databaseContext.UpdateRange(result);
            databaseContext.Update(address);
            await databaseContext.SaveChangesAsync();
        }
    }
}
