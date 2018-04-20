using System;
using System.Collections.Generic;
using System.Text;
using Infrastructure.Commands;
using Infrastructure.ServiceStack;
using ServiceStack;

namespace eShop.Location.Location.Entities.Point.Services
{
    [Api("Location")]
    [Route("/location/{LocationId}/point/{PointId}", "DELETE")]
    public class RemovePoint : DomainCommand
    {
        public Guid LocationId { get; set; }
        public Guid PointId { get; set; }
    }
}
