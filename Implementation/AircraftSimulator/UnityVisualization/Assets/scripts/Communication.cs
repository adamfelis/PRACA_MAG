using System;
using UnityEngine;
using System.Collections;
using Client;
using Client.Priveleges;
using Common.EventArgs;
using Common.Containers;
using Random = System.Random;

public class Communication : MonoBehaviour, IClientPrivileges
{
    private IClient communicator;
    private Random random;
	// Use this for initialization
	void Start () {
        communicator = new global::Client.Client(this);
        random = new Random();
        Debug.Log(communicator.ConnectToServer());
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnApplicationQuit(){
        communicator.DisconnectFromServer();
    }

    private void sendDataToTheServer(int n)
    {
        IData data = new Data()
        {
            Array = matrixArrayCreator(n),
            MessageType = MessageType.ClientDataRequest,
            Response = ActionType.ResponseRequired
        };
        Debug.Log(communicator.ClientInputPriveleges.SendDataRequest(data));
    }

    private float[][] matrixArrayCreator(int n)
    {
        var array = new float[n][];
        for (int i = 0; i < n; i++)
        {
            array[i] = new float[n];
            for (int j = 0; j < n; j++)
                array[i][j] = (float) random.NextDouble();
        }
        return array;
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
                      sendDataToTheServer(4);
                      break;
                  case MessageType.ServerDisconnected:
                      Debug.Log("Disconnected from the server");
                      break;
                  case MessageType.ClientDataResponse:
                      Debug.Log("Response received from the server:");
                      foreach (var f in eventArgs.Data.Array)
                      {
                          string toPrint = String.Empty;
                          foreach (var f1 in f)
                          {
                              toPrint += f1 + " ";
                          }
                          Debug.Log(toPrint);
                      }
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
