using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Ordering.Controllers
{

    [Route("api/v1/[controller]")]
    [Authorize]
    [ApiController]
    public class OrderController : ControllerBase
    {
        [HttpPost("{id}")]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
        public Task Draft(string id)
        {

        }
        [HttpPost("{id}/cancel")]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
        public Task CancelOrder(string id)
        {

        }
        [HttpPost("{id}/confirm")]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
        public Task ConfirmOrder(string id)
        {

        }
        [HttpPost("{id}/pay")]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
        public Task PayOrder(string id)
        {

        }
        [HttpPost("{id}/ship")]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
        public Task ShipOrder(string id)
        {

        }




        [HttpPut("{id}/address")]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
        public Task ChangeAddress(string id)
        {

        }
        [HttpPut("{id}/payment_method")]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
        public Task ChangePaymentMethod(string id)
        {

        }



        [HttpGet("{id}")]
        [ProducesResponseType(typeof(eShop.Ordering.Order.Models.OrderingOrder), (int)HttpStatusCode.OK)]
        public Task<eShop.Ordering.Order.Models.OrderingOrder> Get(string id)
        {

        }
        [HttpGet]
        [ProducesResponseType(typeof(global::Infrastructure.Responses.Paged<eShop.Ordering.Order.Models.OrderingOrderIndex>), (int)HttpStatusCode.OK)]
        public Task<global::Infrastructure.Responses.Paged<eShop.Ordering.Order.Models.OrderingOrderIndex>> List()
        {

        }

        [HttpGet("sales_by_state")]
        [ProducesResponseType(typeof(global::Infrastructure.Responses.Paged<eShop.Ordering.Order.Models.SalesByState>), (int)HttpStatusCode.OK)]
        public Task<global::Infrastructure.Responses.Paged<eShop.Ordering.Order.Models.SalesByState>> SalesByState()
        {

        }
        [HttpGet("sales_week_over_week")]
        [ProducesResponseType(typeof(global::Infrastructure.Responses.Paged<eShop.Ordering.Order.Models.SalesWeekOverWeek>), (int)HttpStatusCode.OK)]
        public Task<global::Infrastructure.Responses.Paged<eShop.Ordering.Order.Models.SalesWeekOverWeek>> SalesWeekOrderWeek()
        {

        }
        [HttpGet("sales")]
        [ProducesResponseType(typeof(global::Infrastructure.Responses.Paged<eShop.Ordering.Order.Models.SalesChart>), (int)HttpStatusCode.OK)]
        public Task<global::Infrastructure.Responses.Paged<eShop.Ordering.Order.Models.SalesChart>> SalesChart()
        {

        }

    }
}
