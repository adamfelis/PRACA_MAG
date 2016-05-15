using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.scripts
{
    public class AircraftInterpolator : IAircraftInterpolator
    {
        private float targetTheta;
        private float targetPhi;
        private float targetPsi;

        private float targetVelocityX;
        private float targetVelocityY;
        private float targetVelocityZ;

        private float prevTheta;
        private float prevPhi;
        private float prevPsi;

        private float prevVelocityX;
        private float prevVelocityY;
        private float prevVelocityZ;

        private float globalIterationCounter;

        private GameObject Body;

        public AircraftInterpolator(GameObject Body)
        {
            this.Body = Body;
        }

        public float TargetTheta
        {
            get { return targetTheta; }
            set
            {
                prevTheta = targetTheta;
                targetTheta = value;
            }
        }

        public float TargetPhi
        {
            get { return targetPhi; }
            set
            {
                prevPhi = targetPhi;
                targetPhi = value;
            }
        }

        public float TargetPsi
        {
            get { return targetPsi; }
            set
            {
                prevPsi= targetPsi;
                targetPsi = value;
            }
        }

        public float TargetVelocityX
        {
            get { return targetVelocityX; }
            set
            {
                prevVelocityX = targetVelocityX;
                targetVelocityX = value;
            }
        }

        public float TargetVelocityY
        {
            get { return targetVelocityY; }
            set
            {
                prevVelocityY = targetVelocityY;
                targetVelocityY = value;
            }
        }

        public float TargetVelocityZ
        {
            get { return targetVelocityZ; }
            set
            {
                prevVelocityZ = targetVelocityZ;
                targetVelocityZ = value;
            }
        }

        public void Interpolate(float singleIterationTime, float wholeInterpolationTime)
        {
            float iterations = wholeInterpolationTime/singleIterationTime;
            globalIterationCounter += singleIterationTime;
            float t = globalIterationCounter / wholeInterpolationTime;
            float eps = 0.0001f;
            float diff = Mathf.Abs(globalIterationCounter - wholeInterpolationTime);
            if (diff < eps)
                globalIterationCounter = 0.0f;

            interpolateLateralRotation(iterations);
            interpolateLateralVelocity(t);

            interpolateLongitudinalRotation(iterations);
            interpolateLongitudinalVelocity(t);
        }

        private void interpolateLongitudinalRotation(float iterations)
        {
            float delta = TargetTheta - prevTheta;
            delta /= iterations;
            var rot = new Vector3(delta, 0, 0);
            Body.transform.Rotate(rot);
        }

        private void interpolateLateralRotation(float iterations)
        {
            float delta = TargetPhi - prevPhi;
            delta /= iterations;
            var rot = new Vector3(0, 0, -delta);
            Body.transform.Rotate(rot);

            delta = TargetPsi - prevPsi;
            delta /= iterations;
            rot = new Vector3(0, delta, 0);
            Body.transform.Rotate(rot);
        }

        private void interpolateLongitudinalVelocity(float t)
        {
            float X = Mathf.Lerp(prevVelocityX, TargetVelocityX, t);
            float Y = Mathf.Lerp(prevVelocityY, TargetVelocityY, t);
            var velocity = new Vector3(X, Y, 0);
            Body.transform.Translate(velocity * Time.deltaTime, Space.World);
        }

        private void interpolateLateralVelocity(float t)
        {
            float Z = Mathf.Lerp(prevVelocityZ, TargetVelocityZ, t);
            var velocity = new Vector3(0, 0, Z);
            Body.transform.Translate(velocity * Time.deltaTime, Space.World);
        }
    }
}
