using BackEnd.DTOS.Product;
using BackEnd.DTOS.ProofOfPaymentCancel;
using BackEnd.Interfaces;
using BackEnd.Models;
using BackEnd.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BackEnd.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ProofOfPaymentCancelController : ControllerBase
    {
        private readonly IProofOfPaymentCancelService proofOfPaymentCancel;

        public ProofOfPaymentCancelController(IProofOfPaymentCancelService proofOfPaymentCancel)
        {
            this.proofOfPaymentCancel = proofOfPaymentCancel;
        }

        [HttpGet("[action]/{idOrder}")]
        public async Task<IActionResult> GetProofOfPaymentCancelByIdOrder(string idOrder)
        {
            return Ok((await proofOfPaymentCancel.GetByIdOrder(idOrder)).Select(ProofOfPaymentCancelResponse.FromProofOfPaymentCancel));
        }
    }
}
