using System;
using System.Collections.Generic;
using System.Text;
using Infrastructure.Commands;
using Infrastructure.ServiceStack;
using ServiceStack;

namespace eShop.Location.Location.Services
{
    [Api("Location")]
    [Route("/location/{LocationId}/description", "POST")]
    public class UpdateDescriptionLocation : DomainCommand
    {
        public Guid LocationId { get; set; }

        public string Description { get; set; }
    }
}
