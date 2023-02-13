using BackEnd.Interfaces;
using BackEnd.Models;
using Microsoft.EntityFrameworkCore;

namespace BackEnd.Services
{
    public class DeliveryService : IDeliveryService
    {
        private readonly DatabaseContext databaseContext;

        public DeliveryService(DatabaseContext databaseContext)
        {
            this.databaseContext = databaseContext;
        }
        public async Task Add(Delivery delivery)
        {
            await databaseContext.Deliverys.AddAsync(delivery);
            await databaseContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<Delivery>> GetAll(string searchOrder = "" , string idCustomer = "")
        {
            var idOrder = new List<string>();
            var delivery = new List<Delivery>();
            var productList = await databaseContext.ProductLists.Where(e => e.Product.CustomerID.Equals(idCustomer)).ToListAsync();
            // loop เพื่อเก็บ idOrder
            for (int i = 0; i < productList.Count(); i++)
            {
               var result = idOrder.FirstOrDefault(e => e.Equals(productList[i].OrderID));
                if(result == null)
                {
                    idOrder.Add(productList[i].OrderID);
                }
            }
            if (idOrder.Count() > 0)
            {
                for (int i = 0; i < idOrder.Count(); i++)
                {
                   var result = await databaseContext.Deliverys.Include(e => e.OrderCustomer).Include(e => e.StatusDelivery).AsNoTracking().FirstOrDefaultAsync(e => e.OrderCustomerID == idOrder[i]);
                    if (result != null)
                    {
                        delivery.Add(result);
                    }
                }
            }
            if (!string.IsNullOrEmpty(searchOrder))
            {
                delivery = delivery.Where(e => e.OrderCustomerID == searchOrder).ToList();
            }
            return delivery;
        }

        public async Task<Delivery> GetById(string id)
        {
           var result = await databaseContext.Deliverys.Include(e => e.OrderCustomer).Include(e => e.StatusDelivery).AsNoTracking().FirstOrDefaultAsync(e => e.ID == id);
            return result;

        }

        public async Task<Delivery> GetByIdOrder(string idOrder)
        {
            var result = await databaseContext.Deliverys.Include(e => e.StatusDelivery).AsNoTracking().FirstOrDefaultAsync(e => e.OrderCustomerID == idOrder);
   
            return result;
        }

        public async Task Update(Delivery delivery)
        {
            databaseContext.Update(delivery);
            await databaseContext.SaveChangesAsync();
        }
    }
}
