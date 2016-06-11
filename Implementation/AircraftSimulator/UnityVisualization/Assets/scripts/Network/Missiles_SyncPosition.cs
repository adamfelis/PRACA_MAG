using System.Net.Mime;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.scripts.Model;
using UnityEngine.Networking;
using UnityEngine.UI;

[NetworkSettings(channel = 1, sendInterval = 0.033f)]
public class Missiles_SyncPosition : NetworkBehaviour
{
    [SyncVar(hook = "OnPosMissileLeft1Changed")]
    public Vector3 missileLeft1;

    [SyncVar(hook = "OnPosMissileLeft1Hit")]
    private bool missileLeft1Hit;

    [SyncVar(hook = "OnPosMissileLeft2Changed")]
    public Vector3 missileLeft2;

    [SyncVar(hook = "OnPosMissileLeft2Hit")]
    private bool missileLeft2Hit;

    [SyncVar(hook = "OnPosMissileRight1Changed")]
    public Vector3 missileRight1;

    [SyncVar(hook = "OnPosMissileRight1Hit")]
    private bool missileRight1Hit;

    [SyncVar(hook = "OnPosMissileRight2Changed")]
    public Vector3 missileRight2;

    [SyncVar(hook = "OnPosMissileRight2Hit")]
    private bool missileRight2Hit;



    private Vector3 lastPos;
    private float threshold = 5f;
    private Text latencyText;
    private int latency;
    private float globalIterationCounter = 0.0f;
    private MissileController missileController;
    private Transform weapons;
    private bool isInitialized = false;
    public void Initialize()
    {
        missileController = GetComponent<MissileController>();
        weapons = missileController.missiles[0].Body.transform.parent;
        isInitialized = true;
    }

	void Update ()
	{
        if (!isInitialized)
            return;
	    if (!isLocalPlayer)
	    {
	        LerpPosition();
	    }

	}

    void FixedUpdate()
    {
        if (!isInitialized)
            return;
        TransmitPosition();
    }

    /// <summary>
    /// We want to lerp only other characters to limit network traffic. Our own position remains unchanged.
    /// </summary>
    void LerpPosition()
    {
        float singleIterationTime = Time.deltaTime;
        float wholeInterpolationTime = Time.fixedDeltaTime;
        globalIterationCounter += singleIterationTime;
        float t = globalIterationCounter / wholeInterpolationTime;
        foreach (var missileComponentse in missileController.missiles.Values)
        {
            if (missileComponentse.IsTriggered)
            {
                missileComponentse.Body.transform.position =
                    Vector3.Lerp(missileComponentse.PrevPos, missileComponentse.TargetPos, t);
                //Debug.Log(missileComponentse.Body.transform.position);
            }
        }
        if (t > 1)
            globalIterationCounter = 0.0f;
    }


    private void OnPosMissileLeft1Changed(Vector3 pos)
    {
        if (!isLocalPlayer)
        {
            if (missileController.missiles[0].Body.transform.parent != null)
                missileController.missiles[0].Body.transform.parent = null;
            missileController.missiles[0].IsTriggered = true;
            missileController.missiles[0].TargetPos = pos;
            //Debug.Log(pos);
        }
    }

    private void OnPosMissileLeft1Hit(bool hit)
    {
        if (hit)
        {
            restartMissile(0);
        }
    }

    private void OnPosMissileLeft2Changed(Vector3 pos)
    {
        if (!isLocalPlayer)
        {
            if (missileController.missiles[1].Body.transform.parent != null)
                missileController.missiles[1].Body.transform.parent = null;
            missileController.missiles[1].IsTriggered = true;
            missileController.missiles[1].TargetPos = pos;
        }
    }

    private void OnPosMissileLeft2Hit(bool hit)
    {
        if (hit)
        {
            restartMissile(1);
        }
    }

    private void OnPosMissileRight1Changed(Vector3 pos)
    {
        if (!isLocalPlayer)
        {
            if (missileController.missiles[2].Body.transform.parent != null)
                missileController.missiles[2].Body.transform.parent = null;
            missileController.missiles[2].IsTriggered = true;
            missileController.missiles[2].TargetPos = pos;
        }
    }

    private void OnPosMissileRight1Hit(bool hit)
    {
        if (hit)
        {
            restartMissile(2);
        }
    }

    private void OnPosMissileRight2Changed(Vector3 pos)
    {
        if (!isLocalPlayer)
        {
            if (missileController.missiles[3].Body.transform.parent != null)
                missileController.missiles[3].Body.transform.parent = null;
            missileController.missiles[3].IsTriggered = true;
            missileController.missiles[3].TargetPos = pos;
        }
    }

    private void OnPosMissileRight2Hit(bool hit)
    {
        if (hit)
        {
            restartMissile(3);
        }
    }

    private void restartMissile(int missileId)
    {
        missileController.missiles[missileId].Body.transform.SetParent(weapons);
        missileController.missiles[missileId].IsTriggered = false;
        missileController.missiles[missileId].Body.transform.localPosition = missileController.missiles[missileId].offset;
    }

    [Command]
    void CmdProvideMissilePositionToServer(Vector3 value, int missileId)
    {
        switch (missileId)
        {
            case 0:
                missileLeft1 = value;
                break;
            case 1:
                missileLeft2 = value;
                break;
            case 2:
                missileRight1 = value;
                break;
            case 3:
                missileRight2 = value;
                break;
        }
    }

    [Command]
    void CmdProvideMissileHit(int missileId)
    {
        bool value = true;
        switch (missileId)
        {
            case 0:
                missileLeft1Hit = value;
                break;
            case 1:
                missileLeft2Hit = value;
                break;
            case 2:
                missileRight1Hit = value;
                break;
            case 3:
                missileRight2Hit = value;
                break;
        }
    }

    [ClientCallback]
    public void TransmitMissileHit(int missileId)
    {
        if (isLocalPlayer)
        {
            CmdProvideMissileHit(missileId);
        }
    }

    [ClientCallback]
    private void TransmitPosition()
    {
        //if (isLocalPlayer && Vector3.Distance(myTransform.position, lastPos) > threshold)
        if (isLocalPlayer)
        {
            //Debug.Log(missileController.missiles[0].IsTriggered);
            if (missileController.missiles[0].IsTriggered)
                CmdProvideMissilePositionToServer(missileController.missiles[0].Body.transform.localPosition, 0);

            if (missileController.missiles[1].IsTriggered)
                CmdProvideMissilePositionToServer(missileController.missiles[1].Body.transform.localPosition, 1);

            if (missileController.missiles[2].IsTriggered)
                CmdProvideMissilePositionToServer(missileController.missiles[2].Body.transform.localPosition, 2);

            if (missileController.missiles[3].IsTriggered)
                CmdProvideMissilePositionToServer(missileController.missiles[3].Body.transform.localPosition, 3);
        }
    }
}
