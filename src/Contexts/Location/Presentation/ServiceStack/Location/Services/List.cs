using System;
using System.Collections.Generic;
using System.Text;
using Infrastructure.ServiceStack;
using ServiceStack;

namespace eShop.Location.Location.Services
{
    [Api("Location")]
    [Route("/location", "GET")]
    public class ListLocations : Paged<Models.Location>
    {
    }
}
