using System;
using System.Collections.Generic;
using System.Text;

namespace Common.VNextFramework
{
    public class BCChinaException : Exception
    {
        public BCChinaException()
        {
        }

        public BCChinaException(string message)
            : base(message)
        {
        }

        public BCChinaException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
