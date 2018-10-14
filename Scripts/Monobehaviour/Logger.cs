using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Logger : MonoBehaviour
{
    public void Log(string log)
    {
        Debug.Log(log);
    }

    public void Break()
    {
        Debug.Break();
    }
}
