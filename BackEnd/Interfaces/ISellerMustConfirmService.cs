using BackEnd.Models;

namespace BackEnd.Interfaces
{
    public interface ISellerMustConfirmService
    {
        Task<IEnumerable<SellerMustConfrim>> GetByIdCustomer (string idCustomer);
        Task<SellerMustConfrim> GetById(string id);
        Task AddSellerMustConfirms(SellerMustConfrim sellerMustConfirms);
        Task Confirm (List<SellerMustConfrim> sellerMustConfirms );
        Task<IEnumerable<OrderCustomer>> GetByIdCustomerToOrder(string idCustomer);
    }
}
