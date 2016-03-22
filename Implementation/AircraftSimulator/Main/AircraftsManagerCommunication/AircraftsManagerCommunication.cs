using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationManager.AircraftsManagerCommunication
{
    class AircraftsManagerCommunication : IAircraftsManagerCommunication
    {
        public AircraftsManager.AircraftsManager ManagerInstance
        {
            get
            {
                return AircraftsManager.AircraftsManager.Instance;
            }
        }
    }
}
