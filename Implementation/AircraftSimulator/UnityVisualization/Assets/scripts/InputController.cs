using UnityEngine;
using System.Collections;
using Assets.scripts;

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
        aircraft.RotateAircraft(Vertical, Horizontal);
    }
}
