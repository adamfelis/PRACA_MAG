using System;
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
    private float rotationOffset = 0;
    private float rotationMaxOffset;
    public OnInitializeHandler OnInitializeHandler { get; set; }

    public float RotationOffset
    {
        get
        {
            return rotationOffset * Mathf.Deg2Rad;
            return refinedRotationOffset() * Mathf.Deg2Rad;
        }
    }

    private Quaternion targetRotation;

    private Vector3 RelativeAxis
    {
        get
        {
            return Second.transform.position - First.transform.position;
        }
    }

    private Vector3 CenterOfRotation
    {
        get
        {
            if (!collided)
                return gameObject.GetComponent<MeshRenderer>().bounds.center;
            return First.transform.position + (Second.transform.position - First.transform.position) / 2.0f;
        }
    }

    public void Initialize(float rotationMaxOffset)
    {
        this.rotationMaxOffset = rotationMaxOffset;
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
        Second.transform.position = Vector3.right;
        //TargetRotation = Quaternion.identity;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    private float refinedRotationMaxOffset = 0.0f;
    private float refinedRotationMinOffset = 0.0f;
    private float refinedRotationPrevious = 0.0f;
    private bool isIncreasing = false;
    private bool previousIncreasing = false;
    private float previousToRet = 0.0f;
    private float beginOffset = 0.0f;

    private const bool DEBUG = false;

    private float refinedRotationOffset()
    {
        float toRet = 0.0f;
        bool sentinel = false;
        bool increasing = false;
        bool equal = rotationOffset == refinedRotationPrevious;
        if (!equal)
        {
            increasing = false || rotationOffset > refinedRotationPrevious;
        }

        if (DEBUG && tag == Tags.ElevatorRight)
            Debug.Log("111111111MINOffset: " + refinedRotationMinOffset +
                      " MAXOffset: " + refinedRotationMaxOffset +
                      " rotationOffset: " + rotationOffset +
                      " refinedRotationPREV: " + refinedRotationPrevious +
                      " equal: " + equal +
                      " beginOffset: " + beginOffset +
                      " increasing: " + increasing +
                      " previousIncreasing: " + previousIncreasing);

        if (equal)
        {
            toRet = previousToRet;
            sentinel = true;
            if (DEBUG && tag == Tags.ElevatorRight)
                Debug.Log("g: " + toRet);
        }
        else
        {
            if (increasing != previousIncreasing && Math.Abs(rotationOffset) > 1.0f)
            {
                //if (rotationOffset < 0)
                //    beginOffset = refinedRotationMinOffset;
                //else
                //{
                //    beginOffset = refinedRotationMaxOffset;
                //}
                if (previousIncreasing)
                {
                    beginOffset = refinedRotationMaxOffset;
                }
                else
                {
                    beginOffset = refinedRotationMinOffset;
                }
            }

            if (DEBUG && tag == Tags.ElevatorRight)
                Debug.Log("22222222MINOffset: " + refinedRotationMinOffset +
                          " MAXOffset: " + refinedRotationMaxOffset +
                          " rotationOffset: " + rotationOffset +
                          " beginOffset: " + beginOffset);
            if (increasing)
            {
                if (rotationOffset < 0)
                {
                    if (DEBUG && tag == Tags.ElevatorRight)
                        Debug.Log("a: " + refinedRotationMinOffset);
                    toRet = refinedRotationMinOffset;
                }
                else if (rotationOffset > 0.0f)
                {
                    toRet = scale(0, rotationMaxOffset, rotationOffset, beginOffset, rotationMaxOffset);
                    refinedRotationMaxOffset = toRet;
                    if (DEBUG && tag == Tags.ElevatorRight)
                        Debug.Log("b: " + toRet);
                }
                else
                {
                    toRet = previousToRet;
                    if (DEBUG && tag == Tags.ElevatorRight)
                        Debug.Log("e: " + toRet);
                }
            }
            else
            {
                if (rotationOffset > 0)
                {
                    if (DEBUG && tag == Tags.ElevatorRight)
                        Debug.Log("c: " + refinedRotationMaxOffset);
                    toRet = refinedRotationMaxOffset;
                }
                else if (rotationOffset < 0.0f)
                {
                    toRet = scale(0, -rotationMaxOffset, rotationOffset, beginOffset, -rotationMaxOffset);
                    refinedRotationMinOffset = toRet;
                    if (DEBUG && tag == Tags.ElevatorRight)
                        Debug.Log("d: " + toRet);
                }
                else
                {
                    toRet = previousToRet;
                    if (DEBUG && tag == Tags.ElevatorRight)
                        Debug.Log("f: " + toRet);
                }
            }
            previousIncreasing = increasing;
        }
        //if (!sentinel)
        //    previousToRet = toRet;
        previousToRet = toRet;
        refinedRotationPrevious = rotationOffset;
        return toRet;
    }

    float scale(float a, float b, float v, float minOffset, float maxOffset)
    {
        float val = Mathf.Abs(v - a) / Mathf.Abs(b - a);
        float toRet = val*(maxOffset - minOffset) + minOffset;
        return toRet;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.transform.root.name != transform.root.name)
            return;

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

    //public void Rotate(float delta, bool checkRequired = true)
    //{
    //    if (checkRequired)
    //    {
    //        var oldRotationOffset = rotationOffset;
    //        rotationOffset += delta;
    //        rotationOffset = Mathf.Clamp(rotationOffset, -rotationMaxOffset, rotationMaxOffset);
    //        delta = rotationOffset - oldRotationOffset;
    //    }
    //    float eps = 0.00001f;
    //    if (Mathf.Abs(delta) > eps)
    //        transform.RotateAround(CenterOfRotation, RelativeAxis, delta);
    //}


    public void Rotate(float delta, bool checkRequired = true, bool fromKeyboard = true)
    {
        if (fromKeyboard)
            rotateKeyboard(delta, checkRequired);
        else
        {
            rotateJoystick(delta, checkRequired);
        }
    }

    private void rotateKeyboard(float delta, bool checkRequired = true)
    {
        if (checkRequired)
        {
            var oldRotationOffset = rotationOffset;
            rotationOffset += delta;
            rotationOffset = Mathf.Clamp(rotationOffset, -rotationMaxOffset, rotationMaxOffset);
            delta = rotationOffset - oldRotationOffset;
        }
        float eps = 0.00001f;
        if (Mathf.Abs(delta) > eps)
            transform.RotateAround(CenterOfRotation, RelativeAxis, delta);
    }

    private float timeFromLastChange = 0.0f;
    private float prevDelta = 0.0f;
    private void rotateJoystick(float delta, bool checkRequired = true)
    {
        float eps = 0.0001f;
        float minFraction = 0.1f;
        float maxFraction = 0.5f;
        float fractionInterpolatationTime = 50.0f;
        if (checkRequired)
        {

            if (Mathf.Abs(delta - prevDelta) < eps)
                timeFromLastChange += Time.deltaTime;
            else
            {
                timeFromLastChange = 0.0f;
            }
            prevDelta = delta;
            var oldRotationOffset = rotationOffset;
            var fraction = Mathf.Lerp(minFraction, maxFraction, timeFromLastChange / fractionInterpolatationTime);
            rotationOffset = Mathf.Lerp(rotationOffset, delta, fraction);
            rotationOffset = Mathf.Clamp(rotationOffset, -rotationMaxOffset, rotationMaxOffset);
            delta = rotationOffset - oldRotationOffset;
        }
        if (Mathf.Abs(delta) > eps)
            transform.RotateAround(CenterOfRotation, RelativeAxis, delta);
    }
}
