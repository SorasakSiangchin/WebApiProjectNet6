using BackEnd.DTOS.CartCustomer;
using BackEnd.Interfaces;
using BackEnd.Models;
using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BackEnd.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CartCustomerController : ControllerBase
    {
        private readonly ICartCustomerService cartCustomerService;

        public CartCustomerController(ICartCustomerService cartCustomerService)
        {
            this.cartCustomerService = cartCustomerService;
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetCartCustomerAll(string idCustomer)
        {
           var data = (await cartCustomerService.GetAll(idCustomer)).Select(CartCustomerResponse.FromCartCustomer);
            return Ok(data);
        }
        [HttpGet("[action]/{id}")]
        public async Task<IActionResult> GetCartCustomerByID(string id)
        {
            var data = CartCustomerResponse.FromCartCustomer(await cartCustomerService.GetByID(id));
            return Ok(data);
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> AddCartCustomer([FromForm] CartCustomerRequest cartCustomerRequest)
        {
            var cartCustomer = cartCustomerRequest.Adapt<CartCustomer>();
            await cartCustomerService.Create(cartCustomer);
            return Ok(new {msg = "OK"});
        }

        [HttpPut("[action]")]
        public async Task<IActionResult> UpdateCartCustomer([FromForm] CartCustomerRequest cartCustomerRequest)
        {
            var cartCustomer = await cartCustomerService.GetByID(cartCustomerRequest.ID);
            if (cartCustomer == null)
            {
                return Ok(new { msg = "ไม่พบสินค้า" });
            }
            var result = cartCustomerRequest.Adapt(cartCustomer);
            await cartCustomerService.Update(result);
            return Ok(new { msg = "OK" });
        }

        [HttpDelete("[action]")]
        public async Task<IActionResult> DeleteCartCustomer(string id)
        {
            var result = await cartCustomerService.GetByID(id);
            if (result == null)
            {
                return Ok(new { msg = "ไม่พบสินค้า" });
            }

            await cartCustomerService.Delete(result);
            return Ok(new { msg = "OK", data = result });
        }
    }
}
