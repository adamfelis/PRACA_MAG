using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
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
    private Queue<DataEventArgs>communicatesFromServer;
    private int ticks = 0;
	// Use this for initialization
	void Start () {
        communicator = new global::Client.Client(this);
        inputController = Camera.main.GetComponent<InputController>();
	    aircraftsController = GetComponent<AircraftsController>();
        random = new Random();
        communicatesFromServer = new Queue<DataEventArgs>();
        Debug.Log(communicator.ConnectToServer());
    }
	
	// Update is called once per frame
	void Update ()
	{
	}

    void FixedUpdate()
    {
        ticks++;
        readCommunicatesFromServer();
    }

    void readCommunicatesFromServer()
    {
        lock (communicatesFromServer)
        {
            while (communicatesFromServer.Count > 0)
            {
                var eventArgs = communicatesFromServer.Dequeue();
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
            }
        }
    }

    void OnApplicationQuit(){
        communicator.DisconnectFromServer();
    }
    private const int simulation_time = 101;
    private static int simulation_index = 0;
    private IData composeLogitudinalData(Vector3 velocity, float q, float theta, float ni, float tau)
    {
        simulation_index = (simulation_index + 1) % simulation_time;
        if (simulation_index == 0)
            simulation_index++;
        return new Data()
        {
            Array = new float[][]
            {
                new float[]
                {
                    //velocity in X (from book) here -Z axis (U)
                    velocity.z,
                    //velocity in Z (from book) here Y axis (W)
                    velocity.y,
                    //q
                    q,
                    //angle of attack
                    theta
                },
                new float[]
                {
                    //elevator rotation
                    ni,
                    //aircrafts throtle
                    tau
                },
                new float[]
                {
                    //index of simulation
                    simulation_index,
                }
            },
            MessageType = MessageType.ClientDataRequest,
            InputType = DataType.Matrix,
            OutputType = DataType.Vector,
            Response = ActionType.ResponseRequired
        };
    }

    private void sendInputToServer()
    {
        var aircraft = aircraftsController.aircraft;
        var angle = aircraft.Body.transform.localEulerAngles.x;
        if (angle >= 180)
        {
            angle = -(360 - angle);
        }
        //Debug.Log("rotacja przed wysłaniem" + aircraft.Body.transform.rotation.eulerAngles.x);
        Debug.Log(communicator.ClientInputPriveleges.SendDataRequest(composeLogitudinalData(
            aircraft.Velocity, //u, w
            aircraft.q,
            angle * Mathf.Deg2Rad, //theta
            aircraft.Ni,
            aircraft.Tau
            )));
        ticks = 0;
    }



    private float prev = 0;
    private void handleOutputFromServer(IData data)
    {
        //Debug.Log("ile minęło ticków: " + ticks);
        var aircraft = aircraftsController.aircraft;
        //velocity in x axis (u)
        aircraft.Velocity.z = data.Array[0][0];
        //velocity in Z axis (w)
        aircraft.Velocity.y = data.Array[0][1];
        //rotary velocity in y axis (q)
        var q = data.Array[0][2];
        aircraft.q = q;
        Vector3 asd = -aircraft.Velocity * Time.fixedDeltaTime;
        //aircraft.Body.transform.Translate(-aircraft.Velocity * Time.fixedDeltaTime);

        var position = aircraft.Body.transform.localPosition;
        position.x += aircraft.Velocity.z * Time.fixedDeltaTime;
        position.y += -aircraft.Velocity.y * Time.fixedDeltaTime;
        position.z += -aircraft.Velocity.x * Time.fixedDeltaTime;
        aircraft.Body.transform.localPosition = position;
        //theta is angle attack
        var theta = data.Array[0][3] * Mathf.Rad2Deg;
        if (theta >= 180)
            theta = -(360 - theta);
        var rotation = aircraft.Body.transform.localEulerAngles;
        rotation.x = theta;
        //Debug.Log("ustawiona rotacja " + rotation.x);
        aircraft.Body.transform.localEulerAngles = rotation;

        //var deltaTheta = theta - aircraft.Body.transform.eulerAngles.x;
        //Debug.Log("delta Theta " + deltaTheta);
        //var rotation = Quaternion.AngleAxis(deltaTheta, Vector3.right);
        //aircraft.Body.transform.rotation *= rotation;
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
          return (object sender, DataEventArgs eventArgs) =>
          {
              lock (communicatesFromServer)
              {
                  communicatesFromServer.Enqueue(eventArgs);
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
