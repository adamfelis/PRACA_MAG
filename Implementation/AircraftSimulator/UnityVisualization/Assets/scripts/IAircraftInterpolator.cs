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

        void Interpolate(float singleIterationTime, float wholeInterpolationTime);
    }
}
