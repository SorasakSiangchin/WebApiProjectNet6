using BackEnd.DTOS.Delivery;
using BackEnd.Interfaces;
using BackEnd.Models;
using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BackEnd.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class DeliveryController : ControllerBase
    {
        private readonly IDeliveryService deliveryService;

        public DeliveryController(IDeliveryService deliveryService)
        {
            this.deliveryService = deliveryService;
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetDeliveryAll(string? searchIdOrder = "" , string idCustomer = "")
        {
            var result = await deliveryService.GetAll(searchIdOrder , idCustomer);
            if (result == null)
            {
                return Ok(new { msg = "ไม่พบข้อมูล" });
            }
            return Ok(new { msg = "OK", data = result });
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetDeliveryByIdOrder(string idOrder)
        {
            var result = await deliveryService.GetByIdOrder(idOrder );
            if (result == null)
            {
                return Ok(new {msg ="ไม่พบข้อมูล"});
            }
            return Ok(new { msg = "OK", data= result });
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> AddDelivery([FromForm] DeliveryRequest deliveryRequest )
        {
           var data = deliveryRequest.Adapt<Delivery>();
           await deliveryService.Add(data);
            return Ok(new {msg ="OK"});
        }


        [HttpPut("[action]")]
        public async Task<IActionResult> UpdateDelivery([FromForm] DeliveryRequest deliveryRequest)
        {
            var result = await deliveryService.GetById(deliveryRequest.ID);
            if (result==null)
            {
                return Ok(new { msk = "ไม่พบข้อมูล" });
            }
            await deliveryService.Update(deliveryRequest.Adapt<Delivery>());
            return Ok(new { msg = "OK", data = result });
        }
    }
}
