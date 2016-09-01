using UnityEngine;
using System.Collections;
using Assets.scripts;
using Assets.scripts.Data_Manipulation;
using Assets.scripts.Model;
using Assets.scripts.UI;
using UnityEngine.Networking;
using UnityEngine.UI;

public enum AircraftType
{
    F15,
    F35
}

public class AircraftsController : NetworkBehaviour
{
    public Aircraft Aircraft;
    public event NotifierHandler Initialized;

    public bool IsDestroying { get; set; }
    private GameObject pullUp, pullDown, lostSteering;

    public bool FirstResponseReceived { get; set; }

    public MissileController MissileController;

    private FileWriter fileWriter = new FileWriter();
    private string longitudinal = "longitudinal.txt";
    private string lateral = "lateral.txt";
    private string control = "control.txt";
    // Use this for initialization
    public void Initialize ()
	{
        //fileWriter.CreateFile(longitudinal);
        //fileWriter.CreateFile(lateral);
        //fileWriter.CreateFile(control);
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
            pullDown = GameObject.FindGameObjectWithTag(Tags.PullDown);
            pullUp = GameObject.FindGameObjectWithTag(Tags.PullUp);
            lostSteering = GameObject.FindGameObjectWithTag(Tags.WarningLostSteering);
            Tags.FindGameObjectWithTagInParent(Tags.TrailMarker, name).GetComponent<MeshRenderer>().material.SetColor("_Color", Color.green);
            body.AddComponent<GUIUpdater>().Aircraft = Aircraft;
            var trailerRenderer = Tags.FindGameObjectWithTagInParent(Tags.TrailMarker, name).transform.parent.GetComponent<TrailRenderer>();
            //trailerRenderer.material = Resources.Load("trail_my", typeof (Material)) as Material;
            trailerRenderer.material = Resources.Load("my", typeof(Material)) as Material;
            trailerRenderer.startWidth = trailerRenderer.endWidth = 20;
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
            //trailerRenderer.material = Resources.Load("trail_enemy", typeof(Material)) as Material;
            trailerRenderer.material = Resources.Load("enemy", typeof(Material)) as Material;
            trailerRenderer.startWidth = trailerRenderer.endWidth = 20;
        }
        //Tags.FindGameObjectWithTagInParent(Tags.TrailMarker, name).GetComponent<MeshRenderer>().enabled = true;
	}

    private float timeCounter = 0.0f;
    void Update()
    {
        if (isLocalPlayer)
        {
            Aircraft.aircraftInterpolator.Interpolate(Time.deltaTime);
            float a = 0.0f;
            timeCounter += Time.deltaTime;
            if (timeCounter > Time.fixedDeltaTime)
            {
                a = 1.0f;
                timeCounter = 0.0f;
            }
            string content = Aircraft.Velocity.x.ToString() + " " + Aircraft.Velocity.y.ToString() + " " + Aircraft.q + " " +
                             Aircraft.Theta * Mathf.Rad2Deg + " " + a;
            //Debug.Log(content);
            fileWriter.Write(longitudinal, content);
            content = Aircraft.Velocity.z.ToString() + " " + Aircraft.p.ToString() + " " + Aircraft.r + " " +
                             Aircraft.Phi * Mathf.Rad2Deg + " " + Aircraft.Psi * Mathf.Rad2Deg + " " + a;
            fileWriter.Write(lateral, content);
            content = Aircraft.Eta * Mathf.Rad2Deg + " " + Aircraft.Xi * Mathf.Rad2Deg + " " + Aircraft.Zeta * Mathf.Rad2Deg + " " + Aircraft.Tau + " " + a;
            fileWriter.Write(control, content);
        }
    }

    void FixedUpdate()
    {
        if (FirstResponseReceived && isLocalPlayer && !IsDestroying)
            checkFlightConditions();
    }

    private float minVelocity = 50;
    void checkFlightConditions()
    {
        if (Aircraft.Velocity.x < minVelocity)
        {
            toggleWarning(pullDown, true);
        }
        else
        {
            toggleWarning(pullDown, false);
        }
        if (Aircraft.Velocity.x < 0)
        {
            toggleWarning(lostSteering, true);
            toggleWarning(pullDown, false);
            StartCoroutine(closeGame());
        }
        //if (Aircraft.Theta * Mathf.Rad2Deg > maxAngle)
        //{
        //    toggleWarning(pullDown, true);
        //}
        //else
        //{
        //    toggleWarning(pullDown, false);
        //}

            //if (Aircraft.Theta * Mathf.Rad2Deg < -maxAngle)
            //{
            //    toggleWarning(pullUp, true);
            //}
            //else
            //{
            //    toggleWarning(pullUp, false);
            //}
    }

    void toggleWarning(GameObject panel, bool active)
    {

            panel.GetComponentInChildren<Text>().enabled = active;
            panel.GetComponentInChildren<RawImage>().enabled = active;
    }

    IEnumerator closeGame()
    {
        yield return new WaitForSeconds(3);
        GameObject.FindGameObjectWithTag(Tags.NetworkManager).GetComponent<CustomNetworkManager>().DisconnectFromServer();
        //GameObject.FindGameObjectWithTag(Tags.ApplicationManager).GetComponent<Communication>().Disconnect();
    }
}
