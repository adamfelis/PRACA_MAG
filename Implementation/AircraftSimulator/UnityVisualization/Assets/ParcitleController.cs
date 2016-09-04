using UnityEngine;
using System.Collections;

public class ParcitleController : MonoBehaviour
{

    private EllipsoidParticleEmitter emitter;
    private Aircraft aircraft;
    private Rigidbody rigidbody;
	// Use this for initialization
	void Start () {
        emitter = GetComponent<EllipsoidParticleEmitter>();
	    aircraft = transform.root.GetComponent<AircraftsController>().Aircraft;
        rigidbody = transform.root.GetComponent<Rigidbody>();

    }

    private Vector3 prevPosition ;
	// Update is called once per frame
	void Update () {
        //emitter.worldVelocity = aircraft.
	    var currentPosition = aircraft.Body.transform.position;
	    var v = (currentPosition - prevPosition)/Time.deltaTime;
	    prevPosition = currentPosition;
	    Debug.Log(v);
	    emitter.worldVelocity = v;
	}
}
