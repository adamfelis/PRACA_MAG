using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.scripts;
using UnityEngine.Events;
//using UnityEditor.SceneManagement;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public delegate void ClientDisconnectedHandler();
public class CustomNetworkManager : NetworkManager
{
    public event ClientDisconnectedHandler ClientDisconnected;
    private Dictionary<string, MatchDesc> myMatches = new Dictionary<string, MatchDesc>();
    private string localIPAddress;
    private SceneController sceneController;

    public string LocalIPAddress
    {
        get { return localIPAddress;}
    }
    private string RoomName
    {
        get
        {
            var inputField = GameObject.FindGameObjectWithTag(Tags.InputRoom);
            string roomName = inputField.GetComponent<Text>().text;
            if (roomName == "")
            {
                roomName = inputField.transform.parent.FindChild("Placeholder").GetComponent<Text>().text;
            }
            return roomName;
        }
    }

    public void StartupHost()
    {
        SetPort();
        setupLocalIP();
        NetworkManager.singleton.StartHost();
    }

    IEnumerator refreshMatchList()
    {
        while (true)
        {
            if (SceneManager.GetActiveScene().name == "Menu")
                matchMaker.ListMatches(0, 20, "", OnMatchList);
            yield return new WaitForSeconds(3);
        }
    }

    

    public override void OnMatchList(ListMatchResponse matchList)
    {
        base.OnMatchList(matchList);
        var dropdown = GameObject.FindGameObjectWithTag(Tags.DropdownRemoteAddress);
        if (dropdown)
        {
            dropdown.GetComponent<Dropdown>().options.Clear();
            myMatches.Clear();
            foreach (var match in matchList.matches)
            {
                var option = new Dropdown.OptionData()
                {
                    text = match.name
                };
                if (!myMatches.ContainsKey(option.text))
                {
                    dropdown.GetComponent<Dropdown>().options.Add(option);
                    myMatches.Add(option.text, match);
                    dropdown.GetComponent<Dropdown>().value = dropdown.GetComponent<Dropdown>().options.Count;
                }
            }
            Debug.Log(myMatches.Count);
        }
    }

    private void setupLocalIP()
    {
        var inputField = GameObject.FindGameObjectWithTag(Tags.InputLocalAddress);
        string ipAdress = inputField.GetComponent<Text>().text;
        if (ipAdress == "")
        {
            ipAdress = inputField.transform.parent.FindChild("Placeholder").GetComponent<Text>().text;
        }
        localIPAddress = ipAdress;
    }


    public void StartRemoteHost()
    {
        uint roomSize = 2;
        matchMaker.CreateMatch(RoomName, roomSize, true, "", OnMatchCreate);
    }

    public override void OnMatchCreate(CreateMatchResponse matchInfo)
    {
        base.OnMatchCreate(matchInfo);
        matchMaker.JoinMatch(matchInfo.networkId, "", OnMatchJoined);
    }

    public void JoinGame()
    {
        SetIpAdress();
        SetPort();
        NetworkManager.singleton.StartClient();
    }


    public void JoinRemote()
    {
        var dropdown = GameObject.FindGameObjectWithTag(Tags.DropdownRemoteAddress);
        string roomName = dropdown.GetComponent<Dropdown>().captionText.text;
        matchMaker.JoinMatch(myMatches[roomName].networkId, "", OnMatchJoined);
    }

    void SetIpAdress()
    {
        var inputField = GameObject.FindGameObjectWithTag(Tags.InputLocalAddress);
        string ipAdress = inputField.GetComponent<Text>().text;
        if (ipAdress == "")
        {
            ipAdress = inputField.transform.parent.FindChild("Placeholder").GetComponent<Text>().text;
        }
        localIPAddress = ipAdress;
        NetworkManager.singleton.networkAddress = ipAdress;
    }

    void SetPort()
    {
        NetworkManager.singleton.networkPort = 7777;
    }

    void Start()
    {
        sceneController = GetComponent<SceneController>();
        StartCoroutine(refreshMatchList());
    }

    public void OnLevelWasLoaded(int level)
    {
        StartCoroutine(waitForEndOfFrameAndSetup(level));
    }

    IEnumerator waitForEndOfFrameAndSetup(int level)
    {
        yield return new WaitForEndOfFrame();
        if (level == 0)
            sceneController.SetupMenuScene();
        else
        {
            sceneController.SetupOtherSceneButtons();
        }
    }

    public void DisconnectFromServer()
    {
        StartCoroutine(waitForEndOfFrameAndStopServer());
    }

    IEnumerator waitForEndOfFrameAndStopServer()
    {
        var applicationManager = GameObject.FindGameObjectWithTag(Tags.ApplicationManager);
        applicationManager.GetComponent<InputController>().enabled = false;
        ClientDisconnected.Invoke();
        yield return new WaitForEndOfFrame();
        yield return new WaitForFixedUpdate();
        NetworkManager.singleton.StopHost();
    }


}
