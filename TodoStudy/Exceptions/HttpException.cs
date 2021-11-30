using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using TodoStudy.Exceptions.Enums;
using Newtonsoft.Json;

namespace TodoStudy.Exceptions
{
    public abstract class HttpException : Exception
    {
        public abstract HttpStatusCode Code { get; }

        public ExceptionCode ExceptionCode { get; }

        public HttpException(string message) : base(message)
        {
        }

        public HttpException(ExceptionCode exceptionCode, string message)
        {
            ExceptionCode = exceptionCode;
        }

        public string GetResponseMessage()
        {
            return JsonConvert.SerializeObject(new
            {
                ExceptionCode = ExceptionCode.ToString(),
                Message,
            });
        }

    }
}
