using UnityEngine;
using System.Collections;
using Assets.scripts;
using Assets.scripts.UI;
using UnityEngine.Networking;

public enum AircraftType
{
    F15,
    F35
}

public class AircraftsController : NetworkBehaviour
{
    public Aircraft aircraft;
    public event NotifierHandler Initialized;
    // Use this for initialization
    void Start ()
	{
	    GameObject body = this.gameObject;
        aircraft = new Aircraft()
	    {
	        Body = body,
            RudderLeft = Tags.FindGameObjectWithTagInParent(Tags.RudderLeft, name),
            RudderRight = Tags.FindGameObjectWithTagInParent(Tags.RudderRight, name),
            ElevatorLeft = Tags.FindGameObjectWithTagInParent(Tags.ElevatorLeft, name),
            ElevatorRight = Tags.FindGameObjectWithTagInParent(Tags.ElevatorRight, name),
            AileronLeft = Tags.FindGameObjectWithTagInParent(Tags.AileronLeft, name),
            AileronRight = Tags.FindGameObjectWithTagInParent(Tags.AileronRight, name)
        };
        var sceneController = GameObject.FindGameObjectWithTag(Tags.NetworkManager).GetComponent<SceneController>();
        aircraft.Initialize(sceneController);
        
	    if (isLocalPlayer)
	    {
	        body.AddComponent<GUIUpdater>().Aircraft = aircraft;
            var applicationManager = GameObject.FindGameObjectWithTag(Tags.ApplicationManager);
            applicationManager.GetComponent<InputController>().aircraft = aircraft;
            applicationManager.GetComponent<InputController>().cameraSmoothFollow = Tags.FindGameObjectWithTagInParent(Tags.CameraManager, name).GetComponent<CameraSmoothFollow>();
            applicationManager.GetComponent<InputController>().enabled = true;
            applicationManager.GetComponent<Communication>().enabled = true;
	    }
        Initialized.Invoke();
    }

    void Update()
    {
        if (isLocalPlayer)
            aircraft.aircraftInterpolator.Interpolate(Time.deltaTime);
    }
}
