using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Payment.Controllers
{

    [Route("api/v1/[controller]")]
    [Authorize]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        [HttpGet("{id}/claim")]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
        public Task ClaimBasket(string id)
        {

        }
        // DELETE api/values/5
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
        public Task Destroy(string id)
        {

        }
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(eShop.Basket.Basket.Models.Basket), (int)HttpStatusCode.OK)]
        public Task<eShop.Basket.Basket.Models.Basket> Get(string id)
        {

        }
        [HttpPost("{id}")]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
        public Task Initiate(string id)
        {

        }
        [HttpGet]
        [ProducesResponseType(typeof(global::Infrastructure.Responses.Paged<eShop.Basket.Basket.Models.Basket>), (int)HttpStatusCode.OK)]
        public Task<global::Infrastructure.Responses.Paged<eShop.Basket.Basket.Models.Basket>> List()
        {

        }
    }
}
