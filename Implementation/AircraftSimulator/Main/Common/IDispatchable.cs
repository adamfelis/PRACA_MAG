﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace Main.Common
{
    public interface IDispatchable
    {
        Dispatcher Dispatcher
        {
            get;
        }
    }
}
