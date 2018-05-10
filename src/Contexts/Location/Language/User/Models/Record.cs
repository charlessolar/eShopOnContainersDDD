using System;
using System.Collections.Generic;
using System.Text;

namespace eShop.Location.User.Models
{
    public class Record
    {
        public Guid Id { get; set; }

        public Guid LocationId { get; set; }
        public string Code { get; set; }

        public string UserName { get; set; }
        public string Name { get; set; }
    }
}
