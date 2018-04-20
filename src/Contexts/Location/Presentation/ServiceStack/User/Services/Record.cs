using System;
using System.Collections.Generic;
using System.Text;
using Infrastructure.ServiceStack;
using ServiceStack;

namespace eShop.Location.User.Services
{
    [Api("Location")]
    [Route("/location/{LocationId}/record", "POST")]
    public class RecordLocation : DomainCommand
    {
        public Guid RecordId { get; set; }

        public Guid LocationId { get; set; }
        public Guid UserId { get; set; }
    }
}
