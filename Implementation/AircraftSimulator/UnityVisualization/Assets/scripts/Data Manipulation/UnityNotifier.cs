using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.scripts.Data_Manipulation
{
    public abstract class UnityNotifier
    {
        protected ICommunication unityShellNotifier;
        protected AircraftsController aircraftsController;
    }
}
