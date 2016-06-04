using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.scripts.Model
{
    public delegate void MissileFiredHandler(int id);
    public class MissileController : MonoBehaviour
    {
        public event MissileFiredHandler MissileFired;
        private IList<GameObject> missiles;
        private IDictionary<Player_ID, GameObject> visibleTargets;
        private Camera mainCamera;
        private RectTransform canvas;
        public void Initialize()
        {
            missiles = new List<GameObject>();
            visibleTargets = new Dictionary<Player_ID, GameObject>();
            mainCamera = Tags.FindGameObjectWithTagInParent(Tags.MainCamera, transform.root.name).GetComponent<Camera>();
            canvas = GameObject.FindGameObjectWithTag(Tags.Canvas).GetComponent<RectTransform>();
            missiles.Add(GameObject.FindGameObjectWithTag(Tags.MisilleLeft1));
            missiles.Add(GameObject.FindGameObjectWithTag(Tags.MisilleLeft2));
            missiles.Add(GameObject.FindGameObjectWithTag(Tags.MisilleRight1));
            missiles.Add(GameObject.FindGameObjectWithTag(Tags.MisilleRight2));
        }

        public void OnEnemyBecameVisible(Player_ID playerId)
        {
            GameObject aim = (GameObject)Instantiate(Resources.Load("Aim"));
            aim.name = "aim" + playerId.Id;
            aim.transform.SetParent(canvas.transform);
            visibleTargets.Add(new KeyValuePair<Player_ID, GameObject>(playerId, aim));
        }

        void Update()
        {
            foreach (var visibleTarget in visibleTargets)
            {
                Transform target = visibleTarget.Key.transform;
                Vector3 screenPosition = mainCamera.WorldToScreenPoint(target.position);
                var reference = canvas.rect.size;
                float sw = Screen.width;
                float sh = Screen.height;
                reference = new Vector2(reference.x / sw, reference.y / sh);
                var rT = visibleTarget.Value.GetComponent<RectTransform>();
                rT.anchoredPosition = new Vector2(screenPosition.x * reference.x - rT.rect.width / 2, 
                    screenPosition.y * reference.y - rT.rect.height / 2);
                rT.localScale = Vector3.one;
            }
        }

        public void OnEnemyBecameInvisible(Player_ID playerId)
        {
            Destroy(visibleTargets[playerId]);
            visibleTargets.Remove(playerId);
        }

        public void Shoot()
        {
            if (visibleTargets.Count > 0)
            {
                Player_ID target = findClosest();
                if (MissileFired == null)
                    Debug.LogWarning("Unsubscribed MissileFired");
                else
                    MissileFired.Invoke(target.ServerAssignedId);
            }
        }

        private Player_ID findClosest()
        {
            float closestDist = (float)Double.MaxValue;
            Player_ID target = null;
            foreach (var visibleTarget in visibleTargets.Keys)
            {
                float dist = Vector3.Distance(visibleTarget.transform.position, transform.position);
                if (dist < closestDist)
                {
                    closestDist = dist;
                    target = visibleTarget;
                }
            }
            return target;
        }


    }
}
