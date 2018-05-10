using System;
using System.Collections.Generic;
using System.Text;
using Infrastructure.ServiceStack;
using ServiceStack;

namespace eShop.Configuration.Setup.Services
{
    [Api("Configuration")]
    [Route("/configuration/status", "GET")]
    public class GetStatus : Query<Models.Status>
    {
    }
}
