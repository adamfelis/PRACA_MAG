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

        private float targetPositionX;
        private float targetPositionY;
        private float targetPositionZ;

        private float currentPositionX;
        private float currentPositionY;
        private float currentPositionZ;

        private float prevTheta;
        private float prevPhi;
        private float prevPsi;

        private float currentTheta;
        private float currentPhi;
        private float currentPsi;

        private float prevPositionX;
        private float prevPositionY;
        private float prevPositionZ;

        private float targetVelocityX;
        private float targetVelocityY;
        private float targetVelocityZ;

        private Vector3 currentVelocity;
        private Vector3 previousVelocity;

        private float globalIterationCounter;
        private bool interpolationActive = false;
        private float wholeInterpolationTime = Time.fixedDeltaTime;

        private SceneController sceneController;

        public GameObject Body;

        public AircraftInterpolator(GameObject Body, SceneController sceneController)
        {
            this.Body = Body;
            this.sceneController = sceneController;
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

        public float TargetPositionX
        {
            get { return targetPositionX; }
            set
            {
                prevPositionX = targetPositionX;
                currentPositionX = targetPositionX;
                targetPositionX = value;
            }
        }

        public float TargetPositionY
        {
            get { return targetPositionY; }
            set
            {
                prevPositionY = targetPositionY;
                currentPositionY = targetPositionY;
                targetPositionY = value;
            }
        }

        public float TargetPositionZ
        {
            get { return targetPositionZ; }
            set
            {
                prevPositionZ = targetPositionZ;
                currentPositionZ = targetPositionZ;
                targetPositionZ = value;
            }
        }

        public Vector3 CurrentVelocity
        {
            get { return currentVelocity; }
        }

        public float TargetVelocityX
        {
            get { return targetVelocityX; }
            set
            {
                previousVelocity.x = targetVelocityX;
                targetVelocityX = value;
            }
        }

        public float TargetVelocityY
        {
            get { return targetVelocityY; }
            set
            {
                previousVelocity.y = targetVelocityY;
                targetVelocityY = value;
            }
        }

        public float TargetVelocityZ
        {
            get { return targetVelocityZ; }
            set
            {
                previousVelocity.z = targetVelocityZ;
                targetVelocityZ = value;
            }
        }

        public void SetupInitial(float theta0, float phi0, float psi0, Vector3 V0)
        {
            prevTheta = currentTheta = targetTheta = theta0;
            prevPhi = currentPhi = targetPhi = phi0;
            prevPsi = currentPsi = targetPsi = psi0;
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

            previousVelocity = V0;
            targetVelocityX = V0.x;
            targetVelocityY = V0.y;
            targetVelocityZ = V0.z;

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
            //interpolateLateralPosition(t);
            Body.transform.Translate(new Vector3(targetPositionZ - currentPositionZ, 0, 0), Space.Self);
            
            //psi rotation
            var psi = new Vector3(0, targetPsi - currentPsi, 0);
            Body.transform.Rotate(psi);
            //
            //phi rotation
            var phi = new Vector3(0, 0, -(targetPhi - currentPhi));
            Body.transform.Rotate(phi);
            //
            //interpolateLongitudinalPosition(t);
            Body.transform.Translate(new Vector3(0, targetPositionY - currentPositionY, 0), Space.Self);
            Body.transform.Translate(new Vector3(0, 0, targetPositionX - currentPositionX), Space.Self);
            interpolateVelocity(t);
        }

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


            if (sceneController.LateralRotationActive)
                interpolateLateralRotation(iterations);
            if (sceneController.LateralTranslationActive)
                interpolateLateralPosition(t, iterations);

            if (sceneController.LongitudinalRotationActive)
                interpolateLongitudinalRotation(iterations);
            if (sceneController.LongitudinalTranslationActive)
                interpolateLongitudinalPosition(t, iterations);

            interpolateVelocity(t);
        }

        private void interpolateVelocity(float t)
        {
            var targetVelocity = new Vector3(targetVelocityX, targetVelocityY, targetVelocityZ);
            currentVelocity = Vector3.Lerp(previousVelocity, targetVelocity, t);
        }


        private void interpolateLongitudinalRotation(float iterations)
        {
            float delta = TargetTheta - prevTheta;
            delta /= iterations;
            currentTheta += delta;
            var rot = new Vector3(delta, 0, 0);
            Body.transform.Rotate(rot, Space.Self);
        }

        private void interpolateLateralRotation(float iterations)
        {

            float delta = TargetPhi - prevPhi;
            delta /= iterations;
            currentPhi += delta;
            var rot = new Vector3(0, 0, delta);
            Body.transform.Rotate(rot, Space.Self);

            delta = TargetPsi - prevPsi;
            delta /= iterations;
            currentPsi += delta;
            rot = new Vector3(0, delta, 0);
            Body.transform.Rotate(rot, Space.Self);
        }

        private void interpolateLongitudinalPosition(float t, float iterations)
        {
            //float X = Mathf.Lerp(prevPositionX, targetPositionX, t);
            //float Y = Mathf.Lerp(prevPositionY, targetPositionY, t);
            //var position = new Vector3(Body.transform.position.x, Y, -X);
            //Body.transform.position = (position);


            //Body.transform.Translate(velocity * Time.deltaTime, Space.Self);

            float delta = targetPositionX - prevPositionX;
            delta /= iterations;
            currentPositionX += delta;
            //WE PUT MINUS ON PURPOSE, because of model local coordinate system
            Body.transform.Translate(new Vector3(0, 0, -delta), Space.Self);

            delta = targetPositionY - prevPositionY;
            delta /= iterations;
            currentPositionY += delta;
            Body.transform.Translate(new Vector3(0, delta, 0), Space.Self);
        }

        private void interpolateLateralPosition(float t, float iterations)
        {

            //float Z = Mathf.Lerp(prevPositionZ, targetPositionZ, t);
            //var position = new Vector3(Z, Body.transform.position.y, Body.transform.position.z);
            //Body.transform.position = (position);

            //Body.transform.Translate(velocity * Time.deltaTime, Space.Self);


            float delta = targetPositionZ - prevPositionZ;
            delta /= iterations;
            currentPositionZ += delta;
            Body.transform.Translate(new Vector3(delta, 0, 0), Space.Self);
        }
    }
}
