using BackEnd.DTOS.OrderCustomer;
using BackEnd.Interfaces;
using BackEnd.Models;
using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BackEnd.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class OrderCustomerController : ControllerBase
    {
        private readonly IOrderCustomerService orderCustomerService;
        private readonly IProductListService productListService;
        private readonly DatabaseContext databaseContext;

        public OrderCustomerController(IOrderCustomerService orderCustomerService , IProductListService productListService , DatabaseContext databaseContext)
        {
            this.orderCustomerService = orderCustomerService;
            this.productListService = productListService;
            this.databaseContext = databaseContext;
        }

        [HttpGet("[action]/{idCustomer}")]
        public async Task<IActionResult> GetAll(string idCustomer)
        {
            return Ok((await orderCustomerService.GetAll(idCustomer)).Select(OrderResponse.FromOrder));
        }
        [HttpGet("[action]/{idOrder}")]
        public async Task<IActionResult> GetOrderById(string idOrder)
        {
            return Ok(OrderResponse.FromOrder(await orderCustomerService.GetByID(idOrder)));
        }

        [HttpGet("[action]/{idCustomer}")]
        public async Task<IActionResult> GetOrdered(string idCustomer)
        {
            return Ok(await orderCustomerService.GetOrdered(idCustomer));
        }
        
        [HttpGet("[action]")]
        public async Task<IActionResult> GetConfirmOrder()
        {
            var result = (await orderCustomerService.GetConfirm()).Select(OrderResponse.FromOrder);
            if (result == null)
            {
                return Ok(new { msg = "ไม่พบข้อมูล" });
            }
            return Ok(new { msg = "OK" , data = result });
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetSucceedOrder()
        {
           var result = (await orderCustomerService.GetSucceedOrder()).Select(OrderResponse.FromOrder);
            if (result == null)
            {
                return Ok(new {msg = "ไม่พบข้อมูล"});
            }
            return Ok(new { msg = "OK" , data = result });
        }


        [HttpPost("[action]")]
        public async Task<IActionResult> AddOrderCustomer([FromForm] OrderCustomerRequest orderCustomerRequest , [FromForm] ProductListOrderRequest productListOrderRequest)
        {
            var orderCustomer = orderCustomerRequest.Adapt<OrderCustomer>();
            await orderCustomerService.AddOrder(orderCustomer , productListOrderRequest);
            return Ok(new { msg = "OK" });
        }

        // บอกว่าแต่ละเดือนเราใช้เงินไปกี่ %
        [HttpPost("[action]")]
        public async Task<IActionResult> GetMoneyUsageReport([FromForm] DateTime dateEnd, [FromForm] DateTime dateStart , [FromForm] string? idCustomer)
        {
           var result = await orderCustomerService.MoneyUsageReport(dateEnd, dateStart , idCustomer);
            if(result == null || result.Count() == 0) return Ok(new {msg = "ไม่พบข้อมูล"});
            return Ok(new { msg = "OK" , data = result });
        }

        // บอกว่าวันนี้มีคนซื้อสินค้าเรากี่ชิ้น
        [HttpPost("[action]")]
        public async Task<IActionResult> GetOrderReportByProductType([FromForm] DateTime dateNew , [FromForm] string idCustomer)
        {
            var result = await orderCustomerService.OrderReportByProductType(dateNew, idCustomer);
            return Ok(new { msg = "OK", data = result , amount = result.Count() });
        }


        // ชำระเงิน
        [HttpPut("[action]")]
        public async Task<IActionResult> PaymentOrder([FromForm] OrderPaymentRequest orderPaymentRequest)
        {
            var result = await orderCustomerService.GetByID(orderPaymentRequest.ID);
            if (result == null) return Ok(new { msg = "ไม่พบข้อมูล" });
            #region จัดการรูปภาพ
            (string erorrMesage, string imageName) = await orderCustomerService.UploadImage(orderPaymentRequest.FormFiles);
            if (!string.IsNullOrEmpty(erorrMesage)) return BadRequest(erorrMesage);
            if (!string.IsNullOrEmpty(imageName))
            {
                if (!string.IsNullOrEmpty(result.ProofOfPayment)) await orderCustomerService.DeleteImage(result.ProofOfPayment);
                result.ProofOfPayment = imageName;
            }
            #endregion
            await orderCustomerService.UpdateOrder(result); ;
            return Ok(new { msg = "OK", data = result });
        }


        // Admin เป็นคนกด
        [HttpPut("[action]")]
        public async Task<IActionResult> ConfirmOrder([FromForm] ConfrimOrderCustomerRequest confrimOrderCustomer)
        {
            List<OrderCustomer> orderCustomers = new List<OrderCustomer>();

            for (int i = 0; i < confrimOrderCustomer.ID.Length; i++)
            {
                var result = (await databaseContext.OrderCustomers.FindAsync(confrimOrderCustomer.ID[i]));
                if (result == null)
                {
                    return Ok(new { msg = "ไม่พบข้อมูล" });
                }
                orderCustomers.Add(result);
            }
          
            var order  = await orderCustomerService.ConfirmOrder(orderCustomers); 
            if(order == null) return Ok(new { msg = "ยังไม่ได้ใส่เลขบัญชี" });
            return Ok(new { msg = "OK"});
        }

        // Admin เป็นคนกด
        [HttpPut("[action]")]
        public async Task<IActionResult> CancelOrder([FromForm] ConfrimOrderCustomerRequest confrimOrderCustomer)
        {
            List<OrderCustomer> orderCustomers = new List<OrderCustomer>();
            for (int i = 0; i < confrimOrderCustomer.ID.Length; i++)
            {
                var result = (await databaseContext.OrderCustomers.FindAsync(confrimOrderCustomer.ID[i]));
                if (result == null)
                {
                    return Ok(new { msg = "ไม่พบข้อมูล" });
                }
                orderCustomers.Add(result);
            }
            var order = await orderCustomerService.CancelOrder(orderCustomers);
            if (order == null) return Ok(new { msg = "ยังไม่ได้ใส่เลขบัญชี" });
            return Ok(new { msg = "OK" });
        }


        [HttpPut("[action]")]
        public async Task<IActionResult> ConfirmOrderForSeller([FromForm] ConfrimOrderCustomerRequest confrimOrderCustomer)
        {
            List<OrderCustomer> orderCustomers = new List<OrderCustomer>();

            for (int i = 0; i < confrimOrderCustomer.ID.Length; i++)
            {
                var result = (await databaseContext.OrderCustomers.FindAsync(confrimOrderCustomer.ID[i]));
                if (result == null)
                {
                    return Ok(new { msg = "ไม่พบข้อมูล" });
                }
                orderCustomers.Add(result);
            }

            await orderCustomerService.ConfirmOrderForSeller(orderCustomers);
            return Ok(new { msg = "OK" });
        }

        [HttpPut("[action]")]
        public async Task<IActionResult> ConfirmOrderForCustomer([FromForm] string ID)
        {
            var order = await orderCustomerService.GetByID(ID);
            if (order == null) return Ok(new { msg = "ไม่พบข้อมูล" });
            await orderCustomerService.ConfirmOrderForCustomer(order);
            return Ok(new { msg = "OK" });
        }
    }
}
