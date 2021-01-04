using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    public GameObject object1, object2;
    private Vector3 tempPosition;
    private void OnCollisionEnter()
    {
        Debug.Log("collide");
        tempPosition = object1.transform.position;
        object1.transform.position = object2.transform.position;
        object2.transform.position = tempPosition;
    }
}
