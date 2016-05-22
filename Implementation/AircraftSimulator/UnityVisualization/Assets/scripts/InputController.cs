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

    public CameraSmoothFollow cameraSmoothFollow;

    void Update()
    {
        Horizontal = horizontalSpeed * Input.GetAxis("Mouse X");
        Vertical = verticalSpeed * Input.GetAxis("Mouse Y");
        //transform.Rotate(Vertical, Horizontal, 0);
        //aircraft.RotateAircraft(Vertical, Horizontal);
        testKeyboard();
        testCameraInput();
        testWheel();

    }

    private void testWheel()
    {
        float intensity = 10.0f;
        var d = Input.GetAxis("Mouse ScrollWheel");
        d *= intensity;
        if (d != 0.0f)
        {
            cameraSmoothFollow.ZoomCamera(-d);
        }
    }

    private void testCameraInput()
    {
        
        if (Input.GetKeyDown(KeyCode.Keypad6))
        {
            cameraSmoothFollow.SmoothTransist(CameraView.RightView);
        }
        if (Input.GetKeyUp(KeyCode.Keypad6))
        {
            cameraSmoothFollow.Interrupt();
        }

        if (Input.GetKeyDown(KeyCode.Keypad4))
        {
            cameraSmoothFollow.SmoothTransist(CameraView.LeftView);
        }
        if (Input.GetKeyUp(KeyCode.Keypad4))
        {
            cameraSmoothFollow.Interrupt();
        }

        if (Input.GetKeyDown(KeyCode.Keypad2))
        {
            cameraSmoothFollow.SmoothTransist(CameraView.FrontView);
        }
        if (Input.GetKeyUp(KeyCode.Keypad2))
        {
            cameraSmoothFollow.Interrupt();
        }
    }

    private Vector3 steeringSensitivity = new Vector3(0.1f, 0.05f, 0.3f);
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
        aircraft.RotateSteers(deltaAileron, deltaRudder, deltaElevator);
    }
}
