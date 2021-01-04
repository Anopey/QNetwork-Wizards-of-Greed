using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalBehaviour : MonoBehaviour
{
    public PortalBehaviour partner;
    public Camera myCamera;


    void Update()
    {
        RotateCamera();
    }

    //Misc methods

    private void RotateCamera()
    {
        Transform playerCam = Camera.main.transform;
        Transform camTrans = myCamera.transform;
        Transform partnerTrans = partner.transform;

        Vector3 cameraEuler = Vector3.zero;


        //Find the position of the camera
        Vector3 pos = partnerTrans.InverseTransformPoint(playerCam.position);
        camTrans.localPosition = new Vector3(-pos.x, pos.y, -pos.z);


        //Find the x-rotation
        Transform prevParent = playerCam.parent;
        playerCam.SetParent(transform);

        cameraEuler.x = playerCam.localEulerAngles.x;

        playerCam.SetParent(prevParent);


        /*Find the y-rotation*/

        //Temporarily rotate the player cam so it's flat
        Vector3 oldPlayerRot = playerCam.localEulerAngles;
        playerCam.localRotation = Quaternion.Euler(0, oldPlayerRot.y, oldPlayerRot.z);

        //Use DiegoSLTS's method for finding the y-rot.
        cameraEuler.y = SignedAngle(-partnerTrans.forward, playerCam.forward, Vector3.up);

        //Restore the player cam
        playerCam.localRotation = Quaternion.Euler(oldPlayerRot);

        //Apply the rotation
        camTrans.localRotation = Quaternion.Euler(cameraEuler);

    }

    private float SignedAngle(Vector3 a, Vector3 b, Vector3 n)
    {
        //Code stolen from DiegoSLTS
        //http://answers.unity3d.com/questions/992289/portal-effect-using-render-textures-how-should-i-m.html

        // angle in [0,180]
        float angle = Vector3.Angle(a, b);
        float sign = Mathf.Sign(Vector3.Dot(n, Vector3.Cross(a, b)));

        // angle in [-179,180]
        float signed_angle = angle * sign;

        while (signed_angle < 0) signed_angle += 360;

        return signed_angle;
    }
}
