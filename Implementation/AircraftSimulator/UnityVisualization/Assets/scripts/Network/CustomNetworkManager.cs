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

    void StartupHost()
    {
        SetPort();
        NetworkManager.singleton.StartHost();
    }

    IEnumerator refreshMatchList()
    {
        while (true)
        {
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


    void StartRemoteHost()
    {
        uint roomSize = 2;
        matchMaker.CreateMatch(RoomName, roomSize, true, "", OnMatchCreate);
    }

    public override void OnMatchCreate(CreateMatchResponse matchInfo)
    {
        base.OnMatchCreate(matchInfo);
        matchMaker.JoinMatch(matchInfo.networkId, "", OnMatchJoined);
    }

    void JoinGame()
    {
        SetIpAdress();
        SetPort();
        NetworkManager.singleton.StartClient();
    }


    void JoinRemote()
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
        NetworkManager.singleton.networkAddress = ipAdress;
    }

    void SetPort()
    {
        NetworkManager.singleton.networkPort = 7777;
    }

    void Start()
    {
        SetupMenuScene();
        StartCoroutine(refreshMatchList());
    }

    public void OnLevelWasLoaded(int level)
    {
        StartCoroutine(waitForEndOfFrameAndSetup(level));
    }

    private void dropdownValueChanged(int index)
    {
        var dropdown = GameObject.FindGameObjectWithTag(Tags.DropdownRemoteAddress);
        string content = dropdown.GetComponent<Dropdown>().options[index].text;
        dropdown.transform.FindChild("Label").GetComponent<Text>().text = content;
    }

    void SetupMenuScene()
    {
        NetworkManager.singleton.StartMatchMaker();

        GameObject.FindGameObjectWithTag(Tags.ButtonStartLocal).GetComponent<Button>().onClick.RemoveAllListeners();
        GameObject.FindGameObjectWithTag(Tags.ButtonStartLocal).GetComponent<Button>().onClick.AddListener(StartupHost);

        GameObject.FindGameObjectWithTag(Tags.ButtonJoinLocal).GetComponent<Button>().onClick.RemoveAllListeners();
        GameObject.FindGameObjectWithTag(Tags.ButtonJoinLocal).GetComponent<Button>().onClick.AddListener(JoinGame);

        GameObject.FindGameObjectWithTag(Tags.ButtonStartRemote).GetComponent<Button>().onClick.RemoveAllListeners();
        GameObject.FindGameObjectWithTag(Tags.ButtonStartRemote).GetComponent<Button>().onClick.AddListener(StartRemoteHost);

        GameObject.FindGameObjectWithTag(Tags.ButtonJoinRemote).GetComponent<Button>().onClick.RemoveAllListeners();
        GameObject.FindGameObjectWithTag(Tags.ButtonJoinRemote).GetComponent<Button>().onClick.AddListener(JoinRemote);

        GameObject.FindGameObjectWithTag(Tags.DropdownRemoteAddress)
            .GetComponent<Dropdown>()
            .onValueChanged.RemoveAllListeners();
        GameObject.FindGameObjectWithTag(Tags.DropdownRemoteAddress).GetComponent<Dropdown>().onValueChanged.AddListener(new UnityAction<int>(dropdownValueChanged));
    }

    IEnumerator waitForEndOfFrameAndSetup(int level)
    {
        yield return new WaitForEndOfFrame();
        if (level == 0)
            SetupMenuScene();
        else
        {
            SetupOtherSceneButtons();
        }
    }

    void disconnectFromServer()
    {
        StartCoroutine(waitForEndOfFrameAndStopServer());
    }

    IEnumerator waitForEndOfFrameAndStopServer()
    {
        Camera.main.GetComponent<CameraSmoothFollow>().enabled = false;
        Camera.main.GetComponent<InputController>().enabled = false;
        ClientDisconnected.Invoke();
        yield return new WaitForEndOfFrame();
        yield return new WaitForFixedUpdate();
        NetworkManager.singleton.StopHost();
    }

    void SetupOtherSceneButtons()
    {
        GameObject.FindGameObjectWithTag(Tags.ButtonDisconnect).GetComponent<Button>().onClick.RemoveAllListeners();
        GameObject.FindGameObjectWithTag(Tags.ButtonDisconnect)
            .GetComponent<Button>()
            .onClick.AddListener(disconnectFromServer);
    }
}
