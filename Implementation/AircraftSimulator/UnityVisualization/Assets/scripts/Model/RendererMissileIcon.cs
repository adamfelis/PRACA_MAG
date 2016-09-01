using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditorInternal;
using UnityEngine;

namespace Assets.scripts.Model
{
    public delegate void MissileVisibilityChangedHandler(Transform missileTransform);
    public class RendererMissileIcon : MonoBehaviour
    {
        public event MissileVisibilityChangedHandler MissileBecameVisible;
        public event MissileVisibilityChangedHandler MissileBecameInvisible;

        private CameraSmoothFollow cameraSmoothFollow;
        private bool isObjectVisible = false;
        private bool sentInformation = false;
        private Camera radarCamera;
        public Collider anObjCollider;
        private Plane[] planes;
        private bool initialized = false;

        public void Initialize(CameraSmoothFollow cameraSmoothFollow)
        {
            this.cameraSmoothFollow = cameraSmoothFollow;
            radarCamera = cameraSmoothFollow.RadarCamera;
            anObjCollider = transform.root.GetComponent<BoxCollider>();
            initialized = true;
        }

        void FixedUpdate()
        {
            if (initialized)
            {
                isObjectVisible = testObjectVisiblity();
                if (sentInformation != isObjectVisible && cameraSmoothFollow.IsBackCameraOnItsPlace())
                {
                    sentInformation = isObjectVisible;
                    if (isObjectVisible)
                    {
                        Debug.Log("object visible");
                        if (MissileBecameVisible == null)
                            Debug.LogWarning("Enemy Became Visible unsubscribed");
                        else
                            MissileBecameVisible.Invoke(transform);
                    }
                    else
                    {
                        Debug.Log("object INVISIBLE");
                        if (MissileBecameInvisible == null)
                            Debug.LogWarning("Enemy Became INVisible unsubscribed");
                        else
                            MissileBecameInvisible.Invoke(transform);
                    }
                }
            }
            checkVisibility();
        }

        private void checkVisibility()
        {
            if (InputController.BattleMapEnabled)
            {
                
            }
        }

        bool testObjectVisiblity()
        {
            planes = GeometryUtility.CalculateFrustumPlanes(radarCamera);
            if (GeometryUtility.TestPlanesAABB(planes, anObjCollider.bounds))
                return true;
            else
                return false;
        }
    }
}
