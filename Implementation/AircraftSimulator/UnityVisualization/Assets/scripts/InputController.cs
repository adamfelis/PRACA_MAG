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
        //aircraft.RotateAircraft(Vertical, Horizontal);
        testKeyboard();
    }


    private Vector3 steeringSensitivity = new Vector3(0.3f, 0.05f, 0.3f);
    void testKeyboard()
    {
        float deltaAileron, deltaRudder, deltaElevator;
        deltaAileron = deltaRudder = deltaElevator = 0.0f;

        #region pitching
        if (Input.GetKey(KeyCode.W))
        {
            deltaElevator = 1.0f;
        }
        if (Input.GetKey(KeyCode.S))
        {
            deltaElevator = -1.0f;
        }
        #endregion

        #region yawing
        if (Input.GetKey(KeyCode.RightArrow))
        {
            deltaRudder = -1.0f;
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            deltaRudder = 1.0f;
        }
        #endregion


        #region rolling
        if (Input.GetKey(KeyCode.A))
        {
            deltaAileron = 1.0f;
        }
        if (Input.GetKey(KeyCode.D))
        {
            deltaAileron = -1.0f;
        }
        #endregion



        deltaAileron *= steeringSensitivity.x;
        deltaRudder *= steeringSensitivity.y;
        deltaElevator *= steeringSensitivity.z;
        aircraft.RotateAircraft(deltaAileron, deltaRudder, deltaElevator);
    }
}
