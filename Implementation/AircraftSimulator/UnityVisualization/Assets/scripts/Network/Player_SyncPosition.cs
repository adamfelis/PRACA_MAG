using System.Net.Mime;
using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;

//[NetworkSettings(channel = 1, sendInterval = 0.033f)]
public class Player_SyncPosition : NetworkBehaviour
{
    /// <summary>
    /// Server will automatically sync this value to other clients when variable changes.
    /// </summary>
    [SyncVar(hook = "OnPosChanged")]
    private Vector3 syncPos;
    [SerializeField] private Transform myTransform;
    [SerializeField] private float lerpRate = 15;


    /// <summary>
    /// time interval between sending request
    /// </summary>
    private Vector3 lastPos;
    /// <summary>
    /// previous interpolated position
    /// </summary>
    private Vector3 previousPos;

    private float threshold = 5f;

    private NetworkClient nClient;
    private Text latencyText;
    private int latency;
    private float globalIterationCounter = 0.0f;
    private Player_ID playerId;

    public Vector3 Velocity
    {
        get { return (myTransform.position - previousPos)/(Time.deltaTime); }
    }

    private void OnPosChanged(Vector3 v)
    {
        globalIterationCounter = 0;
    }


    void Start()
    {
        nClient = GameObject.FindGameObjectWithTag(Tags.NetworkManager).GetComponent<NetworkManager>().client;
        playerId = transform.root.gameObject.GetComponent<Player_ID>();
        myTransform = transform;
        previousPos = Vector3.zero;
        //latencyText = GameObject.FindGameObjectWithTag(Tags.ServerLatency).GetComponent<Text>();
        //if (latencyText == null)
        //    Debug.Log("latency text missing");
    }

	void FixedUpdate () 
    {
	    TransmitPosition();
	}

    void Update()
    {
        LerpPosition();
        //ShowLatency();
    }

    /// <summary>
    /// We want to lerp only other characters to limit network traffic. Our own position remains unchanged.
    /// </summary>
    void LerpPosition()
    {
        if (!playerId.isLocalPlayer)
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

            previousPos = myTransform.position;
            myTransform.position = Vector3.Lerp(myTransform.position, syncPos, t);
        }
    }


    /// <summary>
    /// Client is talking to server, but the code will be invoked on server side.
    /// </summary>
    /// <param name="pos"></param>
    [Command]
    void CmdProvidePositionToServer(Vector3 pos)
    {
        syncPos = pos;
        //Debug.Log("Command position");
    }

    [ClientCallback]
    void TransmitPosition()
    {
        //if (isLocalPlayer && Vector3.Distance(myTransform.position, lastPos) > threshold)
        if (isLocalPlayer)
        {
            CmdProvidePositionToServer(myTransform.position);
            lastPos = myTransform.position;
        }
    }

    void ShowLatency()
    {
        if (isLocalPlayer)
        {
            latency = nClient.GetRTT();
            latencyText.text = latency.ToString();
        }
    }

}
