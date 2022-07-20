using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Identity.Controllers
{

    [Route("api/v1/[controller]")]
    [Authorize]
    [ApiController]
    public class RoleController : ControllerBase
    {
        [HttpGet("{id}/activate")]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
        public Task Activate(string id)
        {

        }
        [HttpGet("{id}/deactivate")]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
        public Task Deactivate(string id)
        {

        }
        [HttpGet("{id}/revoke")]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
        public Task Revoke(string id)
        {

        }

        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
        public Task Destroy(string id)
        {

        }
        [HttpPost("{id}")]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
        public Task Define(string id)
        {

        }
    }
}
