using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Ordering.Controllers
{
    [Route("api/v1/[controller]")]
    [Authorize]
    [ApiController]
    public class OrderItemController : ControllerBase
    {
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
        public Task Remove(string id)
        {

        }
        [HttpPost("{id}")]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
        public Task Add(string id)
        {

        }
        [HttpPut("{id}/price")]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
        public Task OrderridePrice(string id)
        {

        }
        [HttpGet]
        [ProducesResponseType(typeof(global::Infrastructure.Responses.Paged<eShop.Ordering.Buyer.Entities.Address.Models.Address>), (int)HttpStatusCode.OK)]
        public Task<global::Infrastructure.Responses.Paged<eShop.Ordering.Buyer.Entities.Address.Models.Address>> List()
        {

        }
    }
}
