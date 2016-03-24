using UnityEngine;
using System.Collections;
using Client;
using Client.Priveleges;
using Common.EventArgs;
using Common.Containers;

public class Communication : MonoBehaviour, IClientPrivileges
{
    private IClient communicator;
	// Use this for initialization
	void Start () {
        communicator = new global::Client.Client(this);
        Debug.Log(communicator.ConnectToServer());
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public ServerDataReceivedHandler ServerDataReceived
    {
        get { return serverDataReceived; }
    }

    private void serverDataReceived(object sender, DataEventArgs eventArgs)
    {
        switch (eventArgs.Data.MessageType)
        {
            case MessageType.ClientJoinResponse:
                Debug.Log("connected to the server");
                break;
            default:
                break;
        }
    }

}
