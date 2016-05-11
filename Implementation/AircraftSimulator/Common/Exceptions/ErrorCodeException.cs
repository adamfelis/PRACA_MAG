using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Containers;


namespace Common.Exceptions
{
    public class ErrorCodeException : Exception
    {
        public ErrorCode Error { get; set; }
    }
}
