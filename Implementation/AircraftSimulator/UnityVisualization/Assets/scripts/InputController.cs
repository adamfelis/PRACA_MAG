﻿using UnityEngine;
using System.Collections;
using Assets.scripts;

public class InputController : MonoBehaviour {

    public float horizontalSpeed = 1.0F;
    public float verticalSpeed = 1.0F;
    public IAircraft aircraft;
    public float Horizontal;
    public float Vertical;
    void Update()
    {

    }

    void LateUpdate()
    {
        Horizontal = horizontalSpeed * Input.GetAxis("Mouse X");
        Vertical = verticalSpeed * Input.GetAxis("Mouse Y");
        //transform.Rotate(v, h, 0);
        aircraft.RotateAircraft(Vertical, Horizontal);

    }
}
