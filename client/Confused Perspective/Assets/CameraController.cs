using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class CameraController : MonoBehaviour
{
    public float sens = 5.0f;
    public Transform player;
    public Transform cam;
    private float xaxisclamp = 0;
    void Update()
    {
        Cursor.lockState = CursorLockMode.Locked;
        RotateCam();
    }

    void RotateCam()
    {
        float mousex = Input.GetAxis("Mouse X");
        float mousey = Input.GetAxis("Mouse Y");

        float rotX = mousex * sens;
        float rotY = mousey * sens;
        xaxisclamp -= rotY;
        Vector3 rotPlayer = player.transform.rotation.eulerAngles;
        Vector3 rotCam = cam.transform.rotation.eulerAngles;
        rotCam.x -= rotY;
        rotCam.z = 0;
        rotPlayer.y += rotX;

        if (xaxisclamp > 90)
        {
            rotCam.x = 90;
            xaxisclamp = 90;
        }
        else if (xaxisclamp < -90){
            rotCam.x = 270;
            xaxisclamp = -90;
        }
        cam.rotation = Quaternion.Euler(rotCam);
        player.rotation = Quaternion.Euler(rotPlayer);
    }
}
