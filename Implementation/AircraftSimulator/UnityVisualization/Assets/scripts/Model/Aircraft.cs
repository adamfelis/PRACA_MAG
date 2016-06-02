using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Assets.scripts;


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

    private Vector3 rotationMaxOffset;
    private const float angle = 40;

    private float theta_prev, psi_prev, phi_prev;

    public IAircraftInterpolator aircraftInterpolator;

    #region Initial conditions
    public Vector3 Velocity_0;
    public float Theta_0;
    public float Psi_0;
    public float Phi_0;
    #endregion

    #region Velocities
    public Vector3 Velocity
    {
        get
        {
            //return Velocity.z;
            return new Vector3(
                aircraftInterpolator.TargetVelocityX,
                aircraftInterpolator.TargetVelocityY,
                aircraftInterpolator.TargetVelocityZ);
        }
    }

    public float V_0
    {
        get { return Velocity_0.z; }
    }
    public float V_e
    {
        get { return aircraftInterpolator.TargetVelocityX; }
    }
    #endregion

    #region Positions
    public Vector3 Position
    {
        get
        {
            return new Vector3(
                -Body.transform.position.z,
                Body.transform.position.y,
                Body.transform.position.x
                );
        }
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
            //return (float)Math.Round(collider.RotationOffset, 2);
            return collider.RotationOffset;
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
            //return (float)Math.Round(collider.RotationOffset, 2);
            return collider.RotationOffset;
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
            //return (float)Math.Round(collider.RotationOffset, 2);
            return collider.RotationOffset;
        }
    }

    public float Tau
    {
        get { return 0; }
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
            return trimValueToRange(aircraftInterpolator.TargetTheta * Mathf.Deg2Rad);
        }
    }
    /// <summary>
    /// Yaw
    /// </summary>
    public float Psi
    {
        get
        {
            return trimValueToRange(aircraftInterpolator.TargetPsi * Mathf.Deg2Rad);
        }
    }
    /// <summary>
    /// Roll
    /// </summary>
    public float Phi
    {
        get
        {
            return trimValueToRange(aircraftInterpolator.TargetPhi * Mathf.Deg2Rad);
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



    private float trimValueToRange(float value)
    {
        return value;

        //float rangeMin = 0;
        //float rangeMax = 2*Mathf.PI;
        float rangeMin = -Mathf.PI;
        float rangeMax = Mathf.PI;
        //var clamped = Mathf.Clamp(value, rangeMin, rangeMax);
        float trimmed = 0.0f;
        //float eps = 0.0001f;
        if (value > rangeMax)
        {
            trimmed = value % rangeMax;
            trimmed += rangeMin;
        }else if (value < rangeMin)
        {
            trimmed = -value%(-rangeMin);
            trimmed = rangeMax - trimmed;
        }
        else
        {
            trimmed = value;
        }
        return trimmed;
    }

    private void initializeFlightConditions()
    {
        rotationMaxOffset = new Vector3(angle, angle, angle);
        Velocity_0 = new Vector3(0, 0, 178);
        Theta_0 = 9.4f;
        Psi_0 = 0.0f;
        Phi_0 = 0.0f;
        aircraftInterpolator.SetupInitial(Theta_0, Phi_0, Psi_0);
    }

    public void Initialize(SceneController sceneController)
    {
        aircraftInterpolator = new AircraftInterpolator(Body, sceneController);

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

        initializeFlightConditions();
        initializeSteers();
    }

    private void initializeSteers()
    {
        AileronLeft.GetComponent<ColliderHandler>().Initialize(rotationMaxOffset.x);
        AileronRight.GetComponent<ColliderHandler>().Initialize(rotationMaxOffset.x);
        RudderLeft.GetComponent<ColliderHandler>().Initialize(rotationMaxOffset.y);
        RudderRight.GetComponent<ColliderHandler>().Initialize(rotationMaxOffset.y);
        ElevatorLeft.GetComponent<ColliderHandler>().Initialize(rotationMaxOffset.x);
        ElevatorRight.GetComponent<ColliderHandler>().Initialize(rotationMaxOffset.x);
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
        rotateObject(AileronLeft, -angle, false);
        rotateObject(AileronRight, angle, false);
        RotateElevator(-angle, false);
        
        AileronLeft.GetComponent<ColliderHandler>().InverseInitialRotation = Quaternion.Euler(-angle, 0, 0);
        AileronRight.GetComponent<ColliderHandler>().InverseInitialRotation = Quaternion.Euler(-angle, 0, 0);
        ElevatorLeft.GetComponent<ColliderHandler>().InverseInitialRotation = Quaternion.Euler(-angle, 0, 0);
        ElevatorRight.GetComponent<ColliderHandler>().InverseInitialRotation = Quaternion.Euler(-angle, 0, 0);
        RudderLeft.GetComponent<ColliderHandler>().InverseInitialRotation = Quaternion.identity;
        RudderRight.GetComponent<ColliderHandler>().InverseInitialRotation = Quaternion.identity;

    }

    private void rotateObject(GameObject gameObject, float delta, bool checkRequired = true,  bool fromKeyboard = true)
    {
        var colliderHandler = gameObject.GetComponent<ColliderHandler>();
        colliderHandler.Rotate(delta, checkRequired, fromKeyboard);
    }

    public void RotateAileron(float delta, bool checkRequired = true, bool fromKeyboard = true)
    {
        rotateObject(AileronLeft, delta, checkRequired, fromKeyboard);
        rotateObject(AileronRight, delta, checkRequired, fromKeyboard);
    }

    public void RotateRudder(float delta, bool checkRequired = true, bool fromKeyboard = true)
    {
        rotateObject(RudderLeft, -delta, checkRequired, fromKeyboard);
        rotateObject(RudderRight, delta, checkRequired, fromKeyboard);
    }

    public void RotateElevator(float delta, bool checkRequired = true, bool fromKeyboard = true)
    {
        rotateObject(ElevatorLeft, -delta, checkRequired, fromKeyboard);
        rotateObject(ElevatorRight, -delta, checkRequired, fromKeyboard);
    }

    public void RotateInLongitudinal(float theta)
    {
        theta *= Mathf.Rad2Deg;
        aircraftInterpolator.TargetTheta = theta;
    }

    public void RotateInLateral(float phi, float psi)
    {
        phi *= Mathf.Rad2Deg;
        psi *= Mathf.Rad2Deg;
        aircraftInterpolator.TargetPhi = phi;
        aircraftInterpolator.TargetPsi = psi;
    }

    public void TranslateInLongitudinal(float velocityX, float velocityY)
    {
        aircraftInterpolator.TargetVelocityX = velocityX;
        aircraftInterpolator.TargetVelocityY = velocityY;
    }

    public void TranslateInLateral(float velocityZ)
    {
        aircraftInterpolator.TargetVelocityZ = velocityZ;
    }

    public void RotateSteersJoystick(float deltaAileron, float deltaRudder, float deltaElevator)
    {
        RotateAileron(deltaAileron, true, false);
        RotateRudder(deltaRudder, true, false);
        RotateElevator(deltaElevator, true, false);
    }

    public void RotateSteersKeyboard(float deltaAileron, float deltaRudder, float deltaElevator)
    {
        RotateAileron(deltaAileron);
        RotateRudder(deltaRudder);
        RotateElevator(deltaElevator);
    }
}
