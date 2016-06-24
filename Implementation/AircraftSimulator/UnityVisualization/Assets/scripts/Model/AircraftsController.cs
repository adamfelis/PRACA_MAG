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

        MissileController = body.AddComponent<MissileController>();
        MissileController.Initialize();
        if (isLocalPlayer)
        {
            Tags.FindGameObjectWithTagInParent(Tags.TrailMarker, name).GetComponent<MeshRenderer>().material.SetColor("_Color", Color.green);
            body.AddComponent<GUIUpdater>().Aircraft = Aircraft;
            var trailerRenderer = Tags.FindGameObjectWithTagInParent(Tags.TrailMarker, name).transform.parent.GetComponent<TrailRenderer>();
            trailerRenderer.material = Resources.Load("trail_my", typeof (Material)) as Material;
            //MissileController = body.AddComponent<MissileController>();
            //MissileController.Initialize();
            var applicationManager = GameObject.FindGameObjectWithTag(Tags.ApplicationManager);
            applicationManager.GetComponent<InputController>().Initialize(this);
            applicationManager.GetComponent<Communication>().enabled = true;
        }
        else
        {
            Tags.FindGameObjectWithTagInParent(Tags.TrailMarker, name).GetComponent<MeshRenderer>().material.SetColor("_Color", Color.red);
            var trailerRenderer = Tags.FindGameObjectWithTagInParent(Tags.TrailMarker, name).transform.parent.GetComponent<TrailRenderer>();
            trailerRenderer.material = Resources.Load("trail_enemy", typeof(Material)) as Material;
        }
        Tags.FindGameObjectWithTagInParent(Tags.TrailMarker, name).GetComponent<MeshRenderer>().enabled = true;

    }

    void Update()
    {
        if (isLocalPlayer)
            Aircraft.aircraftInterpolator.Interpolate(Time.deltaTime);
    }
}
