using System;
using UnityEngine;

public class FaceCamera : MonoBehaviour
{
    public Camera mainCamera;

    private void Start()
    {
        mainCamera = Camera.main;
    }

    void LateUpdate()
    {
        if (mainCamera == null) return;
        
        transform.LookAt(transform.position + mainCamera.transform.rotation * Vector3.forward,
            mainCamera.transform.rotation * Vector3.up);
    }
}
