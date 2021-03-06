﻿using System;
using UnityEngine;
using System.Collections;
using System.Linq;
using Assets.scripts;
using Assets.scripts.Model;
using UnityEngine.UI;

public class InputController : MonoBehaviour {

    public float horizontalSpeed = 0.01F;
    public float verticalSpeed = 0.01F;
    private Aircraft aircraft;
    private CameraSmoothFollow cameraSmoothFollow;
    private MissileController missileController;
    private RawImage battleMap;
    public static bool BattleMapEnabled = false;

    private Vector3 steeringSensitivity = new Vector3(0.1f, 0.05f, 0.3f);
    private bool readKeyboardInput;

    public void Initialize(AircraftsController aircraftsController)
    {
        this.aircraft = aircraftsController.Aircraft;
        this.missileController = aircraftsController.MissileController;
        this.cameraSmoothFollow = Tags.FindGameObjectWithTagInParent(Tags.CameraManager, aircraftsController.name).GetComponent<CameraSmoothFollow>();
        this.battleMap = GameObject.FindGameObjectWithTag(Tags.BattleMap).GetComponent<RawImage>();
        enabled = true;
    }

    private bool isJoystickConnected
    {
        get
        {
            if (Input.GetJoystickNames().Length == 0)
                return false;
            return Input.GetJoystickNames().First().ToString() != String.Empty;
        }
    }

    void Update()
    {
        readKeyboardInput = false;
        testBattleMap();
        if (!battleMap.enabled)
        {
            testKeyboard();
            testCameraInput();
            testWheel();
            if (!readKeyboardInput && isJoystickConnected)
                testJoystick();
        }
        testFreeCamera();
    }

    private const float throttleMinRange = 0.1f;
    private const float throttleMaxRange = 1f;
    private const float throttleLength = throttleMaxRange - throttleMinRange;
    public static float rangeHalfRangeElevator = 10;
    public static float rangeHalfRangeRudder = 2;
    public static float rangeHalfRangeAileron = 2;
    public static float rangeHalfRangeThrottle = 10;
    private void testJoystick()
    {
        float horizontal;
        float vertical;
        float throttle;
        float z;
        float horizontalRange = rangeHalfRangeAileron;
        float verticalRange = rangeHalfRangeElevator;
        float throttleRange = rangeHalfRangeThrottle;
        float zRange = rangeHalfRangeRudder;
        horizontal = horizontalRange * Input.GetAxis("horizontal");
        vertical = verticalRange * Input.GetAxis("vertical");
        throttle = ((1f + Input.GetAxis("Throttle")) / 2f ) * throttleLength + throttleMinRange;
        z = zRange * Input.GetAxis("z");
        //Debug.Log(horizontal);
        //Debug.Log(vertical);
        //Debug.Log(z);
        //Debug.Log(throttle);

        float deltaAileron, deltaRudder, deltaElevator;

        deltaElevator = vertical;
        deltaAileron = horizontal;
        deltaRudder = z;

        aircraft.RotateSteersJoystick(deltaAileron, deltaRudder, -deltaElevator);
        aircraft.Tau = throttle;
    }

    private void testFreeCamera()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            cameraSmoothFollow.toggleFreeCamera();
        }
    }

    private void testBattleMap()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            battleMap.enabled = BattleMapEnabled = !battleMap.enabled;
        }
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

    void testMissileFire()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            missileController.Shoot();
    }

    void testKeyboard()
    {
        testMissileFire();
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
