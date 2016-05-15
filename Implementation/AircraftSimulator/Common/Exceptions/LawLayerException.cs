using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using Common.Containers;

namespace Common.Exceptions
{
    public class LawLayerException : ErrorCodeException
    {
        public Exception Exception { get; set; }
    }
}
