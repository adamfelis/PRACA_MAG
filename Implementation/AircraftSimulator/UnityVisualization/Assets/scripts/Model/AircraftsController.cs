using UnityEngine;
using System.Collections;
using Assets.scripts;
using Assets.scripts.Model;
using Assets.scripts.UI;
using UnityEngine.Networking;

public enum AircraftType
{
    F15,
    F35
}

public class AircraftsController : NetworkBehaviour
{
    public Aircraft Aircraft;
    public event NotifierHandler Initialized;

    public MissileController MissileController;
    // Use this for initialization
    public void Initialize ()
	{
	    GameObject body = this.gameObject;
        Aircraft = new Aircraft()
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
        Aircraft.Initialize(sceneController);

        if (isLocalPlayer)
        {
            body.AddComponent<GUIUpdater>().Aircraft = Aircraft;
            MissileController = body.AddComponent<MissileController>();
            MissileController.Initialize();
            var applicationManager = GameObject.FindGameObjectWithTag(Tags.ApplicationManager);
            applicationManager.GetComponent<InputController>().Initialize(this);
            applicationManager.GetComponent<Communication>().enabled = true;
        }
	}

    void Update()
    {
        if (isLocalPlayer)
            Aircraft.aircraftInterpolator.Interpolate(Time.deltaTime);
    }
}
