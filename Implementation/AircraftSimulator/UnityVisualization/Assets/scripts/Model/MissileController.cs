using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.scripts.Model
{
    class MissileComponents
    {
        public GameObject Body { get; set; }

        public Vector3 TargetPos
        {
            get { return targetPos; }
            set
            {
                prevPos = targetPos;
                targetPos = value;
            }
        }

        public Vector3 PrevPos
        {
            get { return prevPos; }
        }
        private Vector3 targetPos;
        private Vector3 prevPos;
    }

    public delegate void MissileFiredHandler(int shooterId, int targetId, int missileId);
    public class MissileController : MonoBehaviour
    {
        public event MissileFiredHandler MissileFired;
        public event MissileFiredHandler MissileResponseHandler;
        private IDictionary<int, MissileComponents> missiles;
        private IList<MissileComponents> onStock;
        private IDictionary<Player_ID, GameObject> visibleTargets;
        private Camera mainCamera;
        private RectTransform canvas;
        private Player_ID shooter;
        private float elapsedTime;
        public void Initialize()
        {
            missiles = new Dictionary<int, MissileComponents>();
            visibleTargets = new Dictionary<Player_ID, GameObject>();
            mainCamera = Tags.FindGameObjectWithTagInParent(Tags.MainCamera, transform.root.name).GetComponent<Camera>();
            canvas = GameObject.FindGameObjectWithTag(Tags.Canvas).GetComponent<RectTransform>();
            shooter = GetComponent<Player_ID>();
            missiles.Add(0, new MissileComponents() { Body = GameObject.FindGameObjectWithTag(Tags.MisilleLeft1) });
            missiles.Add(1, new MissileComponents() { Body = GameObject.FindGameObjectWithTag(Tags.MisilleLeft2) });
            missiles.Add(2, new MissileComponents() { Body = GameObject.FindGameObjectWithTag(Tags.MisilleRight1) });
            missiles.Add(3, new MissileComponents() { Body = GameObject.FindGameObjectWithTag(Tags.MisilleRight2) });
            onStock = missiles.Values.ToList();
            foreach (var missile in missiles.Values)
            {
                missile.Body.GetComponent<Player_SyncPosition>().enabled = true;
                missile.Body.GetComponent<Player_SyncRotation>().enabled = true;
            }
        }

        public void UpdateMissilePosition(int missileId, int targetId, Vector3 position)
        {
            missiles[missileId].TargetPos = position;
            elapsedTime = 0.0f;
            var targetPlayer = shooter.GetPlayerByRemoteAssignedId(targetId);
            var dist = Vector3.Distance(position, targetPlayer.transform.position);
            var distanceToHit = 10.0f;
            if (dist < distanceToHit)
            {
                Debug.Log("AIRCRAFT HIT");
            }
            else
            {
                MissileResponseHandler.Invoke(shooter.ServerAssignedId, targetId, missileId);
            }
        }

        private int getMissileId(GameObject missile)
        {
            int i = 0;
            foreach (var value in missiles.Values)
            {
                if (value.Body == missile)
                    return i;
                i++;
            }
            return -1;
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
            interpolateMissiles();
        }

        void interpolateMissiles()
        {
            float globalAnimationTime = Time.fixedDeltaTime;
            elapsedTime += Time.deltaTime;
            float t = elapsedTime/globalAnimationTime;
            foreach (var missileComponent in missiles.Values)
            {
                missileComponent.Body.transform.position = Vector3.Lerp(missileComponent.PrevPos, missileComponent.TargetPos, t);
            }
        }

        public void OnEnemyBecameInvisible(Player_ID playerId)
        {
            Destroy(visibleTargets[playerId]);
            visibleTargets.Remove(playerId);
        }

        public void Shoot()
        {
            if (visibleTargets.Count > 0 && onStock.Count > 0)
            {
                Player_ID target = findClosest();

                if (MissileFired == null)
                    Debug.LogWarning("Unsubscribed MissileFired");
                else
                {
                    var firedMissile = onStock.First();
                    MissileFired.Invoke(shooter.ServerAssignedId, target.ServerAssignedId,
                        getMissileId(firedMissile.Body));
                    onStock.Remove(firedMissile);
                }
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
