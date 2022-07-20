using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Catalog.Controllers
{

    [Route("api/v1/[controller]")]
    [Authorize]
    [ApiController]
    public class CatalogBrandController : ControllerBase
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
        [HttpGet]
        [ProducesResponseType(typeof(global::Infrastructure.Responses.Paged<eShop.Catalog.CatalogBrand.Models.CatalogBrand>), (int)HttpStatusCode.OK)]
        public Task<global::Infrastructure.Responses.Paged<eShop.Catalog.CatalogBrand.Models.CatalogBrand>> List()
        {

        }
    }
}
