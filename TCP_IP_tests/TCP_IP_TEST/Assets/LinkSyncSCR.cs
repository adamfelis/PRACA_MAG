using System;
using UnityEngine;
using System.Collections;
using System.Globalization;
using System.Runtime.InteropServices;
using SharpConnect;
using System.Security.Permissions;
using System.Text;

public class LinkSyncSCR : MonoBehaviour
{
    public Connector test = new Connector();
    string lastMessage;
    private string sNetIP = "127.0.0.1";
    private int iPORT_NUM = 10000;//IT MUST BE
    private float[] tspan;
    private float[] y0;
    private const float timeStep = 0.03f;
    private float startTime = 0.0f;
    void Start()
    {
        Debug.Log(test.Connect(sNetIP, iPORT_NUM, System.Environment.MachineName));
        tspan = new[] {startTime, startTime + timeStep};
        y0 = new float[9];
        y0[0] = 1;
        y0[4] = 1;
        y0[8] = 1;
        test.Send(ComposeRequest());
    }
    void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            Debug.Log("space key was pressed");
            test.Send("space key was pressed");
        }

        if (Input.GetKeyDown("escape"))
        {
            Debug.Log("escape key was pressed");
            test.Send("escape key was pressed");
        }
        if (test.strMessage != "JOIN")
        {
            if (test.res != lastMessage)
            {
                Debug.Log(test.res);
                lastMessage = test.res;
            }
        }
        if (test.strMessage.Contains("DATA"))
        {
            string[] msg = test.strMessage.Split('|');
            deserializeData(msg[1]);
            test.Send(ComposeRequest());
        }
        
    }

    void deserializeData(string msg)
    {
        string[] data = msg.Split(';');
        for (int i = 0; i < data.Length; i++)
        {
            y0[i] = (float)Double.Parse(data[i].Replace(',','.'));
        }
        tspan[0] = tspan[1];
        tspan[1] += timeStep;
        updateCubePosition();
    }

    private void updateCubePosition()
    {
        Vector3 startPos = GameObject.Find("Cube").transform.position;
        Vector3 endPos = startPos + new Vector3(y0[0], y0[1], y0[2]) * timeStep;
        Debug.DrawLine(startPos, endPos, Color.black, 10000);
        GameObject.Find("Cube").transform.position = endPos;
    }


    private string ComposeRequest()
    {
        return parseData("tspan=", tspan) + parseData("y0=", y0);
    }

    private string parseData(string prefix, float[] container)
    {
        StringBuilder t = new StringBuilder(prefix);//"tspan=0;1\n";
        for (int i = 0; i < container.Length; i++)
        {
            t.Append(container[i]);
            if (i < container.Length - 1)
                t.Append(';');
        }
        t.Append('\n');
        return t.ToString();
    }

    void OnApplicationQuit()
    {
        try { test.fnDisconnect(); }
        catch { }
    }
}