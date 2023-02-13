using BackEnd.DTOS.Customer;
using BackEnd.interfaces;
using BackEnd.Models;
using Mapster;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BackEnd.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly DatabaseContext databaseContext;
        private readonly ICustomerService customerService;

        public CustomerController(DatabaseContext databaseContext, ICustomerService customerService)
        {
            this.databaseContext = databaseContext;
            this.customerService = customerService;
        }

        [HttpGet("[action]")]
        public async Task<ActionResult<IEnumerable<Customer>>> GetCustomerAll(string? searchName = "", int searchRole = 0)
        {
            return Ok((await customerService.GetAll(searchName , searchRole)).Select(CustomerResponse.FromCustomer));
        }

        [HttpGet("[action]/{id}")]
        public async Task<ActionResult<Customer>>GetCustomerByID(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }
            return Ok(CustomerResponse.FromCustomer(await customerService.GetByID(id)));
        }

        [HttpGet("[action]/{id}")]
        public async Task<ActionResult<Customer>> GetShopKeeper(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }
            return Ok(CustomerResponse.FromCustomer(await customerService.GetShopKeeperByID(id)));
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> LoginCustomer([FromForm] LoginRequest loginRequest)
        {
            var result = await customerService.Login(loginRequest.Email, loginRequest.Password);
            if (result == null)
            {
                return Ok(new { msg = "เข้าสู่ระบบไม่สำเร็จ" });
            }
            var token = customerService.GenerateToken(result);
            //return Ok(new { msg = "OK", token = token });
            var resultToken = customerService.GetInfo(token);
            return Ok(new { msg = "OK" , token  = token , resultToken  = resultToken });
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> RegisterCustomer([FromForm] RegisterRequest registerRequest)
        {
            #region จัดการรูปภาพ
            (string erorrMesage, string imageName) = await customerService.UploadImage(registerRequest.FormFiles);
            if (!string.IsNullOrEmpty(erorrMesage)) return BadRequest(erorrMesage);
            #endregion

            var customer = registerRequest.Adapt<Customer>();
            customer.Image = imageName;
            var data = await customerService.Register(customer);
            if(data != null) return Ok(data);
            return Ok(new {msg = "OK" , data = customer });
        }

        [Authorize]
        [HttpGet("[action]")]
        public async Task<ActionResult> Info()
        {
            //อ่านค่า Token (คล้ายๆ การอ่าน session)
            // HttpContext เป็น class ทำการ get token ขึ้นมา
            var customerToken = await HttpContext.GetTokenAsync("access_token");
            if (customerToken == null) return Unauthorized();

            var customer = customerService.GetInfo(customerToken);
            return Ok(new
            {
                email = customer.Email,
                role = customer.Role.Name
            });
        }

        [HttpPut("[action]")]
        public async Task<ActionResult<Product>> UpdateCustomer([FromForm] CustomerRequest customerRequest)
        {
            var customer = await customerService.GetByID(customerRequest.ID);
            if (customer == null)
            {
                return Ok(new { msg = "ไม่พบลูกค้า" });
            }
            #region จัดการรูปภาพ


            (string erorrMesage, string imageName) = await customerService.UploadImage(customerRequest.FormFiles);
            if (!string.IsNullOrEmpty(erorrMesage)) return BadRequest(erorrMesage);
            if (!string.IsNullOrEmpty(imageName))
            {
                await customerService.DeleteImage(customer.Image);
                customer.Image = imageName;
            }
            #endregion
            var result = customerRequest.Adapt(customer);
            await customerService.Update(result);
            return Ok(new { msg = "OK", data = result });
        }
    }
}
