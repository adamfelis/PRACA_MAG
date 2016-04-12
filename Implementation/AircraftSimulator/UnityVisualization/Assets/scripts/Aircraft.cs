﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.scripts;

public class Aircraft : IAircraft
{

    public GameObject Body { get; set; }
    public GameObject RudderLeft { get; set; }
    public GameObject RudderRight { get; set; }
    public GameObject ElevatorLeft { get; set; }
    public GameObject ElevatorRight { get; set; }
    public GameObject AileronLeft { get; set; }
    public GameObject AileronRight { get; set; }

    public Vector3 Velocity;

    /// <summary>
    /// Angle of attack
    /// </summary>
    public float Ni
    {
        get
        {
            var collider = ElevatorRight.GetComponent<ColliderHandler>();
            var initial = collider.InitialRotation;
            var current = collider.transform.rotation.eulerAngles;
            return getDist(initial.x - current.x) * Mathf.Deg2Rad;
        }
    }

    public IDictionary<GameObject, bool> partsInitialized;

    private Vector3 rotationMaxOffset;
    private const float angle = 30;


    public void Initialize()
    {
        rotationMaxOffset = new Vector3(angle, angle, angle);
        Velocity = new Vector3(1000, 0, 0);

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
        RotateAileron(-30, false);
        RotateElevator(-30, false);
        foreach (var part in partsInitialized.Keys)
        {
            part.GetComponent<ColliderHandler>().InitialRotation = part.transform.rotation.eulerAngles;
        }
        ElevatorLeft.GetComponent<ColliderHandler>().InitialRotation = ElevatorLeft.transform.rotation.eulerAngles;
        ElevatorRight.GetComponent<ColliderHandler>().InitialRotation = ElevatorRight.transform.rotation.eulerAngles;
    }

    private void rotateObject(GameObject gameObject, float delta, bool checkRequired = true)
    {
        var bounds = gameObject.GetComponent<MeshRenderer>().bounds;
        var colliderHandler = gameObject.GetComponent<ColliderHandler>();
        var relativeAxis = colliderHandler.RelativeAxis;
        GameObject g = new GameObject();
        g.transform.position = gameObject.transform.position;
        g.transform.rotation = gameObject.transform.rotation;
        g.transform.RotateAround(bounds.center, relativeAxis, delta);
        if (!checkRequired || diffAngles(g.transform.rotation.eulerAngles, colliderHandler.InitialRotation))
        {
            gameObject.transform.RotateAround(bounds.center, relativeAxis, delta);
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
        rotateObject(AileronLeft, delta, checkRequired);
        rotateObject(AileronRight, delta, checkRequired);
        //var r = AileronLeft.transform.rotation;
        //var t = relativePoint - v.center;
    }

    public void RotateRudder(float delta, bool checkRequired = true)
    {
        rotateObject(RudderLeft, -delta, checkRequired);
        rotateObject(RudderRight, delta, checkRequired);
    }

    public void RotateElevator(float delta, bool checkRequired = true)
    {
        rotateObject(ElevatorLeft, delta, checkRequired);
        rotateObject(ElevatorRight, delta, checkRequired);
    }

    public void RotateAircraft(float vertical, float horizontal)
    {
        RotateAileron(vertical);
        RotateRudder(horizontal);
        RotateElevator(vertical);
    }
}
