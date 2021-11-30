using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using TodoStudy.Exceptions.Enums;

namespace TodoStudy.Exceptions
{
    public class BadRequestException : HttpException
    {
       public override HttpStatusCode Code => HttpStatusCode.BadRequest;

        public BadRequestException(string message) : base(message)
        {
        }
        public BadRequestException(ExceptionCode exceptionCode, string message) : base(exceptionCode, message)
        {
        }
    }
}
