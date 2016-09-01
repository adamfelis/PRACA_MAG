using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.AircraftData;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.scripts.Model
{
    public class MissileComponents
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

        public GameObject TargetGameObject { get; set; }
        public GameObject ShooterGameObject { get; set; }

        private Vector3 targetPos;
        private Vector3 prevPos;
        public ParticleRenderer ParticleRenderer { get; set; }
        public MeshRenderer MeshRenderer { get; set; }
        public Light PointLight { get; set; }
        public int TargetId { get; set; }
        public int ShooterId { get; set; }
        public int MissileId { get; set; }
        public Vector3 offset { get; set; }
        public Quaternion rotation { get; set; }
        public bool RequestPosition { get; set; }
        public bool IsTriggered { get; set; }
    }

    public delegate void MissileFiredHandler(MissileData missileData);
    public class MissileController : MonoBehaviour
    {
        public event MissileFiredHandler MissileFired;
        public event MissileFiredHandler MissileResponseHandler;
        public IDictionary<int, MissileComponents> missiles;
        public IList<MissileComponents> onStock;
        private IDictionary<Player_ID, GameObject> visibleTargets;
        private Camera mainCamera;
        private RectTransform canvas;
        private Player_ID shooter;
        private float elapsedTime;
        private Missiles_SyncPosition missilesSyncPosition;
        public void Initialize()
        { 
            missiles = new Dictionary<int, MissileComponents>();
            visibleTargets = new Dictionary<Player_ID, GameObject>();
            canvas = GameObject.FindGameObjectWithTag(Tags.Canvas).GetComponent<RectTransform>();
            shooter = GetComponent<Player_ID>();
            if (shooter.isLocalPlayer)
                mainCamera = Tags.FindGameObjectWithTagInParent(Tags.MainCamera, transform.root.name).GetComponent<Camera>();
            missiles.Add(0, new MissileComponents() { Body = Tags.FindGameObjectWithTagInParent(Tags.MisilleLeft1, name), });
            missiles.Add(1, new MissileComponents() { Body = Tags.FindGameObjectWithTagInParent(Tags.MisilleLeft2, name) });
            missiles.Add(2, new MissileComponents() { Body = Tags.FindGameObjectWithTagInParent(Tags.MisilleRight1, name) });
            missiles.Add(3, new MissileComponents() { Body = Tags.FindGameObjectWithTagInParent(Tags.MisilleRight2, name) });

            foreach (var missileComponentse in missiles.Values)
            {
                missileComponentse.offset = missileComponentse.Body.transform.localPosition;
                missileComponentse.rotation = missileComponentse.Body.transform.rotation;
                //missileComponentse.ParticleRenderer = missileComponentse.Body.transform.FindChild("Engine").GetComponent<ParticleRenderer>();
                missileComponentse.PointLight = missileComponentse.Body.GetComponent<Light>();
                missileComponentse.MeshRenderer = missileComponentse.Body.transform.FindChild("Sphere").GetComponent<MeshRenderer>();
            }

            onStock = missiles.Values.ToList();
            missilesSyncPosition = gameObject.GetComponent<Missiles_SyncPosition>();
            missilesSyncPosition.Initialize();
        }

        void OnDestroy()
        {

            var playerId = GetComponent<Player_ID>();
            if (!playerId.isLocalPlayer)
            {
                var missileConrtroller = playerId.GetLocalPlayer().GetComponent<MissileController>();
                missileConrtroller.OnEnemyBecameInvisible(playerId);
                foreach (var missileComponentse in missileConrtroller.missiles.Values)
                {
                    if (missileComponentse.TargetId == playerId.ServerAssignedId)
                    {
                        missileComponentse.RequestPosition = false;
                    }  
                }
            }
        }

        public void BeginFly(int shooterId, int targetId, int missileId)
        {
            missilesSyncPosition.CmdBroadcastMissileCreated(shooterId, targetId, missileId);

            missiles[missileId].RequestPosition = true;
        }

        public void UpdateMissilePosition(int missileId, int targetId, Vector3 position)
        {
            if (!missiles[missileId].IsTriggered)
                return;
            //missiles[missileId].TargetPos = position;
            missilesSyncPosition.TransmitPosition(position);
            //Debug.Log(position);

            //missiles[missileId].IsTriggered = true;

            elapsedTime = 0.0f;
            var targetPlayer = shooter.GetPlayerByRemoteAssignedId(targetId);
            var dist = Vector3.Distance(position, targetPlayer.transform.position);
            var distanceToHit = 5.0f;
            if (dist < distanceToHit)
            {
                Debug.Log("AIRCRAFT HIT");
                //missiles[missileId].IsTriggered = false;
                //missiles[missileId].PointLight.enabled = false;
                //missiles[missileId].MeshRenderer.enabled = false;
                missilesSyncPosition.TransmitMissileHit(missiles[missileId]);
                StartCoroutine(enemyHit(missiles[missileId]));
            }
            else
            {
                missiles[missileId].RequestPosition = true; 
                //MissileResponseHandler.Invoke(shooter.ServerAssignedId, targetId, missileId);
            }
        }

        System.Collections.IEnumerator enemyHit(MissileComponents missile)
        {
            if (visibleTargets.ContainsKey(missile.TargetGameObject.GetComponent<Player_ID>()))
            {
                Destroy(visibleTargets[missile.TargetGameObject.GetComponent<Player_ID>()]);
                visibleTargets.Remove(missile.TargetGameObject.GetComponent<Player_ID>());
            }
            GameObject WarningHit = GameObject.FindGameObjectWithTag(Tags.WarningHit);
            toggleWarning(WarningHit, true);
            yield return new WaitForSeconds(3);
            toggleWarning(WarningHit, false);
        }

        void toggleWarning(GameObject panel, bool active)
        {
            panel.GetComponentInChildren<Text>().enabled = active;
            panel.GetComponentInChildren<RawImage>().enabled = active;
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
            if (playerId == null)
                return;
            GameObject aim = (GameObject)Instantiate(Resources.Load("Aim"));
            aim.name = "aim" + playerId.Id;
            aim.transform.SetParent(canvas.transform);
            visibleTargets.Add(new KeyValuePair<Player_ID, GameObject>(playerId, aim));
        }

        void requestMissilesPositions()
        {
            foreach (var pair in missiles)
            {
                var missileComponent = pair.Value;
                if (missileComponent.RequestPosition)
                {
                    var targetPos = missileComponent.TargetGameObject.transform.position;
                    var shooterPos = missileComponent.ShooterGameObject.transform.position;
                    MissileData missileData = new MissileData(shooter.ServerAssignedId, missileComponent.TargetId, pair.Key,
                                                              targetPos.x, targetPos.y, targetPos.z,
                                                               shooterPos.x, shooterPos.y, shooterPos.z);
                    missileComponent.RequestPosition = false;
                    MissileResponseHandler.Invoke(missileData);
                }
            }
        }

        void FixedUpdate()
        {
            if (shooter.isLocalPlayer)
                requestMissilesPositions();
        }

        void Update()
        {
            if (shooter.isLocalPlayer)
            {
                foreach (var visibleTarget in visibleTargets)
                {
                    Transform target = visibleTarget.Key.transform;
                    Vector3 screenPosition = mainCamera.WorldToScreenPoint(target.position);
                    var reference = canvas.rect.size;
                    float sw = Screen.width;
                    float sh = Screen.height;
                    reference = new Vector2(reference.x/sw, reference.y/sh);
                    var rT = visibleTarget.Value.GetComponent<RectTransform>();
                    rT.anchoredPosition = new Vector2(screenPosition.x*reference.x - rT.rect.width/2,
                        screenPosition.y*reference.y - rT.rect.height/2);
                    rT.localScale = Vector3.one;
                }
            }
        }

        public void OnEnemyBecameInvisible(Player_ID playerId)
        {
            if (visibleTargets.ContainsKey(playerId))
            {
                Destroy(visibleTargets[playerId]);
                visibleTargets.Remove(playerId);
            }
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
                    Debug.Log("Aircraft position on shoot: " + transform.position);
                    var firedMissile = onStock.First();
                    int id = 0;
                    foreach (var missileComponent in missiles.Values)
                    {
                        if (missileComponent.Body == firedMissile.Body)
                        {
                            missileComponent.TargetId = target.ServerAssignedId;
                            missileComponent.ShooterId = shooter.ServerAssignedId;
                            missileComponent.MissileId = id;
                            missileComponent.TargetGameObject = target.gameObject;
                            missileComponent.ShooterGameObject = gameObject;
                            //missileComponent.ParticleRenderer.enabled = true;
                            missileComponent.PointLight.enabled = true;
                            missileComponent.MeshRenderer.enabled = true;
                            //GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                            //sphere.transform.s = Vector3.one*10;
                            //sphere.transform.SetParent(missileComponent.Body.transform);
                            var targetPos = missileComponent.TargetGameObject.transform.position;
                            var shooterPos = missileComponent.ShooterGameObject.transform.position;
                            MissileData missileData = new MissileData(shooter.ServerAssignedId, missileComponent.TargetId, id,
                                           targetPos.x, targetPos.y, targetPos.z,
                                            shooterPos.x, shooterPos.y, shooterPos.z);

                            MissileFired.Invoke(missileData);
                        }
                        id++;
                    }
                    //MissileFired.Invoke(shooter.ServerAssignedId, target.ServerAssignedId,
                    //getMissileId(firedMissile.Body));
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
                float dist = Vector3.Distance(visibleTarget.transform.position, shooter.transform.position);
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
