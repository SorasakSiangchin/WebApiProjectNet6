using BackEnd.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BackEnd.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class StatusDeliveryController : ControllerBase
    {
        private readonly IStatusDeliveryService statusDeliveryService;

        public StatusDeliveryController(IStatusDeliveryService statusDeliveryService)
        {
            this.statusDeliveryService = statusDeliveryService;
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetStatusDeliveryAll()
        {
           var result= await statusDeliveryService.GetAll();
           return Ok(result);
        }
    }
}
