using UnityEngine;
using System.Collections;
using System;
using MatlabConnection;

public class InverseOfTheRandomMatrix : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        object[] res = Connection.Instance.ExecuteCommand();
        double? result = 1;
        result = res[0] as double?;
        Debug.Log(result);
    }
}
