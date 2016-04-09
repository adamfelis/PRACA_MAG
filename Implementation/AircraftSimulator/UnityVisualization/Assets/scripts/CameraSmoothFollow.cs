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

        void Start()
        {
            offsetPosition = target.position - Camera.main.transform.position;
            offsetRotation = target.rotation;
            distanceTargetCamera = offsetPosition.magnitude;
            //Debug.Log("distance target camera = " + distanceTargetCamera);
        }

        void LateUpdate()
        {
            Vector3 newPosition = target.position + target.forward * distanceTargetCamera;
            Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, newPosition, Time.deltaTime);
            Camera.main.transform.position = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, target.position.z + target.forward.z * distanceTargetCamera);
            //Camera.main.transform.rotation = target.rotation * offsetRotation;
            //Camera.main.transform.position = newPosition;
            Camera.main.transform.rotation = Quaternion.Lerp(Camera.main.transform.rotation, target.rotation * offsetRotation,
                Time.deltaTime);
        }
    }
}
