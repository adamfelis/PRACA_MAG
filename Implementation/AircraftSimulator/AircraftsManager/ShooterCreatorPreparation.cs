using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AircraftsManager
{
    public partial class AircraftsManager
    {
        private void PrepareShooterCreator(Shooters.ShooterType shooterType)
        {
            switch (shooterType)
            {
                case Shooters.ShooterType.F16:
                case Shooters.ShooterType.F17:
                    instance.shooterCreator = new Aircraft.Creator.AircraftCreator();
                    break;
                default:
                    throw new Common.InvalidShooterTypeException();
            }
        }
    }
}
