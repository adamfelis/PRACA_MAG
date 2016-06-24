using UnityEngine;
using System.Collections;

public class CameraMovement : MonoBehaviour {

	private float speed = 4000.0f;
    private float zoomSpeed = 1000.0f;

    public float minX = -360.0f;
	public float maxX = 360.0f;
	
	public float minY = -45.0f;
	public float maxY = 45.0f;

	public float sensX = 100.0f;
	public float sensY = 100.0f;
	
	float rotationY = 0.0f;
	float rotationX = 0.0f;

    private Vector3 offset;

    void Start()
    {
        offset = transform.localEulerAngles;
    }

	void Update () {

        float scroll = Input.GetAxis("Mouse ScrollWheel");
        transform.Translate(0, scroll * zoomSpeed, scroll * zoomSpeed, Space.World);

        if (Input.GetKey(KeyCode.D))
        {
            //transform.position += Vector3.right * speed * Time.deltaTime;
            transform.Translate(Vector3.right * speed * Time.deltaTime, Space.Self);
        }
        if (Input.GetKey(KeyCode.A))
        {
            //transform.position += Vector3.left * speed * Time.deltaTime;
            transform.Translate(Vector3.left * speed * Time.deltaTime, Space.Self);
        }
        if (Input.GetKey(KeyCode.W))
        {
            //transform.position += Vector3.forward * speed * Time.deltaTime;
            transform.Translate(Vector3.forward * speed * Time.deltaTime, Space.Self);
        }
        if (Input.GetKey(KeyCode.S))
        {
            //transform.position += Vector3.back * speed * Time.deltaTime;
            transform.Translate(Vector3.back * speed * Time.deltaTime, Space.Self);
        }

        if (Input.GetMouseButton (0)) {
			rotationX += Input.GetAxis ("Mouse X") * sensX * Time.deltaTime;
			rotationY += Input.GetAxis ("Mouse Y") * sensY * Time.deltaTime;
			rotationY = Mathf.Clamp (rotationY, minY, maxY);
			transform.localEulerAngles = new Vector3 (-rotationY, rotationX, 0) + offset;
		}

    }
}
