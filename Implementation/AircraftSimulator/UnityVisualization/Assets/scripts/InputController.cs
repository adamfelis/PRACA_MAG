using System;
using UnityEngine;
using System.Collections;
using System.Linq;
using Assets.scripts;
using UnityEngine.UI;

public class InputController : MonoBehaviour {

    public float horizontalSpeed = 0.01F;
    public float verticalSpeed = 0.01F;
    public Aircraft aircraft;

    public CameraSmoothFollow cameraSmoothFollow;

    private Vector3 steeringSensitivity = new Vector3(0.1f, 0.05f, 0.3f);
    private bool readKeyboardInput;

    private bool isJoystickConnected
    {
        get { return Input.GetJoystickNames().First().ToString() != String.Empty; }
    }

    void Update()
    {
        readKeyboardInput = false;
        testKeyboard();
        testCameraInput();
        testWheel();
        if (!readKeyboardInput && isJoystickConnected)
            testJoystick();
    }

    private void testJoystick()
    {
        float horizontal;
        float vertical;
        float z;
        float rangeHalfRange = 20;
        float horizontalRange = rangeHalfRange;
        float verticalRange = rangeHalfRange;
        float zRange = rangeHalfRange;
        horizontal = horizontalRange * Input.GetAxis("horizontal");
        vertical = verticalRange * Input.GetAxis("vertical");
        z = zRange * Input.GetAxis("z");
        //Debug.Log(horizontal);
        //Debug.Log(vertical);
        //Debug.Log(z);

        float deltaAileron, deltaRudder, deltaElevator;

        deltaElevator = vertical;
        deltaAileron = horizontal;
        deltaRudder = z;

        aircraft.RotateSteersJoystick(deltaAileron, deltaRudder, deltaElevator);
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
        float eps = 0.0001f;
        if (Mathf.Abs(deltaAileron) > eps ||
            Mathf.Abs(deltaRudder) > eps ||
            Mathf.Abs(deltaElevator) > eps)
        {
            aircraft.RotateSteersKeyboard(deltaAileron, deltaRudder, deltaElevator);
            readKeyboardInput = true;
        }
    }
}
