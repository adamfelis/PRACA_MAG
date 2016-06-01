using System;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
//using UnityStandardAssets.Characters.FirstPerson;

public enum PlayerType
{
    Host,
    Client
}

public enum Team
{
    A, B, None
}

public delegate void NotifierHandler();


public class Player_ID : NetworkBehaviour
{
    [SyncVar]
    public string playerUniqueIdentity;

    [SyncVar]
    public int ServerAssignedId;
    
    public Team _team;
    private NetworkInstanceId playerNetId;
    private Transform myTransform;
    private const string playerName = "Player";
    private PlayerType playerType;
    private bool amIHost;
    private static int localPlayerId;

    public event NotifierHandler PlayerIdentitySet;

    public bool AmIHost
    {
        get { return amIHost; }
    }

    public PlayerType PlayerType
    {
        get { return playerType; }
    }

    public int Id
    {
        get
        {
            int toRet;
            int.TryParse(myTransform.name.TrimStart(playerName.ToCharArray()), out toRet);
            return toRet;
        }
    }

    public override void OnStartLocalPlayer()
    {
        GetNetIdentity();
    }

    // Use this for initialization
    void Start()
    {
        myTransform = transform;
        //SetIdentity();
    }

    // Update is called once per frame
    void Update()
    {
        //We are assigning identity in update cause it requires time for the player object to get instantianted.
        //That is the reason code cannot be applied in Start/Awake function
        if (myTransform.name == "" || myTransform.name == "Player(Clone)")
        {
            SetIdentity();
        }
    }

    [Client]
    void GetNetIdentity()
    {
        playerNetId = GetComponent<NetworkIdentity>().netId;
        playerType = int.Parse(playerNetId.ToString()) == 1 ? PlayerType.Host : PlayerType.Client;
        CmdProvideServerMyIdentity(MakeUniqueName());
    }

    [Client]
    public void SetupRemotelyAssignedId(int id)
    {
        CmdProvideServerRemoteAssignedId(id);
    }

    //void SetMapTag(bool isA)
    //{
    //    var n = gameObject.name;
    //    var s = n + "/MapTag";
    //    var mapTag = GameObject.Find(s);
    //    mapTag.GetComponent<Renderer>().material.color = isA ? Color.red : Color.green;
    //}

    void SetIdentity()
    {
        if (!isLocalPlayer)
        {
            myTransform.name = playerUniqueIdentity;
        }
        else
        {
            myTransform.name = MakeUniqueName();
            if (playerType == PlayerType.Host)
                amIHost = true;
            else
                amIHost = false;
        }

        if (isLocalPlayer)
        {
            localPlayerId = Id;
        }
        PlayerIdentitySet.Invoke();
    }

    string MakeUniqueName()
    {
        return playerName + playerNetId.ToString();
    }

    [Command]
    void CmdProvideServerMyIdentity(string name)
    {
        playerUniqueIdentity = name;
    }

    /// <summary>
    /// This is id assigned by Main application.
    /// It has nothing to do with UNET authorization mechanism
    /// </summary>
    /// <param name="id"></param>
    [Command]
    void CmdProvideServerRemoteAssignedId(int id)
    {
        ServerAssignedId = id;
    }

    private IEnumerable<GameObject> getAllPlayers()
    {
        return GameObject.FindGameObjectsWithTag(Tags.Player);
    }

    public GameObject GetLocalPlayer()
    {
        var players = getAllPlayers();
        foreach (var player in players)
        {
            if (player.GetComponent<Player_ID>().Id == localPlayerId)
                return player;
        }
        return null;
    }
}
