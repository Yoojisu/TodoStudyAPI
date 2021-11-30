using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net;

namespace TodoStudy.Exceptions
{
    public class NotFoundException: HttpException
    {
        public override HttpStatusCode Code => HttpStatusCode.NotFound;

        public NotFoundException(string message) : base(message)
        {

        }
    }
}
