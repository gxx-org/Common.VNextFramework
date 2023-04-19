using System;
using System.Collections.Generic;
using System.Text;
using Common.VNextFramework.ExceptionHanding;
using Microsoft.Extensions.Logging;

namespace Common.VNextFramework
{
    public class BusinessException : Exception, IHasErrorCode, IHasErrorDetails, IBusinessException
    {
        public string Code { get; set; }

        public string Details { get; set; }

        public BusinessException(
            string message = null,
            string code = null,
            string details = null,
            Exception innerException = null)
            : base(message, innerException)
        {
            Code = code;
            Details = details;
        }

        public BusinessException WithData(string name, object value)
        {
            Data[name] = value;
            return this;
        }
    }
}
