using System;
using System.Collections.Generic;
using System.Text;
using Infrastructure.ServiceStack;
using ServiceStack;

namespace eShop.Location.User.Services
{
    [Api("Locations")]
    [Route("/location/{UserId}/locations", "GET")]
    public class HistoryByUser : Paged<Models.Record>
    {
        public Guid UserId { get; set; }
    }
}
