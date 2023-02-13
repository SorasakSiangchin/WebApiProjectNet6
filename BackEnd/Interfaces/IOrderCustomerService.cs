using BackEnd.DTOS.OrderCustomer;
using BackEnd.Models;
using BackEnd.Models.Reposts;

namespace BackEnd.Interfaces
{
    public interface IOrderCustomerService
    {
        Task<IEnumerable<OrderCustomer>> GetAll(string idCustomer);
        Task<OrderCustomer> GetByID(string id);
        // ดึงใบสั่งซื้อที่ส่งหลักฐานและยังไม่ได้ยืนยัน
        Task<IEnumerable<OrderCustomer>> GetConfirm();
        Task<object> CancelOrder(List<OrderCustomer> orderCustomer);
        Task<object> ConfirmOrder(List<OrderCustomer> orderCustomer);
        Task ConfirmOrderForSeller(List<OrderCustomer> orderCustomer);
        Task ConfirmOrderForCustomer(OrderCustomer orderCustomer);
        // ดึงใบสั่งซื้อที่ยืนยันแล้ว
        Task<IEnumerable<OrderCustomer>> GetSucceedOrder();
        Task UpdateOrder(OrderCustomer orderCustomer);
        Task AddOrder(OrderCustomer orderCustomer , ProductListOrderRequest productListOrderRequest);
        // รายงานรายได้แต่ละเดือน
        Task<IEnumerable<double>> MoneyUsageReport(DateTime dateEnd, DateTime dateStart, string idCustomer);
        // รายงานว่าวันนี้เราขายสินค้าได้กี่ชิ้น
        Task<IEnumerable<CategoryProductRepost>> OrderReportByProductType(DateTime dateNew, string idCustomer);
        Task<(string errorMessage, string imageName)> UploadImage(IFormFileCollection formFiles);
        Task DeleteImage(string fileName);
        // ค้นหาใบสั่งซื้อที่มีสินค้าของเราที่ลูกค้าสั่ง
        Task<IEnumerable<OrderCustomer>> GetOrdered(string idCustomer);

        //Task<IEnumerable<OrderCustomer>> GetOrderDelivery (string idCustomer);  
    }
}
