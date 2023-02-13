using BackEnd.DTOS.SellerMustConfirm;
using BackEnd.Interfaces;
using BackEnd.Models;
using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BackEnd.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class SellerMustConfirmController : ControllerBase
    {
        private readonly ISellerMustConfirmService sellerMustConfirmService;

        public SellerMustConfirmController(ISellerMustConfirmService sellerMustConfirmService)
        {
            this.sellerMustConfirmService = sellerMustConfirmService;
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetSellerMustConfirmByIdCustomer(string idCustomer)
        {
           var result = await sellerMustConfirmService.GetByIdCustomer(idCustomer);  
            if(result?.Count() == 0)
            {
                return Ok(new { msg = "ไม่พบข้อมูล" });
            }
            return Ok(new {msg = "OK" , data = result });
        }
        [HttpGet("[action]")]
        public async Task<IActionResult> GetSellerMustConfirmByIdCustomerToOrder(string idCustomer)
        {
            var result = await sellerMustConfirmService.GetByIdCustomerToOrder(idCustomer);
            if (result?.Count() == 0)
            {
                return Ok(new { msg = "ไม่พบข้อมูล" });
            }
            return Ok(new { msg = "OK", data = result });
        }

        [HttpPut("[action]")]
        public async Task<IActionResult> Confirm ([FromForm] SellerMustConfrimRequest sellerMustConfrimRequest )
        {
            List<SellerMustConfrim> sellerMustConfirm = new List<SellerMustConfrim>();

            for (int i = 0; i < sellerMustConfrimRequest.ID.Length; i++)
            {
                var result = (await sellerMustConfirmService.GetById(sellerMustConfrimRequest.ID[i])).Adapt<SellerMustConfrim>();
                if (result == null)
                {
                    return Ok(new { msg = $"ไม่พบข้อมูลรหัสที่ : {sellerMustConfrimRequest.ID[i]}" });
                }
                sellerMustConfirm.Add(result);
            }

            await sellerMustConfirmService.Confirm(sellerMustConfirm);
            return Ok(new { msg = "OK" });
        }
    }
}
