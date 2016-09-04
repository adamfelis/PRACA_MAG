using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.scripts;

public delegate void NewCenterHandler(Vector3 center);
public class CloudCreator : MonoBehaviour, ICloudCreator
{
    public event NewCenterHandler NewCenterReached;
    Vector3 offset = new Vector3(10.0f, 10.0f, 10.0f);
    float cameraFarPlan = 5000.0f;
    private System.Random r;
    private IList<GameObject> cloudContainers;
    private int bottomLayer = -1;
    private int upLayer = 1;
    // Use this for initialization
    void Start ()
	{
         r = new System.Random();
        cloudContainers = new List<GameObject>();
        for (int i = -1; i <= 1; i++)
            for (int j = bottomLayer; j <= upLayer; j++)
                for (int k = -1; k <= 1; k++)
                {
                    GameObject instance = (GameObject) Instantiate(Resources.Load("cloudPrefab"));
                    instance.transform.parent = transform;
                    instance.transform.position = new Vector3(cameraFarPlan * i, cameraFarPlan * j, cameraFarPlan * k);
                    cloudContainers.Add(instance);

                    CloudsToy cloudToy;
                    cloudToy = instance.transform.FindChild("CloudsToy Mngr").GetComponent<CloudsToy>();
                    cloudToy.Side = new Vector3(cameraFarPlan, cameraFarPlan, cameraFarPlan);
                    cloudToy.DisappearMultiplier = 1000;
                    cloudToy.MaxWithCloud = 600;
                    cloudToy.MaxDepthCloud = 600;
                    cloudToy.MaxTallCloud = 300;
                    cloudToy.NumberClouds = 8;
                    if (j == -1 || j == 1)
                    {
                        //cloudToy.NumberClouds = 0;
                    }
                    else
                    {
                    }

                    cloudToy.IsAnimate = false;
                    instance.AddComponent<BoxCollider>().size = cloudToy.Side;
                    instance.GetComponent<BoxCollider>().isTrigger = true;


                    instance.AddComponent<CollisionDetection>().CloudCreator = this;
                    bool a = true;
                    drawCube(cloudToy);
                }
	}

    public void NotifyCenterChanged(Transform transform)
    {
        IList<Vector3> newPositions = generateNewPositions(transform);
        IList<Vector3> toPositions = new List<Vector3>(newPositions);
        IList<GameObject> toReposition = getIncorrectlyPlaced(newPositions, ref toPositions);
        replaceIncorrected(toPositions, toReposition);
    }

    private void replaceIncorrected(IList<Vector3> toPositions, IList<GameObject> toReposition)
    {
        for (int i = 0; i < toPositions.Count; i++)
        {
            toReposition[i].transform.position = toPositions[i];
        }
    }

    private IList<GameObject> getIncorrectlyPlaced(IList<Vector3> newPositions, ref IList<Vector3> toPositions)
    {
        IList<GameObject> toReposition = new List<GameObject>();
        var eps = 0.1f;
        foreach (var cloudContainer in cloudContainers)
        {
            bool sentinel = false;
            foreach (var newPosition in newPositions)
            {
                if (Mathf.Abs(Vector3.Distance(cloudContainer.transform.position, newPosition)) < eps)
                {
                    sentinel = true;
                    toPositions.Remove(newPosition);
                    break;
                }
            }
            if (!sentinel)
                toReposition.Add(cloudContainer);
        }
        return toReposition;
    }

    private IList<Vector3> generateNewPositions(Transform transform)
    {
        Vector3 center = transform.position;
        var cloudToy =  transform.gameObject.GetComponent<CloudsToy>();
        IList<Vector3> newPositions = new List<Vector3>();
        
        for (int i = -1; i <= 1; i++)
            for (int j = bottomLayer; j <= upLayer; j++)
                for (int k = -1; k <= 1; k++)
                {
                    var newPosition = new Vector3(cloudToy.Side.x * i, cloudToy.Side.y * j, cloudToy.Side.z * k) + center;
                    newPositions.Add(newPosition);
                }
        return newPositions;
    }

    void drawCube(CloudsToy cloudToy)
    {
        byte[] bytes = new byte [3];
        r.NextBytes(bytes);
        IList<Vector3> points = new List<Vector3>();
        Vector3 center = cloudToy.transform.position;
        for (int i = -1; i <= 1; i += 2)
        {
            float x = center.x + i * cloudToy.Side.x / 2;
            for (int j = bottomLayer; j <= upLayer; j += 2)
            {
                float y = center.y + j * cloudToy.Side.y / 2;
                for (int k = -1; k <= 1; k += 2)
                {
                    float z = center.z + k * cloudToy.Side.z / 2;
                    Vector3 start = new Vector3(x, y, z);
                    points.Add(start);
                    Vector3 end = center;

                    //Debug.DrawLine(start, end, Color.red, 10000, false);
                }
            }
        }
        var eps = 0.001;
        foreach (var p1 in points)
        {
            foreach (var p2 in points)
            {
                if (Mathf.Abs(Vector3.Distance(p1, p2) - cameraFarPlan) < eps)
                {
                    Debug.DrawLine(p1 + offset, p2+ offset, new Color(bytes[0] /(float)255.0f, bytes[1] / (float)255.0f, bytes[2] / (float)255.0f), 10000, false);
                }
            }
        }
    }
	
}
