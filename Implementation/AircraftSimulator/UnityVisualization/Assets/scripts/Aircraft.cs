﻿using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
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

    private float theta_prev, psi_prev, phi_prev;


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
            return collider.RotationOffset;
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
            return theta_prev*Mathf.Deg2Rad;
        }
    }
    /// <summary>
    /// Yaw
    /// </summary>
    public float Psi
    {
        get
        {
            return psi_prev * Mathf.Deg2Rad;
        }
    }
    /// <summary>
    /// Roll
    /// </summary>
    public float Phi
    {
        get
        {
            return phi_prev * Mathf.Deg2Rad;
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

    private void rotateObject(GameObject gameObject, float delta, bool checkRequired = true)
    {
        var colliderHandler = gameObject.GetComponent<ColliderHandler>();
        colliderHandler.Rotate(delta, checkRequired);
    }

    public void RotateAileron(float delta, bool checkRequired = true)
    {
        rotateObject(AileronLeft, delta, checkRequired);
        rotateObject(AileronRight, delta, checkRequired);
    }

    public void RotateRudder(float delta, bool checkRequired = true)
    {
        rotateObject(RudderLeft, -delta, checkRequired);
        rotateObject(RudderRight, delta, checkRequired);
    }

    public void RotateElevator(float delta, bool checkRequired = true)
    {
        rotateObject(ElevatorLeft, -delta, checkRequired);
        rotateObject(ElevatorRight, -delta, checkRequired);
    }

    public void RotateInLongitudinal(float theta)
    {
        theta *= Mathf.Rad2Deg;
        float delta = theta - theta_prev;
        var rot = new Vector3(delta, 0, 0);
        Body.transform.Rotate(rot);
        theta_prev = theta;
    }

    public void RotateInLateral(float phi, float psi)
    {
        phi *= Mathf.Rad2Deg;
        float delta = phi - phi_prev;
        var rot = new Vector3(0, 0, -delta);
        Body.transform.Rotate(rot);
        phi_prev = phi;

        psi *= Mathf.Rad2Deg;
        delta = psi - psi_prev;
        rot = new Vector3(0, delta, 0);
        Body.transform.Rotate(rot);
        psi_prev = psi;
    }

    public void TranslateInLongitudinal()
    {
        var velocity = new Vector3(Velocity.x, Velocity.y, 0);
        Body.transform.Translate(velocity * Time.fixedDeltaTime, Space.World);
    }

    public void TranslateInLateral()
    {
        var velocity = new Vector3(0, 0, Velocity.z);
        Body.transform.Translate(velocity * Time.fixedDeltaTime, Space.World);
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
}
