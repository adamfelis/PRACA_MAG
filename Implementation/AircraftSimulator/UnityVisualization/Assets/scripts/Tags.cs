using UnityEngine;
using System.Collections;

public static class Tags
{
    public static GameObject FindGameObjectWithTagInParent(string tag, string name)
    {
        GameObject[] arr = GameObject.FindGameObjectsWithTag(tag);
        foreach (var gameObject in arr)
        {
            if (gameObject.transform.root.name == name)
                return gameObject;
        }
        return null;
    }

    public static string F35 = "F35";
    public static string F15 = "F15";
    public static string RudderLeft = "RudderLeft";
    public static string RudderRight = "RudderRight";
    public static string ElevatorLeft = "ElevatorLeft";
    public static string ElevatorRight = "ElevatorRight";
    public static string AileronLeft = "AileronLeft";
    public static string AileronRight = "AileronRight";
    public static string Roll = "Roll";
    public static string Pitch = "Pitch";
    public static string Yaw = "Yaw";
    public static string Aileron = "Aileron";
    public static string Elevator = "Elevator";
    public static string Rudder = "Rudder";
    public static string Player = "Player";
    public static string ServerLatency = "ServerLatency";
    public static string NetworkManager = "NetworkManager";
    public static string NetworkIPInput = "NetworkIPInput";
    public static string ButtonJoinGame = "ButtonJoinGame";
    public static string ButtonStartHost = "ButtonStartHost";
    public static string ButtonDisconnect = "ButtonDisconnect";
}
