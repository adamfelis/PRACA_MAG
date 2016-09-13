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

    #region model
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
    public static string WingsMain = "WingsMain";

    public static string MisilleLeft2 = "MisilleLeft2";
    public static string MisilleRight2 = "MisilleRight2";
    public static string MisilleLeft1 = "MisilleLeft1";
    public static string MisilleRight1 = "MisilleRight1";
    #endregion


    #region controls ui - menu scene
    public static string InputLocalAddress = "InputLocalAddress";
    public static string ButtonStartLocal = "ButtonStartLocal";
    public static string ButtonStartRemote = "ButtonStartRemote";
    public static string ButtonJoinLocal = "ButtonJoinLocal";
    public static string ButtonJoinRemote = "ButtonJoinRemote";
    public static string InputRoom = "InputRoom";
    public static string DropdownRemoteAddress = "DropdownRemoteAddress";
    public static string ButtonDisconnect = "ButtonDisconnect";
    public static string ServerLatency = "ServerLatency";
    #endregion

    #region ui - main scene
    public static string MiniMap = "MiniMap";
    public static string VelocityForwardU = "VelocityForwardU";
    public static string VelociyRightV = "VelociyRightV";
    public static string VelocityDownW = "VelocityDownW";
    public static string PositionX = "PositionX";
    public static string PositionY = "PositionY";
    public static string PositionZ = "PositionZ";

    public static string LongitudinalRotation = "LongitudinalRotation";
    public static string LongitudinalTranslation = "LongitudinalTranslation";
    public static string LateralRotation = "LateralRotation";
    public static string LateralTranslation = "LateralTranslation";

    public static string Aim = "Aim";
    public static string Canvas = "Canvas";
    public static string BattleMap = "BattleMap";
    public static string PullUp = "PullUp";
    public static string PullDown = "PullDown";
    public static string WarningDestroyed = "WarningDestroyed";
    public static string WarningHit = "WarningHit";
    public static string WarningLostSteering = "WarningLostSteering";
    public static string MissileIcon = "MissileIcon";
    public static string Loading = "Loading";
    #endregion

    #region  inspector-main scene
    public static string ApplicationManager = "ApplicationManager";
    public static string CameraManager = "CameraManager";
    public static string MainCamera = "MainCamera";
    public static string FreeCamera = "FreeCamera";
    public static string RadarCamera = "RadarCamera";
    public static string EnvironmentCreator = "EnvironmentCreator";
    public static string Player = "Player";
    public static string NetworkManager = "NetworkManager";
    public static string Missiles = "Missiles";
    public static string TrailMarker = "TrailMarker";

    #endregion

}
