using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.scripts.Model
{
    public delegate void MissileFiredHandler(int id);
    public class MissileController : MonoBehaviour
    {
        public event MissileFiredHandler MissileFired;
        private IList<GameObject> missiles;
        private void Start()
        {
            missiles = new List<GameObject>();
            missiles.Add(GameObject.FindGameObjectWithTag(Tags.MisilleLeft1));
            missiles.Add(GameObject.FindGameObjectWithTag(Tags.MisilleLeft2));
            missiles.Add(GameObject.FindGameObjectWithTag(Tags.MisilleRight1));
            missiles.Add(GameObject.FindGameObjectWithTag(Tags.MisilleRight2));
        }

        public void Shoot()
        {
            if (true)
            {
                MissileFired.Invoke(1);
            }
        }


    }
}
