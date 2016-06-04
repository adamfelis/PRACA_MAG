using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
//using UnityStandardAssets.Characters.FirstPerson;
using System;

[NetworkSettings(channel = 0, sendInterval = 0.033f)]
public class Player_SyncRotation : NetworkBehaviour
{

    [SyncVar]
    private Quaternion syncPlayeRotation;

    [SerializeField]
    private Transform playerTransform;
    [SerializeField]
    private float lerpRate = 15;
    private Player_ID playerId;
    private Quaternion lastPlayerRot;
    /// <summary>
    /// exceeding threshold enforces interpolation
    /// </summary>
    private float threshold = 1f;


    // Use this for initialization
    void Start()
    {
        playerTransform = transform;
        playerId = transform.root.gameObject.GetComponent<Player_ID>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        TransmitRotations();
        LerpRotations();
    }

    void Update()
    {
        LerpRotations();
    }
    private float globalIterationCounter = 0.0f;
    void LerpRotations()
    {

        if (!playerId.isLocalPlayer)
        { 
            if (IsValidQuaternion(syncPlayeRotation))
            {
                float singleIterationTime = Time.deltaTime;
                float wholeInterpolationTime = Time.fixedDeltaTime;
                globalIterationCounter += singleIterationTime;
                globalIterationCounter = Mathf.Clamp(globalIterationCounter, 0, wholeInterpolationTime);
                float t = globalIterationCounter / wholeInterpolationTime;
                //Debug.Log(t);
                float eps = 0.0001f;
                float diff = Mathf.Abs(globalIterationCounter - wholeInterpolationTime);
                if (diff < eps)
                    globalIterationCounter = 0.0f;

                playerTransform.rotation = Quaternion.Lerp(playerTransform.rotation, syncPlayeRotation, t);
            }
        }
    }

    private bool IsValidQuaternion(Quaternion q)
    {
        return Mathf.Abs(q.x) + Mathf.Abs(q.y) + Mathf.Abs(q.z) + Mathf.Abs(q.w) > float.Epsilon;
    }

    [Command]
    void CmdProvideRotationsToServer(Quaternion playerRot)
    {
        syncPlayeRotation = playerRot;
    }

    [Client]
    void TransmitRotations()
    {
        //only my character can talk to the server
        if (isLocalPlayer)
        {
            if (Quaternion.Angle(playerTransform.rotation, lastPlayerRot) > threshold)
            {
                CmdProvideRotationsToServer(playerTransform.rotation);
                lastPlayerRot = playerTransform.rotation;
            }
        }
    }

}
