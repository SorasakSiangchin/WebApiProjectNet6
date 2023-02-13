using BackEnd.Interfaces;
using BackEnd.Models;
using Microsoft.EntityFrameworkCore;

namespace BackEnd.Services
{
    public class SellerMustConfirmService : ISellerMustConfirmService
    {
        private readonly DatabaseContext databaseContext;

        public SellerMustConfirmService(DatabaseContext databaseContext)
        {
            this.databaseContext = databaseContext;
        }
        public async Task AddSellerMustConfirms(SellerMustConfrim sellerMustConfirms)
        {
           await databaseContext.SellerMustConfirms.AddAsync(sellerMustConfirms);
           await databaseContext.SaveChangesAsync();
        }

        public async Task Confirm(List<SellerMustConfrim> sellerMustConfirms)
        {
            for (var i = 0; i < sellerMustConfirms.Count(); i++)
            {
                databaseContext.Update(sellerMustConfirms[i]);
                await databaseContext.SaveChangesAsync();
            }
        }

        public async Task<SellerMustConfrim> GetById(string id)
        {
            var result = await databaseContext.SellerMustConfirms.Include(e => e.BankAccount).AsNoTracking().FirstOrDefaultAsync(e => e.ID == id);
            if (result == null)
            {
                return null;
            }
            return result;
        }

        public async Task<IEnumerable<SellerMustConfrim>> GetByIdCustomer(string idCustomer)
        {
          var result = await databaseContext.SellerMustConfirms.Include(e => e.BankAccount).Where(e => e.BankAccount.CustomerID.Equals(idCustomer)).ToListAsync();
            if (result == null)
            {
                return null;
            }
            return result;
        }

        public async Task<IEnumerable<OrderCustomer>> GetByIdCustomerToOrder(string idCustomer)
        {
            var result = await databaseContext.SellerMustConfirms.Include(e => e.BankAccount).Where(e => e.BankAccount.CustomerID.Equals(idCustomer)).ToListAsync();
            if(result == null) return null;
            List<string> idOrder = new List<string>();
            List<OrderCustomer> dataOrder = new List<OrderCustomer>();
            for (int i = 0; i < result.Count(); i++)
            {
                var data = idOrder.FirstOrDefault(e => e == result[i].OrderID);
                if (data == null)
                {
                    idOrder.Add(result[i].OrderID); 
                }
            }
            for (int i = 0; i < idOrder.Count(); i++)
            {
                var data = await databaseContext.OrderCustomers.Include(e => e.Address).Include(e => e.Address.Customer).AsNoTracking().FirstOrDefaultAsync(e => e.ID == idOrder[i]);
                dataOrder.Add(data);
            }
            return dataOrder;
        }
    }
}
