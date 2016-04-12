using UnityEngine;
using System.Collections;
using Assets.scripts;


public delegate void OnInitializeHandler(GameObject gameObject);
public class ColliderHandler : MonoBehaviour
{
    public Vector3 RelativeAxis { get; set; }
    public Vector3 InitialRotation { get; set; }
    public OnInitializeHandler OnInitializeHandler { get; set; }

    // Use this for initialization
    void Start ()
	{
	    RelativeAxis = Vector3.forward;
        InitialRotation = Vector3.zero;
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnCollisionEnter(Collision collision)
    {
        Destroy(GetComponent<Rigidbody>());
        Destroy(GetComponent<MeshCollider>());
        var n = collision.contacts.Length;
        Debug.DrawLine(collision.contacts[0].point, collision.contacts[n - 1].point, Color.green, 10000, false);
        var center = collision.contacts[0].point + (collision.contacts[n - 1].point - collision.contacts[0].point)/2;
        //Debug.DrawLine(center, center + Vector3.up, Color.black, 10000, false);
        RelativeAxis = collision.contacts[n - 1].point - collision.contacts[0].point;
        OnInitializeHandler(gameObject);
    }
}
