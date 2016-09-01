using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.scripts
{
    public interface IAircraftInterpolator
    {

        float TargetTheta { get; set; }
        float TargetPhi { get; set; }
        float TargetPsi { get; set; }

       float CurrentTheta { get; }
        
       float CurrentPhi { get; }
        
        float CurrentPsi { get; }


        float TargetPositionX { get; set; }
        float TargetPositionY { get; set; }
        float TargetPositionZ { get; set; }

        Vector3 CurrentVelocity { get; }

        float TargetVelocityX { get; set; }
        float TargetVelocityY { get; set; }
        float TargetVelocityZ { get; set; }

        bool InterpolationPending { get; }

        void Interpolate(float singleIterationTime);
        //when this method is called neither of rotation or translastion will be performed
        //untill 'UnlockInterpolation' is called
        void LockInterpolation();
        void UnclockInterpolation();

        void SetupInitial(float theta0, float phi0, float psi0, Vector3 V0);
    }
}
