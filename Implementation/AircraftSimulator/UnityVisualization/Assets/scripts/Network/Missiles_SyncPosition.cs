using System.Net.Mime;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.scripts;
using Assets.scripts.Model;
using UnityEngine.Networking;
using UnityEngine.UI;

[NetworkSettings(channel = 1, sendInterval = 0.033f)]
public class Missiles_SyncPosition : NetworkBehaviour
{
    private Vector3 lastPos;
    private float threshold = 5f;
    private Text latencyText;
    private int latency;
    private float globalIterationCounter = 0.0f;
    private MissileController missileController;
    private Transform weapons;
    private bool isInitialized = false;
    private Player_ID playerId;
    public void Initialize()
    {
        playerId = GetComponent<Player_ID>();
        visibleTargets = new Dictionary<Transform, KeyValuePair<GameObject, GameObject>>();
        mainCamera = Tags.FindGameObjectWithTagInParent(Tags.MainCamera, transform.root.name).GetComponent<Camera>();
        canvas = GameObject.FindGameObjectWithTag(Tags.Canvas).GetComponent<RectTransform>();
        missileController = GetComponent<MissileController>();
        weapons = missileController.missiles[0].Body.transform.parent;
        isInitialized = true;
    }

	void Update ()
	{
        if (!isInitialized)
            return;
        LerpPosition();
	    UpdateMissileIconsPosiions();
    }

    private void UpdateMissileIconsPosiions()
    {
        foreach (var visibleTarget in visibleTargets)
        {
            Transform target = visibleTarget.Key.transform;
            Vector3 screenPosition = mainCamera.WorldToScreenPoint(target.position);
            var reference = canvas.rect.size;
            float sw = Screen.width;
            float sh = Screen.height;
            reference = new Vector2(reference.x/sw, reference.y/sh);
            var icon = visibleTarget.Value.Key.GetComponent<RectTransform>();
            icon.anchoredPosition = new Vector2(screenPosition.x*reference.x - icon.rect.width/2,
                screenPosition.y*reference.y - icon.rect.height/2);
            icon.localScale = Vector3.one;
            var missile = visibleTarget.Value.Value.GetComponent<RectTransform>();
            missile.anchoredPosition = icon.anchoredPosition + new Vector2(-93f, 103.1f);
            missile.localScale = Vector3.one;
        }
    }

    void FixedUpdate()
    {
        if (!isInitialized)
            return;
    }

    void LerpPosition()
    {
        float singleIterationTime = Time.deltaTime;
        float wholeInterpolationTime = Time.fixedDeltaTime;
        globalIterationCounter += singleIterationTime;
        float t = globalIterationCounter / wholeInterpolationTime;
        foreach (var missileComponent in missileController.missiles.Values)
        {
            if (missileComponent.IsTriggered)
            {
                missileComponent.Body.transform.position =
                    Vector3.Lerp(missileComponent.PrevPos, missileComponent.TargetPos, t);
                //Debug.Log(missileComponent.Body.transform.position);
            }
        }
    }

    private void addMissileTrail(GameObject gameObject)
    {

        var trailRenderer = (GameObject)GameObject.Instantiate(Resources.Load("TrailRenderer"));
        var tr = trailRenderer.GetComponent<TrailRenderer>();
        //tr.material = Resources.Load("trail_missile", typeof(Material)) as Material;
        tr.material = Resources.Load("missile", typeof(Material)) as Material;
        tr.startWidth = tr.endWidth = 15;
        trailRenderer.name = "TrailRenderer";
        trailRenderer.transform.parent = gameObject.transform;
        trailRenderer.transform.localPosition = Vector3.zero;
    }

    private void enableMissileIcon(MissileComponents missileComponents)
    {
        StartCoroutine(updateMissileTimer(missileComponents));

        var missile = missileComponents.Body;
        if (isLocalPlayer)
        {
            var rendererMissileIcon = missile.AddComponent<RendererMissileIcon>();
            var cameraSmoothFollow = Tags.FindGameObjectWithTagInParent(Tags.CameraManager, name).GetComponent<CameraSmoothFollow>();
            rendererMissileIcon.Initialize(cameraSmoothFollow);
            rendererMissileIcon.MissileBecameVisible += OnMissileBecameVisible;
            rendererMissileIcon.MissileBecameInvisible += OnMissileBecameInvisible;
        }
    }
    private RectTransform canvas;
    private IDictionary<Transform, KeyValuePair<GameObject, GameObject>> visibleTargets;
    private Camera mainCamera;

    public void OnMissileBecameVisible(Transform missileTransform)
    {
        if (playerId == null)
            return;
        GameObject missileIcon = (GameObject)Instantiate(Resources.Load("MissileIcon"));
        GameObject missileInfo = (GameObject)Instantiate(Resources.Load("MissileInfo"));
        missileIcon.name = "missile" + playerId.Id;
        missileIcon.transform.SetParent(canvas.transform);
        missileInfo.name = "missileInfo" + playerId.Id;
        missileInfo.transform.SetParent(canvas.transform);
        visibleTargets.Add(new KeyValuePair<Transform, KeyValuePair<GameObject, GameObject>>
            (missileTransform, new KeyValuePair<GameObject, GameObject>(missileIcon, missileInfo)));
    }

    public void OnMissileBecameInvisible(Transform missileTransform)
    {
        if (visibleTargets.ContainsKey(missileTransform))
        {
            Destroy(visibleTargets[missileTransform].Key);
            Destroy(visibleTargets[missileTransform].Value);
            visibleTargets.Remove(missileTransform);
        }
    }

    private IEnumerator updateMissileTimer(MissileComponents missileComponents)
    {
        float actualTimer = 0.0f;
        float timeToDestroyMissile = 30.0f;
        var shooter = gameObject;
        var target = playerId.GetPlayerByRemoteAssignedId(missileComponents.TargetId);
        var missile = missileComponents.Body;

        while (true)
        {
            actualTimer += Time.deltaTime;
            timeToDestroyMissile -= Time.deltaTime;
            string format = "n2";
            var targetPos = target.transform.position;
            var missilePosition = missile.transform.position;

            if (visibleTargets.ContainsKey(missile.transform))
            {
                var missileInfo = visibleTargets[missile.transform].Value;
                var textComponents = missileInfo.GetComponentsInChildren<Text>();
                var distanceText = textComponents.First(x => x.name == "Distance");
                var timeText = textComponents.First(x => x.name == "Time");

                //timeText.text = "Missile flight time: " + actualTimer.ToString(format) + "s.";
                timeText.text = "Remaining time: " + timeToDestroyMissile.ToString(format) + "s.";
                if (timeToDestroyMissile <= 0f)
                {
                    restartMissile(missileComponents.MissileId);
                    break;
                }

                if (gameObject == shooter)
                    distanceText.text = "Distance to target: " +
                                        Vector3.Distance(targetPos, missilePosition).ToString(format);
                else
                    distanceText.text = "Distance to approaching missile: " +
                                        Vector3.Distance(targetPos, missilePosition).ToString(format);
            }
            yield return 0;

        }
    }

    void toggleWarning(GameObject panel, bool active)
    {
        panel.GetComponentInChildren<Text>().enabled = active;
        panel.GetComponentInChildren<RawImage>().enabled = active;
    }

    IEnumerator unlockChecking()
    {
        yield return new WaitForSeconds(3);
        playerId.GetLocalPlayer().GetComponent<AircraftsController>().IsDestroying = false;
    }

    IEnumerator closeGame()
    {
        yield return new WaitForSeconds(3);
        GameObject.FindGameObjectWithTag(Tags.NetworkManager).GetComponent<CustomNetworkManager>().DisconnectFromServer();
        //GameObject.FindGameObjectWithTag(Tags.ApplicationManager).GetComponent<Communication>().Disconnect();
    }

    [Command]
    void CmdMissileHit(int shooterId, int targetId, int missileId)
    {
        RpcMissileHit(shooterId, targetId, missileId);
    }

    /// <summary>
    /// invoked by shooter
    /// </summary>
    private void restartMissile(int missileId)
    {
        var shooter = gameObject;
        var missileComponents = missileController.missiles[missileId];
        var trailRenderer = missileComponents.Body.transform.FindChild("TrailRenderer");
        if (trailRenderer != null)
            trailRenderer.parent = null;
        missileComponents.Body.transform.SetParent(weapons);
        missileComponents.IsTriggered = false;
        missileComponents.RequestPosition = false;
        missileComponents.PointLight.enabled = false;
        missileComponents.MeshRenderer.enabled = false;
        missileComponents.Body.transform.localPosition = missileController.missiles[missileId].offset;
        missileComponents.Body.transform.localRotation = missileController.missiles[missileId].rotation;
        shooter.GetComponent<MissileController>().onStock.Add(missileComponents);

        if (isLocalPlayer)
        {
            OnMissileBecameInvisible(missileComponents.Body.transform);
            Destroy(missileComponents.Body.GetComponent<RendererMissileIcon>());
        }
    }

    [ClientRpc]
    private void RpcMissileHit(int shooterId, int targetId, int missileId)
    {
        if (playerId.ServerAssignedId == shooterId)
        {
            Debug.Log("I am a shooter, HIT, is local: " + playerId.isLocalPlayer);

        }
        else if (playerId.ServerAssignedId == targetId)
        {
            Debug.Log("I am a target, HIT, is local: " + playerId.isLocalPlayer);
        }
        StopAllCoroutines();
        
        restartMissile(missileId);

        //saving trail renderer
        var target = playerId.GetPlayerByRemoteAssignedId(targetId);
        var trailRendererTarget = target.transform.FindChild("Trail");
        trailRendererTarget.parent = null;

        if (playerId.GetLocalPlayer().GetComponent<Player_ID>().ServerAssignedId == targetId)
        {
            target.GetComponent<AircraftsController>().IsDestroying = true;
            GameObject pullDown = GameObject.FindGameObjectWithTag(Tags.PullDown);
            GameObject pullUp = GameObject.FindGameObjectWithTag(Tags.PullUp);
            toggleWarning(pullDown, false);
            toggleWarning(pullUp, false);

            GameObject warningDestroyed = GameObject.FindGameObjectWithTag(Tags.WarningDestroyed);
            toggleWarning(warningDestroyed, true);
            StartCoroutine(closeGame());
        }
    }

    [Command]
    public void CmdBroadcastMissileCreated(int shooterId, int targetId, int missileId)
    {
        RpcMissileCreated(shooterId, targetId, missileId);
    }

    [ClientRpc]
    public void RpcMissileCreated(int shooterId, int targetId, int missileId)
    {
        var shooter = gameObject;
        var missileComponents = shooter.GetComponent<MissileController>().missiles[missileId];

        missileComponents.Body.transform.parent = null;
        missileComponents.TargetPos = shooter.transform.position;
        missileComponents.TargetPos = shooter.transform.position;
        missileComponents.IsTriggered = true;
        addMissileTrail(missileComponents.Body);

        enableMissileIcon(missileComponents);
    }

    [Command]
    private void CmdBroadcastMissileUpdate(int shooterId, int targetId, int missileId, Vector3 missilePosition)
    {
        RpcUpdateMissile(shooterId, targetId, missileId, missilePosition);
    }

    [ClientRpc]
    public void RpcUpdateMissile(int shooterId, int targetId, int missileId, Vector3 missilePosition)
    {
        var playerId = GetComponent<Player_ID>();
        if (playerId.ServerAssignedId == shooterId)
        {
            Debug.Log("I am a shooter, is local player: "+ playerId.isLocalPlayer);
            globalIterationCounter = 0.0f;
            var shooter = GetComponent<Player_ID>().GetPlayerByRemoteAssignedId(shooterId);
            shooter.GetComponent<MissileController>().missiles[missileId].TargetPos = missilePosition;
        }
        else if (playerId.ServerAssignedId == targetId)
        {
            Debug.Log("I am a target, is local player: " + playerId.isLocalPlayer);
        }

    }
    
    [ClientCallback]
    public void TransmitMissileHit(MissileComponents missileComponents)
    {
        if (isLocalPlayer)
        {
            CmdMissileHit(missileComponents.ShooterId, missileComponents.TargetId, missileComponents.MissileId);
            //CmdProvideTargetIdHit(targetId);
        }
    }

    [ClientCallback]
    public void TransmitPosition(Vector3 missilePosition)
    {
        if (isLocalPlayer)
        {
            for (int i = 0; i < 4; i++)
            {
                if (missileController.missiles[i].IsTriggered)
                    CmdBroadcastMissileUpdate(
                        missileController.missiles[i].ShooterId,
                        missileController.missiles[i].TargetId,
                        i,
                        missilePosition);
            }
            

        }
    }
}
