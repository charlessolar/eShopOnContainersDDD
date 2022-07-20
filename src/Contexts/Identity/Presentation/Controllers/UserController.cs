using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Identity.Controllers
{

    [Route("api/v1/[controller]")]
    [Authorize]
    [ApiController]
    public class UserController : ControllerBase
    {
        [HttpPut("{id}/assign_role")]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
        public Task AssignRole(string id)
        {

        }
        [HttpPut("{id}/revoke_role")]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
        public Task RevokeRole(string id)
        {

        }
        [HttpPut("{id}/name")]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
        public Task ChangeName(string id)
        {

        }
        [HttpPut("{id}/password")]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
        public Task ChangePassword(string id)
        {

        }


        [HttpGet("{id}")]
        [ProducesResponseType(typeof(eShop.Identity.User.Models.User), (int)HttpStatusCode.OK)]
        public Task<eShop.Identity.User.Models.User> Get(string id)
        {

        }

        [HttpPost("{id}")]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
        public Task Register(string id)
        {

        }
        [HttpPost("{id}")]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
        public Task Enable(string id)
        {

        }
        [HttpPost("{id}")]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
        public Task Disable(string id)
        {

        }
        [HttpGet]
        [ProducesResponseType(typeof(global::Infrastructure.Responses.Paged<eShop.Identity.User.Models.User>), (int)HttpStatusCode.OK)]
        public Task<global::Infrastructure.Responses.Paged<eShop.Identity.User.Models.User>> List()
        {

        }
    }
}
