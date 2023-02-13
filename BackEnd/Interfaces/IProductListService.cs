using BackEnd.Models;

namespace BackEnd.Interfaces
{
    public interface IProductListService
    {
        Task<IEnumerable<ProductList>> GetAll(string idOrder);
        Task<List<ProductList>> GetByIdProduct(string idProduct);
        Task<ProductList> GetById(string id);
        // ดึงสินค้าที่ต้องชำระเงินอยู่ในส่วน Seller
        Task<IEnumerable<ProductList>> GetProductOrdered(string idOrder , string idCustomer);
    }
}
