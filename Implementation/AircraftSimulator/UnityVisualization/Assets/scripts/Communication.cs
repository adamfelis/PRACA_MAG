using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.scripts;
using Assets.scripts.Data_Manipulation;
using Client;
using Client.Priveleges;
using Common.AircraftData;
using Common.EventArgs;
using Common.Containers;
using Random = System.Random;

public class Communication : MonoBehaviour, ICommunication
{
    private AircraftsController aircraftsController;
    private IDataReader dataReader;
    private IDataWriter dataWriter;
    private IDataHandler dataHandler;

    public void NotifyUnityShell(string toOutput)
    {
        Debug.Log(toOutput);
    }

    private void Initialize()
    {

        aircraftsController = GetComponent<AircraftsController>();

        dataHandler = new DataHandler(this, aircraftsController);
        dataReader = new DataReader(dataHandler, this);
        dataWriter = new DataWriter(dataReader, this, aircraftsController);

        dataHandler.ClientResponseHandler += dataWriter.SendInputToServer;
    }

    private void Start()
    {
        Initialize();
        dataWriter.SendJoinMessage();
    }

    private void Update()
    {
    }

    private void FixedUpdate()
    {
        dataReader.ReadDataSentFromServer();
    }

    private void OnApplicationQuit()
    {
        dataWriter.DisconnectFromServer();
    }
}
