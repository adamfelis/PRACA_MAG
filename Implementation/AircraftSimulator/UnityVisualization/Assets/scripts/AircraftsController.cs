using UnityEngine;
using System.Collections;
using Assets.scripts;
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
        //var aircraftModel = GameObject.FindGameObjectWithTag(Tags.F15);
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
        //var aircraftInterpolator = gameObject.AddComponent<AircraftInterpolator>();
        //aircraftInterpolator.Body = gameObject;
        //aircraft.aircraftInterpolator = aircraftInterpolator;
        aircraft.Initialize();
        
	    if (isLocalPlayer)
	    {
	        Camera.main.GetComponent<CameraSmoothFollow>().target = aircraft.Body.transform;
	        Camera.main.GetComponent<CameraSmoothFollow>().enabled = true;
	        Camera.main.GetComponent<InputController>().aircraft = aircraft;
	        Camera.main.GetComponent<InputController>().enabled = true;
	        Camera.main.GetComponent<Communication>().enabled = true;
	    }
        Initialized.Invoke();
    }

    // Update is called once per frame
    void Update()
    {
        if (isLocalPlayer)
            aircraft.aircraftInterpolator.Interpolate(Time.deltaTime);
    }

    //void FixedUpdate()
    //{
    //    if (isLocalPlayer)
    //        aircraft.aircraftInterpolator.Interpolate(Time.fixedDeltaTime, Time.fixedDeltaTime);
    //}
}
