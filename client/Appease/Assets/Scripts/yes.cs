using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.QNetwork.StandardAttributeAddon;
using Game.QNetwork;

public class yes : MonoBehaviour
{

    [QDataReciever(16)]
    public static void randomfuncname(string msg)
    {
        Debug.Log(msg);
    }

}
