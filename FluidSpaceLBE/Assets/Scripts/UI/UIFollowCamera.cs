using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIFollowCamera : MonoBehaviour
{
    public Transform cameraTransform;
    public float distanceFromCamera = 1.5f;

    void Update()
    {
        Vector3 forward = cameraTransform.forward;
        transform.position = cameraTransform.position + forward * distanceFromCamera;
        transform.rotation = Quaternion.LookRotation(forward, cameraTransform.up);
    }
}
