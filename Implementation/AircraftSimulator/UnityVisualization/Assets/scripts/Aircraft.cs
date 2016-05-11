using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.scripts;
using UnityEngine.UI;

public class Aircraft : IAircraft
{

    public GameObject Body;
    public GameObject RudderLeft;
    public GameObject RudderRight;
    public GameObject ElevatorLeft;
    public GameObject ElevatorRight;
    public GameObject AileronLeft;
    public GameObject AileronRight;

    public IDictionary<GameObject, bool> partsInitialized;

    private GameObject roll, pitch, yaw;
    private GameObject aileron, elevator, rudder;
    private Vector3 rotationMaxOffset;
    private const float angle = 20;
    private Quaternion initialInverseRotation;


    #region Velocities
    public Vector3 Velocity;
    public Vector3 Velocity_0;
    public float V_0
    {
        get { return Velocity_0.z; }
    }

    public float V
    {
        get { return Velocity.z; }
    }
    #endregion

    #region Steering
    /// <summary>
    /// Elevator rotation
    /// </summary>
    public float Eta
    {
        get
        {
            var collider = ElevatorRight.GetComponent<ColliderHandler>();
            Quaternion rotation = collider.transform.localRotation * collider.InverseInitialRotation;
            return clampAngle(rotation.eulerAngles.x);
        }
    }
    /// <summary>
    /// Aileron rotation
    /// </summary>
    public float Xi
    {
        get
        {
            var collider = AileronRight.GetComponent<ColliderHandler>();
            Quaternion rotation = collider.transform.localRotation * collider.InverseInitialRotation;
            return clampAngle(rotation.eulerAngles.x);
        }
    }
    /// <summary>
    /// Rudder rotatation
    /// </summary>
    public float Zeta
    {
        get
        {
            var collider = RudderRight.GetComponent<ColliderHandler>();
            Quaternion rotation = collider.transform.localRotation * collider.InverseInitialRotation;
            return clampAngle(rotation.eulerAngles.y);
        }
    }

    public float Tau
    {
        get { return 1; }
    }
    #endregion

    #region aircraft rotations

    /// <summary>
    /// Pitch
    /// </summary>
    public float Theta
    {
        get
        {
            return clampAngle((Body.transform.rotation * initialInverseRotation).eulerAngles.x);
        }
    }
    /// <summary>
    /// Yaw
    /// </summary>
    public float Psi
    {
        get
        {
            return clampAngle((Body.transform.rotation * initialInverseRotation).eulerAngles.y);
        }
    }
    /// <summary>
    /// Roll
    /// </summary>
    public float Phi
    {
        get
        {
            return clampAngle((Body.transform.rotation * initialInverseRotation).eulerAngles.z);
        }
    }

    #endregion

    #region rotary velocities
    public float q
    {
        get; set;
    }
    public float p
    {
        get; set;
    }
    public float r
    {
        get; set;
    }
    #endregion

    public void Initialize()
    {
        rotationMaxOffset = new Vector3(angle, angle, angle);
        Velocity_0 = new Vector3(0, 0, 178);
        initialInverseRotation = Quaternion.Inverse(Body.transform.rotation);

        partsInitialized = new Dictionary<GameObject, bool>();
        partsInitialized.Add(new KeyValuePair<GameObject, bool>(RudderLeft, false));
        partsInitialized.Add(new KeyValuePair<GameObject, bool>(RudderRight, false));
        //partsInitialized.Add(new KeyValuePair<GameObject, bool>(ElevatorLeft, false));
        //partsInitialized.Add(new KeyValuePair<GameObject, bool>(ElevatorRight, false));
        partsInitialized.Add(new KeyValuePair<GameObject, bool>(AileronLeft, false));
        partsInitialized.Add(new KeyValuePair<GameObject, bool>(AileronRight, false));
        foreach (var part in partsInitialized.Keys)
        {
            part.GetComponent<ColliderHandler>().OnInitializeHandler = onInitializeHandler;
            var meshCollider = part.GetComponent<MeshCollider>();
            if (meshCollider != null)
                meshCollider.enabled = true;
        }

        roll = GameObject.FindGameObjectWithTag(Tags.Roll);
        pitch = GameObject.FindGameObjectWithTag(Tags.Pitch);
        yaw = GameObject.FindGameObjectWithTag(Tags.Yaw);

        aileron = GameObject.FindGameObjectWithTag(Tags.Aileron);
        elevator = GameObject.FindGameObjectWithTag(Tags.Elevator);
        rudder = GameObject.FindGameObjectWithTag(Tags.Rudder);
    }


    private  void onInitializeHandler(GameObject gameObject)
    {
        partsInitialized[gameObject] = true;
        foreach (var value in partsInitialized.Values)
        {
            if (!value)
                return;
        }
        //all initialized
        //It is necessary to adjust provided model to initial position
        float angle = -30;
        RotateAileron(angle, false);
        RotateElevator(angle, false);
        
        AileronLeft.GetComponent<ColliderHandler>().InverseInitialRotation = Quaternion.Euler(-angle, 0, 0);
        AileronRight.GetComponent<ColliderHandler>().InverseInitialRotation = Quaternion.Euler(-angle, 0, 0);
        ElevatorLeft.GetComponent<ColliderHandler>().InverseInitialRotation = Quaternion.Euler(-angle, 0, 0);
        ElevatorRight.GetComponent<ColliderHandler>().InverseInitialRotation = Quaternion.Euler(-angle, 0, 0);
        RudderLeft.GetComponent<ColliderHandler>().InverseInitialRotation = Quaternion.identity;
        RudderRight.GetComponent<ColliderHandler>().InverseInitialRotation = Quaternion.identity;

    }

    private void rotateObject(GameObject gameObject, float delta, float maxOffset, bool checkRequired = true)
    {
        var bounds = gameObject.GetComponent<MeshRenderer>().bounds;
        var colliderHandler = gameObject.GetComponent<ColliderHandler>();
        var relativeAxis = colliderHandler.RelativeAxis;
        GameObject g = new GameObject();
        g.transform.position = gameObject.transform.position;
        g.transform.rotation = gameObject.transform.rotation;
        g.transform.RotateAround(colliderHandler.CenterOfRotation, relativeAxis, delta);
        //float angle = Quaternion.Angle(g.transform.rotation * colliderHandler.InverseInitialRotation, Body.transform.rotation);
        float angle = Mathf.Abs((g.transform.rotation*colliderHandler.InverseInitialRotation).eulerAngles.x - Body.transform.eulerAngles.x);
        if (gameObject.tag == Tags.RudderLeft || gameObject.tag == Tags.RudderRight)
        {
            angle = Mathf.Abs((g.transform.rotation * colliderHandler.InverseInitialRotation).eulerAngles.y - Body.transform.eulerAngles.y);
        }
        angle = clampAngle(angle)*Mathf.Rad2Deg;
        angle = Mathf.Abs(angle);
        if (!checkRequired || angle < maxOffset)
        {
            gameObject.transform.RotateAround(colliderHandler.CenterOfRotation, relativeAxis, delta);
        }
        Object.Destroy(g);
    }

    private float getDist(float first)
    {
        var dist1 = Mathf.Abs(first);
        var dist2 = Mathf.Abs(dist1 - 360);
        return dist1 < dist2 ? dist1 : dist2;
    }

    private bool diffAngles(Vector3 first, Vector3 second)
    {
        var distX = getDist(first.x - second.x);
        var distY = getDist(first.y - second.y);
        var distZ = getDist(first.z - second.z);
        if (distX < rotationMaxOffset.x &&
            distY < rotationMaxOffset.y &&
            distZ < rotationMaxOffset.z)
            return true;
        return false;
    }

    public void RotateAileron(float delta, bool checkRequired = true)
    {
        rotateObject(AileronLeft, -delta, rotationMaxOffset.x, checkRequired);
        rotateObject(AileronRight, delta, rotationMaxOffset.x, checkRequired);
    }

    public void RotateRudder(float delta, bool checkRequired = true)
    {
        rotateObject(RudderLeft, -delta, rotationMaxOffset.y, checkRequired);
        rotateObject(RudderRight, delta, rotationMaxOffset.y, checkRequired);
    }

    public void RotateElevator(float delta, bool checkRequired = true)
    {
        rotateObject(ElevatorLeft, delta, rotationMaxOffset.x, checkRequired);
        rotateObject(ElevatorRight, delta, rotationMaxOffset.x, checkRequired);
    }

    public void RotateAircraft(float deltaAileron, float deltaRudder, float deltaElevator)
    {
        RotateAileron(deltaAileron);
        RotateRudder(deltaRudder);
        RotateElevator(deltaElevator);

        string format = "n2";
        aileron.GetComponent<Text>().text = (Xi * Mathf.Rad2Deg).ToString(format);
        elevator.GetComponent<Text>().text = (Eta * Mathf.Rad2Deg).ToString(format);
        rudder.GetComponent<Text>().text = (Zeta * Mathf.Rad2Deg).ToString(format);

        roll.GetComponent<Text>().text = (Phi * Mathf.Rad2Deg).ToString(format);
        pitch.GetComponent<Text>().text = (Theta * Mathf.Rad2Deg).ToString(format);
        yaw.GetComponent<Text>().text = (Psi * Mathf.Rad2Deg).ToString(format);
    }


    private float clampAngle(float angle)
    {
        if (angle >= 180)
            angle = -(360 - angle);
        return angle * Mathf.Deg2Rad;
    }
}
