using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.scripts.Model
{
    public delegate void EnemyVisibilityChangedHandler(Player_ID playerId);
    public class RendererDetector : MonoBehaviour
    {
        public event EnemyVisibilityChangedHandler EnemyBecameVisible;
        public event EnemyVisibilityChangedHandler EnemyBecameInvisible;

        private CameraSmoothFollow cameraSmoothFollow;
        private bool isObjectVisible = false;
        private bool sentInformation = false;
        private Camera mainCamera;
        public Collider anObjCollider;
        private Plane[] planes;
        private bool initialized = false;

        public void Initialize(CameraSmoothFollow cameraSmoothFollow)
        {
            this.cameraSmoothFollow = cameraSmoothFollow;
            mainCamera = cameraSmoothFollow.MainCamera;
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
                        if (EnemyBecameVisible == null)
                            Debug.LogWarning("Enemy Became Visible unsubscribed");
                        else
                            EnemyBecameVisible.Invoke(transform.root.GetComponent<Player_ID>());
                    }
                    else
                    {
                        Debug.Log("object INVISIBLE");
                        if (EnemyBecameInvisible == null)
                            Debug.LogWarning("Enemy Became INVisible unsubscribed");
                        else
                            EnemyBecameInvisible.Invoke(transform.root.GetComponent<Player_ID>());
                    }
                }
            }
        }

        bool testObjectVisiblity()
        {
            planes = GeometryUtility.CalculateFrustumPlanes(mainCamera);
            if (GeometryUtility.TestPlanesAABB(planes, anObjCollider.bounds))
                return true;
            else
                return false;
        }
    }
}
