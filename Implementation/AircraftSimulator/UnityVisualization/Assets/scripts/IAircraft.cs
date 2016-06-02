using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
namespace Assets.scripts
{
    public interface IAircraft
    {
        void RotateSteersKeyboard(float deltaAileron, float deltaRudder, float deltaElevator);
        void RotateSteersJoystick(float deltaAileron, float deltaRudder, float deltaElevator);
        
        float Theta { get; }
        float Psi { get; }
        float Phi { get; }

        float Eta { get; }
        float Xi { get; }
        float Zeta { get; }

        Vector3 Velocity { get; }
        Vector3 Position { get; }
    }
}
