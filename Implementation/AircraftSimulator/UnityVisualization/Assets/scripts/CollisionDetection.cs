using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.scripts
{
    public class CollisionDetection : MonoBehaviour
    {
        public ICloudCreator CloudCreator { get; set; }
        void OnTriggerEnter(Collider other)
        {
            //Destroy(gameObject);
            //gameObject.transform.Translate(new Vector3(1000, 0, 0));
            var cloudToy = transform.FindChild("CloudsToy Mngr");
            CloudCreator.NotifyCenterChanged(cloudToy.transform);
        }
    }
}
