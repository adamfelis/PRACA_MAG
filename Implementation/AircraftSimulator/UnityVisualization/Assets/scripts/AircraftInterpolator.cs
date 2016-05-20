using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.scripts
{
    public class AircraftInterpolator: IAircraftInterpolator
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

        private float currentTheta;
        private float currentPhi;
        private float currentPsi;

        private float prevVelocityX;
        private float prevVelocityY;
        private float prevVelocityZ;

        private float globalIterationCounter;
        private bool interpolationActive = false;
        private float wholeInterpolationTime = Time.fixedDeltaTime;

        public GameObject Body;

        public AircraftInterpolator(GameObject Body)
        {
            this.Body = Body;
        }

        public bool InterpolationPending
        {
            get
            {
                return globalIterationCounter <= wholeInterpolationTime;
            }
        }

        public float TargetTheta
        {
            get { return targetTheta; }
            set
            {
                prevTheta = targetTheta;
                currentTheta = targetTheta;
                targetTheta = value;
            }
        }

        public float TargetPhi
        {
            get { return targetPhi; }
            set
            {
                prevPhi = targetPhi;
                currentPhi = targetPhi;
                targetPhi = value;
            }
        }

        public float TargetPsi
        {
            get { return targetPsi; }
            set
            {
                prevPsi= targetPsi;
                currentPsi = targetPsi;
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

        public void SetupInitial(float theta0, float phi0, float psi0)
        {
            prevTheta = currentTheta = targetTheta = theta0;
            prevPhi = currentPhi = targetPhi = phi0;
            prevPsi = currentPsi = targetPsi = theta0;
            globalIterationCounter = wholeInterpolationTime + 1.0f;

            //theta0 rotation
            var rotTheta = new Vector3(theta0, 0, 0);
            Body.transform.Rotate(rotTheta);

            //psi0 rotation
            var rotPsi = new Vector3(0, psi0, 0);
            Body.transform.Rotate(rotPsi);
            //

            //phi0 rotation
            var rotPhi = new Vector3(0, 0, -(phi0));
            Body.transform.Rotate(rotPhi);
            //
        }


        public void LockInterpolation()
        {
            interpolationActive = false;
            //if previous interpolation is not completed
            if (globalIterationCounter < wholeInterpolationTime)
            {
                //Debug.Log("NIE SKONCZYL: " + (wholeInterpolationTime - globalIterationCounter));
                completePreviousIterationImmediately();
            }
            else
            {
                //Debug.Log("SKONCZYL: " + globalIterationCounter);
            }
        }

        public void UnclockInterpolation()
        {
            //reset iteration counter 
            globalIterationCounter = 0.0f;
            interpolationActive = true;
        }

        private void completePreviousIterationImmediately()
        {
            float t = 1.0f;

            //theta rotation
            var theta = new Vector3(targetTheta - currentTheta, 0, 0);
            //Debug.Log(theta);
            Body.transform.Rotate(theta);
            //
            interpolateLateralVelocity(t);

            //psi rotation
            var psi = new Vector3(0, targetPsi - currentPsi, 0);
            Body.transform.Rotate(psi);
            //
            //phi rotation
            var phi = new Vector3(0, 0, -(targetPhi - currentPhi));
            Body.transform.Rotate(phi);
            //
            interpolateLongitudinalVelocity(t);
        }

        private bool coroutineWorking = false;
        public void Interpolate(float singleIterationTime)
        {
            //if new target data are currently being set then wait untill operation completes
            if (!interpolationActive)
                return;

            //if we have already interpolated towards target values then wait for new data portion
            if (!InterpolationPending)
                return;

            float iterations = wholeInterpolationTime / singleIterationTime;
            globalIterationCounter += singleIterationTime;
            float t = globalIterationCounter / wholeInterpolationTime;

            interpolateLateralRotation(iterations);
            interpolateLateralVelocity(t);

            interpolateLongitudinalRotation(iterations);
            interpolateLongitudinalVelocity(t);
        }


        private void interpolateLongitudinalRotation(float iterations)
        {
            float delta = TargetTheta - prevTheta;
            delta /= iterations;
            currentTheta += delta;
            var rot = new Vector3(delta, 0, 0);
            Body.transform.Rotate(rot);
        }

        private void interpolateLateralRotation(float iterations)
        {
            float delta = TargetPhi - prevPhi;
            delta /= iterations;
            currentPhi += delta;
            var rot = new Vector3(0, 0, -delta);
            Body.transform.Rotate(rot);

            delta = TargetPsi - prevPsi;
            delta /= iterations;
            currentPsi += delta;
            rot = new Vector3(0, delta, 0);
            Body.transform.Rotate(rot);
        }

        private void interpolateLongitudinalVelocity(float t)
        {
            float X = Mathf.Lerp(prevVelocityX, TargetVelocityX, t);
            float Y = Mathf.Lerp(prevVelocityY, TargetVelocityY, t);
            var velocity = new Vector3(0, Y, -X);
            Body.transform.Translate(velocity * Time.deltaTime, Space.Self);
        }

        private void interpolateLateralVelocity(float t)
        {
            float Z = Mathf.Lerp(prevVelocityZ, TargetVelocityZ, t);
            var velocity = new Vector3(Z, 0, 0);
            Body.transform.Translate(velocity * Time.deltaTime, Space.Self);
        }
    }
}
