using System;
using System.Collections.Generic;
using System.Text;
using Infrastructure.Commands;

namespace eShop.Location.User.Commands
{
    public class Record : StampedCommand
    {
        public Guid RecordId { get; set; }

        public Guid LocationId { get; set; }
        public Guid UserId { get; set; }
    }
}
