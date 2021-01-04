using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateLeft : MonoBehaviour
{
    public Transform pivot;
    public float rotationsPerMinute = 10.0f;
    public int flag = 0;
    void Start()
    {
        RotateDown();
    }

    void Update()
    {
        if (pivot.transform.rotation.eulerAngles.x < 45 && pivot.transform.rotation.eulerAngles.x > 40 && flag == 1)
        {
            RotateDown();
        }
        else if (pivot.transform.rotation.eulerAngles.x < 135 && pivot.transform.rotation.eulerAngles.x > 130 && flag == -1)
        {
            RotateUp();
        }
        else if (flag == -1)
        {
            RotateDown();
        }
        else if (flag == 1)
        {
            RotateUp();
        }
    }

    void RotateUp(){
        pivot.transform.Rotate(-6 * rotationsPerMinute * Time.deltaTime, 0, 0);
        flag = 1;
    }

    void RotateDown()
    {
        flag = -1;
        pivot.transform.Rotate(6 * rotationsPerMinute * Time.deltaTime, 0, 0);
    }
}
