using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Ordering.Controllers
{

    [Route("api/v1/[controller]")]
    [Authorize]
    [ApiController]
    public class BuyerController : ControllerBase
    {
        [HttpPut("{id}/good_standing")]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
        public Task SetGoodStanding(string id)
        {

        }
        [HttpPut("{id}/suspended")]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
        public Task SetSuspended(string id)
        {

        }
        [HttpPut("{id}/address")]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
        public Task SetPreferredAddress(string id)
        {

        }
        [HttpPut("{id}/payment_method")]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
        public Task SetPreferredPaymentMethod(string id)
        {

        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(eShop.Ordering.Buyer.Models.OrderingBuyer), (int)HttpStatusCode.OK)]
        public Task<eShop.Ordering.Buyer.Models.OrderingBuyer> Get(string id)
        {

        }
        [HttpPost("{id}")]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
        public Task Initiate(string id)
        {

        }
        [HttpGet]
        [ProducesResponseType(typeof(global::Infrastructure.Responses.Paged<eShop.Ordering.Buyer.Models.OrderingBuyerIndex>), (int)HttpStatusCode.OK)]
        public Task<global::Infrastructure.Responses.Paged<eShop.Ordering.Buyer.Models.OrderingBuyerIndex>> List()
        {

        }
    }
}
