using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.scripts;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public delegate void ClientDisconnectedHandler();
public class CustomNetworkManager : NetworkManager
{
    public event ClientDisconnectedHandler ClientDisconnected;

    public void StartupHost()
    {
        if (!NetworkManager.singleton.isNetworkActive)
        {
            SetPort();
            NetworkManager.singleton.StartHost();
        }

    }


    public void JoinGame()
    {
        SetIpAdress();
        SetPort();
        NetworkManager.singleton.StartClient();
    }

    void SetIpAdress()
    {
        var inputField = GameObject.FindGameObjectWithTag(Tags.NetworkIPInput);
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
        SetupMenuSceneButtons();
    }

    public void OnLevelWasLoaded(int level)
    {
        StartCoroutine(waitForEndOfFrameAndSetup(level));
    }

    void SetupMenuSceneButtons()
    {
        GameObject.FindGameObjectWithTag(Tags.ButtonStartHost).GetComponent<Button>().onClick.RemoveAllListeners();
        GameObject.FindGameObjectWithTag(Tags.ButtonStartHost).GetComponent<Button>().onClick.AddListener(StartupHost);

        GameObject.FindGameObjectWithTag(Tags.ButtonJoinGame).GetComponent<Button>().onClick.RemoveAllListeners();
        GameObject.FindGameObjectWithTag(Tags.ButtonJoinGame).GetComponent<Button>().onClick.AddListener(JoinGame);
    }

    IEnumerator waitForEndOfFrameAndSetup(int level)
    {
        yield return new WaitForEndOfFrame();
        if (level == 0)
            SetupMenuSceneButtons();
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
