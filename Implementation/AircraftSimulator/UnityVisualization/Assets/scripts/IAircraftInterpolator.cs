using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.scripts
{
    public interface IAircraftInterpolator
    {

        float TargetTheta { get; set; }
        float TargetPhi { get; set; }
        float TargetPsi { get; set; }

        float TargetVelocityX { get; set; }
        float TargetVelocityY { get; set; }
        float TargetVelocityZ { get; set; }

        bool InterpolationPending { get; }

        void Interpolate(float singleIterationTime);
        //when this metho is called neither of rotation or translastion will be performed
        //untill 'UnlockInterpolation' is called
        void LockInterpolation();
        void UnclockInterpolation();

        void SetupInitial(float theta0, float phi0, float psi0);
    }
}
