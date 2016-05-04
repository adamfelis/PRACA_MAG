using System;
using UnityEngine;
using System.Collections;
using Assets.scripts;
using UnityEngine.UI;

public class InputController : MonoBehaviour {

    public float horizontalSpeed = 0.01F;
    public float verticalSpeed = 0.01F;
    public Aircraft aircraft;
    public float Horizontal;
    public float Vertical;

    void Update()
    {

    }

    void LateUpdate()
    {
        Horizontal = horizontalSpeed * Input.GetAxis("Mouse X");
        Vertical = verticalSpeed * Input.GetAxis("Mouse Y");
        //transform.Rotate(Vertical, Horizontal, 0);
        testKeyboard();
    }


    private float steeringSensitivity = 0.3f;
    void testKeyboard()
    {
        float deltaAileron, deltaRudder, deltaElevator;
        deltaAileron = deltaRudder = deltaElevator = 0.0f;
        if (Input.GetKey(KeyCode.W))
        {
            deltaElevator = 1.0f;
        }
        if (Input.GetKey(KeyCode.S))
        {
            deltaElevator = -1.0f;
        }

        if (Input.GetKey(KeyCode.A))
        {
            deltaRudder = 1.0f;
        }
        if (Input.GetKey(KeyCode.D))
        {
            deltaRudder = -1.0f;
        }

        if (Input.GetKey(KeyCode.UpArrow))
        {
            deltaAileron = 1.0f;
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            deltaAileron = -1.0f;
        }


        deltaAileron *= steeringSensitivity;
        deltaRudder *= steeringSensitivity;
        deltaElevator *= steeringSensitivity;
        aircraft.RotateAircraft(deltaAileron, deltaRudder, deltaElevator);
    }
}
