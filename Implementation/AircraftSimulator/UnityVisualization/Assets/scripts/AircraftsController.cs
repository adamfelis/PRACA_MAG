using UnityEngine;
using System.Collections;
using Assets.scripts;

public enum AircraftType
{
    F15,
    F35
}

public class AircraftsController : MonoBehaviour
{
    public Aircraft aircraft;
	// Use this for initialization
	void Start ()
	{
        GameObject body = GameObject.FindGameObjectWithTag(Tags.F15);
        aircraft = new Aircraft()
	    {
	        Body = body,
            RudderLeft = Tags.FindGameObjectWithTagInParent(body, Tags.RudderLeft),
            RudderRight = Tags.FindGameObjectWithTagInParent(body, Tags.RudderRight),
            ElevatorLeft = Tags.FindGameObjectWithTagInParent(body, Tags.ElevatorLeft),
            ElevatorRight = Tags.FindGameObjectWithTagInParent(body, Tags.ElevatorRight),
            AileronLeft = Tags.FindGameObjectWithTagInParent(body, Tags.AileronLeft),
            AileronRight = Tags.FindGameObjectWithTagInParent(body, Tags.AileronRight)
        };
        aircraft.Initialize();
	    Camera.main.GetComponent<CameraSmoothFollow>().target = aircraft.Body.transform;
	    Camera.main.GetComponent<CameraSmoothFollow>().enabled = true;
	    Camera.main.GetComponent<InputController>().aircraft = aircraft;
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
