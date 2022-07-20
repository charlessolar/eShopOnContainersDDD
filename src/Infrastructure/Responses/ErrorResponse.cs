using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Responses
{
    public class ErrorResponse
    {
        public string[] Messages { get; set; }

        public object DeveloperMessage { get; set; }
    }
}
