﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Containers;

namespace Client.Priveleges
{
    public interface IClientInputPriveleges
    {
        string SendDataRequest(IData data);
    }
}
