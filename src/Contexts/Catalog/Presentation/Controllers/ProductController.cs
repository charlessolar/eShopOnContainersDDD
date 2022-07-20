using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Catalog.Controllers
{

    [Route("api/v1/[controller]")]
    [Authorize]
    [ApiController]
    public class ProductController : ControllerBase
    {
        [HttpPost("{id}")]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
        public Task Add(string id)
        {

        }

        [HttpGet]
        [ProducesResponseType(typeof(global::Infrastructure.Responses.Paged<eShop.Catalog.Product.Models.CatalogProductIndex>), (int)HttpStatusCode.OK)]
        public Task<global::Infrastructure.Responses.Paged<eShop.Catalog.Product.Models.CatalogProductIndex>> Search()
        {

        }
        [HttpGet]
        [ProducesResponseType(typeof(global::Infrastructure.Responses.Paged<eShop.Catalog.Product.Models.CatalogProductIndex>), (int)HttpStatusCode.OK)]
        public Task<global::Infrastructure.Responses.Paged<eShop.Catalog.Product.Models.CatalogProductIndex>> List()
        {

        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(eShop.Catalog.Product.Models.CatalogProduct), (int)HttpStatusCode.OK)]
        public Task<eShop.Catalog.Product.Models.CatalogProduct> Get(string id)
        {

        }

        [HttpPut("{id}/mark_reordered")]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
        public Task MarkReordered(string id)
        {

        }
        [HttpPut("{id}/unmark_reordered")]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
        public Task UnmarkReordered(string id)
        {

        }

        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
        public Task Remove(string id)
        {

        }
        [HttpPut("{id}/picture")]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
        public Task SetPicture(string id)
        {

        }

        [HttpPut("{id}/description")]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
        public Task SetDescription(string id)
        {

        }
        [HttpPut("{id}/price")]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
        public Task SetPrice(string id)
        {

        }
        [HttpPut("{id}/stock")]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
        public Task SetStock(string id)
        {

        }
    }
}
