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
    [SyncVar(hook = "OnIdentityChanged")]
    public string playerUniqueIdentity;

    [SyncVar]
    public int ServerAssignedId;
    
    public Team _team;
    private NetworkInstanceId playerNetId;
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
            int.TryParse(transform.name.TrimStart(playerName.ToCharArray()), out toRet);
            return toRet;
        }
    }

    private void Start()
    {
        if (!isLocalPlayer && playerUniqueIdentity != "")
        {
            transform.name = playerUniqueIdentity;
            if (PlayerIdentitySet == null)
                Debug.LogWarning("PlayerIdentitySet unsubscribed");
            else
                PlayerIdentitySet.Invoke();
        }
    }

    public override void OnStartLocalPlayer()
    {
        GetNetIdentity();
    }

    private void OnIdentityChanged(string uniqueIdentity)
    {
        transform.name = uniqueIdentity;
        if (isLocalPlayer)
            localPlayerId = Id;
        if (playerType == PlayerType.Host)
            amIHost = true;
        else
            amIHost = false;
        if (PlayerIdentitySet == null)
            Debug.LogWarning("PlayerIdentitySet unsubscribed");
        else
            PlayerIdentitySet.Invoke();
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

    public GameObject GetPlayerById(int targetId)
    {
        var players = getAllPlayers();
        foreach (var player in players)
        {
            if (player.GetComponent<Player_ID>().Id == targetId)
                return player;
        }
        return null;
    }

    public GameObject GetPlayerByRemoteAssignedId(int remoteId)
    {
        var players = getAllPlayers();
        foreach (var player in players)
        {
            if (player.GetComponent<Player_ID>().ServerAssignedId == remoteId)
                return player;
        }
        return null;
    }

    public string GetLocalPlayerName()
    {
        return GetLocalPlayer().transform.root.name;
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
