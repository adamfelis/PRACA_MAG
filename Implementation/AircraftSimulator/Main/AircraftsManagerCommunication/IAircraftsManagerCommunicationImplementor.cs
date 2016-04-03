using Main.AircraftsManagerCommunication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Main.AircraftsManagerCommunication
{
    public interface IAircraftsManagerCommunicationImplementor
    {
        IAircraftsManagerCommunication AircraftsManagerCommunication
        {
            get;
        }
    }
}
