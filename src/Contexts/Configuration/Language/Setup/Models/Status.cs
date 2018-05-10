using System;
using System.Collections.Generic;
using System.Text;

namespace eShop.Configuration.Setup.Models
{
    public class Status
    {
        // needed so mongo doesnt generate random id
        public string Id { get; set; }
        public bool IsSetup { get; set; }
    }
}
