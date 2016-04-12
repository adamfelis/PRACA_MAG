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
    private InputController inputController;
    private AircraftsController aircraftsController;
    private float deltaTime;
	// Use this for initialization
	void Start () {
        communicator = new global::Client.Client(this);
        inputController = Camera.main.GetComponent<InputController>();
	    aircraftsController = GetComponent<AircraftsController>();
        random = new Random();
        Debug.Log(communicator.ConnectToServer());
    }
	
	// Update is called once per frame
	void Update ()
	{
	    deltaTime += Time.deltaTime;
	}

    void OnApplicationQuit(){
        communicator.DisconnectFromServer();
    }

    private IData composeLogitudinalData(Vector3 velocity, float ni)
    {
        return new Data()
        {
            Array = new float[][]
            {
                new float[]
                {
                    //velocity in X axis (U)
                    velocity.x,
                    //velocity in Z axis (W)
                    velocity.z,
                    //angle of attack
                    ni
                }
            },
            MessageType = MessageType.ClientDataRequest,
            InputType = DataType.Vector,
            Response = ActionType.ResponseRequired
        };
    }

    private void sendInputToServer()
    {
        var aircraft = aircraftsController.aircraft;
        Debug.Log(communicator.ClientInputPriveleges.SendDataRequest(composeLogitudinalData(
                aircraft.Velocity,
                aircraft.Ni
            )));
    }

    private void handleOutputFromServer(IData data)
    {
        var aircraft = aircraftsController.aircraft;
        //velocity in x axis (u)
        aircraft.Velocity.x = data.Array[0][0];
        //velocity in z axis (w)
        aircraft.Velocity.z = data.Array[0][1];
        //rotary velocity in y axis (q)
        var q = data.Array[0][2];
        aircraft.Body.transform.Translate(aircraft.Velocity * deltaTime);
        deltaTime = 0;
        //theta is angle attack
        aircraft.Body.transform.Rotate(Vector3.right, data.Array[0][3]);
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
                      //sendDataToTheServer(4);
                      sendInputToServer();
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
                      handleOutputFromServer(eventArgs.Data);
                      sendInputToServer();
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
