using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AircraftsManager
{
    public partial class AircraftsManager
    {
        private void PrepareShooterCreator(Shooter.ShooterType shooterType)
        {
            switch (shooterType)
            {
                case Shooter.ShooterType.F16:
                case Shooter.ShooterType.F17:
                    instance.shooterCreator = new Aircraft.Creator.AircraftCreator();
                    break;
                default:
                    throw new Common.InvalidShooterTypeException();
            }
        }
    }
}
