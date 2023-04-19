using System;
using System.Collections.Generic;
using System.Text;

namespace Common.VNextFramework.ExceptionHanding
{
    public interface IHasHttpStatusCode
    {
        int HttpStatusCode { get; }
    }
}
