using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.scripts
{
    //public enum AircraftPart
    //{
    //    RudderLeft,
    //    RudderRight,
    //    ElevatorLeft,
    //    ElevatorRight,
    //    AileronLeft,
    //    AileronRight
    //}
    public interface IAircraft
    {
        void RotateAircraft(float vertical, float horizontal);
    }
}
