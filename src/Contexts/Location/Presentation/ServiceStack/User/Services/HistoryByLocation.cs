using System;
using System.Collections.Generic;
using System.Text;
using Infrastructure.ServiceStack;
using ServiceStack;

namespace eShop.Location.User.Services
{
    [Api("Locations")]
    [Route("/location/{LocationId}/history", "GET")]
    public class HistoryByLocation : Paged<Models.Record>
    {
        public Guid LocationId { get; set; }
    }
}
