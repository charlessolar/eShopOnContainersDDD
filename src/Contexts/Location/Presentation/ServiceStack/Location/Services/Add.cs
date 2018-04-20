using System;
using System.Collections.Generic;
using System.Text;
using Infrastructure.Commands;
using Infrastructure.ServiceStack;
using ServiceStack;

namespace eShop.Location.Location.Services
{
    [Api("Location")]
    [Route("/location", "POST")]
    public class AddLocation : DomainCommand
    {
        public Guid LocationId { get; set; }

        public string Code { get; set; }
        public string Description { get; set; }

        public Guid ParentId { get; set; }
    }
}
