using System;
using System.Collections.Generic;
using System.Text;
using Infrastructure.Commands;
using Infrastructure.ServiceStack;
using ServiceStack;

namespace eShop.Location.Location.Services
{
    [Api("Location")]
    [Route("/location/{LocationId}", "DELETE")]
    public class RemoveLocation : DomainCommand
    {
        public Guid LocationId { get; set; }
    }
}
