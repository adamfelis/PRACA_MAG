using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;
using SharpConnect;
using System.Security.Permissions;

public class LinkSyncSCR : MonoBehaviour
{
    public Connector test = new Connector();
    string lastMessage;
    private string sNetIP = "127.0.0.1";
    private int iPORT_NUM = 10000;//IT MUST BE

    void Start()
    {
        Debug.Log(test.Connect(sNetIP, iPORT_NUM, System.Environment.MachineName));
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
        
        //test.fnPacketTest(PlayerCoord.position[0] + "," + PlayerCoord.position[1] + "," + PlayerCoord.position[2]);
    }

    void OnApplicationQuit()
    {
        try { test.fnDisconnect(); }
        catch { }
    }
}