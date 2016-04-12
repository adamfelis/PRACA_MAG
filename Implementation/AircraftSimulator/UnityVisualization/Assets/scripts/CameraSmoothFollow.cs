using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.scripts
{
    public class CameraSmoothFollow : MonoBehaviour
    {

        // The target we are following
        public Transform target;
        // The distance in the x-z plane to the target
        public float distance = 1f;
        // the height we want the camera to be above the target
        public float height = 0f;
        // How much we 
        public float heightDamping = 0.05f;
        public float rotationDamping = 0f;
        private Vector3 offsetPosition;
        private Quaternion offsetRotation;
        private Quaternion inverseoffsetRotation;
        private float distanceTargetCamera;
        private Vector3 cameraOffset = new Vector3(0, 0, 0);
        private float positionFollowFactor = 10;

        void Start()
        {
            offsetPosition = target.position - Camera.main.transform.position;
            offsetRotation = target.rotation;
            distanceTargetCamera = offsetPosition.magnitude;
            //Debug.Log("distance target camera = " + distanceTargetCamera);
        }

        void LateUpdate()
        {
            //-90 rotation is due to axis change from z to x (now plane forward is X)
            Camera.main.transform.rotation = Quaternion.Lerp(Camera.main.transform.rotation, target.rotation * offsetRotation * Quaternion.AngleAxis(-90, Vector3.up),
    Time.deltaTime);
            Vector3 newPosition = target.position + target.forward * distanceTargetCamera + cameraOffset;
            Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, newPosition, Time.fixedDeltaTime * positionFollowFactor);
        }
    }
}
