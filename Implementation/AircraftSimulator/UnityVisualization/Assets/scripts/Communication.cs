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

    void OnApplicationQuit(){
        communicator.DisconnectFromServer();
    }

    public ServerDataReceivedHandler ServerDataReceived
    {
        get
        {
          return delegate (object sender, DataEventArgs eventArgs)
          {
              switch (eventArgs.Data.MessageType)
              {
                  case MessageType.ClientJoinResponse:
                      Debug.Log("Connected to the server");
                      break;
                  case MessageType.ServerDisconnected:
                      Debug.Log("Disconnected from the server");
                      break;
                  default:
                      break;
              }
          };
        }
    }

    public ServerDisconnectedHandler ServerDisconnected
    {
        get
        {
            return delegate(object sender) 
            {
                Debug.Log("Server connection interrupted.");
            };
        }
    }

}
