using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoStudy.Exceptions.Enums;
using System.Net;

namespace TodoStudy.Exceptions
{
    public class ConflictException: HttpException
    {
        public override HttpStatusCode Code => HttpStatusCode.Conflict;

        public ConflictException(ExceptionCode exceptionCode, string message):base(exceptionCode, message)
        {
        }
    }
}
