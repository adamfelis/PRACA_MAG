using UnityEngine;
using System.Collections;
using Assets.scripts;


public delegate void OnInitializeHandler(GameObject gameObject);
public class ColliderHandler : MonoBehaviour
{
    public GameObject First;
    public GameObject Second;
    private bool collided = false;
    public Quaternion InverseInitialRotation { get; set; }
    public OnInitializeHandler OnInitializeHandler { get; set; }

    public Vector3 RelativeAxis
    {
        get
        {
            return Second.transform.position - First.transform.position;
        }
    }

    public Vector3 CenterOfRotation
    {
        get
        {
            if (!collided)
                return gameObject.GetComponent<MeshRenderer>().bounds.center;
            return First.transform.position + (Second.transform.position - First.transform.position) / 2.0f;
        }
    }

    // Use this for initialization
    void Start ()
	{
        InverseInitialRotation = Quaternion.identity;
        First = new GameObject("first");
        First.transform.parent = transform;
        Second = new GameObject("second");
        Second.transform.parent = transform;
        First.transform.position = Vector3.zero;
        Second.transform.position = Vector3.forward;
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnCollisionEnter(Collision collision)
    {
        Destroy(GetComponent<Rigidbody>());
        Destroy(GetComponent<MeshCollider>());
        collided = true;
        var n = collision.contacts.Length;
        Vector3 first;
        Vector3 second;
        if (n == 3)
        {
            first = collision.contacts[0].point;
            second = collision.contacts[2].point;
        }
        else
        {
            first = collision.contacts[1].point;
            second = collision.contacts[2].point;
        }
        //CenterOfRotation = first + (second - first) / 2;
        Debug.DrawLine(first, second, Color.green, 10000, false);
        //Debug.DrawLine(center, center + Vector3.up, Color.black, 10000, false);
        //RelativeAxis = second - first;
        First.transform.position = first;
        Second.transform.position = second;
        OnInitializeHandler(gameObject);
    }
}
