using System;
using System.Collections.Generic;
using System.Text;
using Common.VNextFramework.ExceptionHanding;

namespace Common.VNextFramework
{
    public class BCChinaUnauthorizedException : BCChinaException, IHasHttpStatusCode,IHasErrorCode
    {
        public BCChinaUnauthorizedException():base("Authentication token is not valid.")
        {

        }
        public BCChinaUnauthorizedException(string message) : base(message)
        {
        }

        public BCChinaUnauthorizedException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public int HttpStatusCode { get; } = 401;
        public string Code { get; set; } = "auth_failed";
    }
}
