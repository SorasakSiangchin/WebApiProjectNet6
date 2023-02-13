using BackEnd.interfaces;
using BackEnd.Interfaces;
using BackEnd.Models;
using Microsoft.EntityFrameworkCore;

namespace BackEnd.Services
{
    public class CartCustomerService : ICartCustomerService
    {
        private readonly DatabaseContext databaseContext;
        private readonly IUploadFileService uploadFileService;

        public CartCustomerService(DatabaseContext databaseContext , IUploadFileService uploadFileService)
        {
            this.databaseContext = databaseContext;
            this.uploadFileService = uploadFileService;
        }
        public async Task Create(CartCustomer cartCustomer)
        {
            
            var result = await databaseContext.CartCustomers.AsNoTracking().FirstOrDefaultAsync(e => e.ProductID == cartCustomer.ProductID && e.CustomerID == cartCustomer.CustomerID);
            if(result != null)
            {
                result.AmountProduct += cartCustomer.AmountProduct;
                 databaseContext.CartCustomers.Update(result);
                await databaseContext.SaveChangesAsync();
            }
            else
            {
                if (string.IsNullOrEmpty(cartCustomer.ID))
                {
                    cartCustomer.ID = GenerateID();
                }
                await databaseContext.CartCustomers.AddAsync(cartCustomer);
                await databaseContext.SaveChangesAsync();
            }
           
        }

        public async Task Delete(CartCustomer cartCustomer)
        {
            databaseContext.Remove(cartCustomer);
            await databaseContext.SaveChangesAsync();
        }

        public async Task DeleteImage(string fileName)
        {
            await uploadFileService.DeleteImage(fileName);
        }

        public async Task<IEnumerable<CartCustomer>> GetAll(string idCustomer)
        {
            return await databaseContext.CartCustomers.Include(e => e.Product).Include(j => j.Product.CategoryProduct).Where(a => a.CustomerID.Equals(idCustomer)).ToListAsync();
        }

        public async Task<CartCustomer> GetByID(string ID)
        {
            var result = await databaseContext.CartCustomers.Include(e => e.Product).Include(j => j.Product.CategoryProduct).AsNoTracking().FirstOrDefaultAsync(e => e.ID == ID);
            if (result == null)
            {
                throw new Exception("ไม่พบสินค้า");
            }
            return result;
        }

        public async Task Update(CartCustomer cartCustomer)
        {

            databaseContext.Update(cartCustomer);
            await databaseContext.SaveChangesAsync();
        }

        public string GenerateID()
        {
            var result = databaseContext.CartCustomers.OrderByDescending(a => a.ID).FirstOrDefault();
            if (result != null)
            {
                int ID = Convert.ToInt32(result.ID);
                return (ID + 1).ToString();
            }
            return "1";
        }
    }
}
