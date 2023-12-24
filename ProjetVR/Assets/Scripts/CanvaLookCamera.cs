using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvaLookCamera : MonoBehaviour
{
    [SerializeField] Camera mCameraToLook = null;

    void Update()
    {
        if (!mCameraToLook) return;
        transform.LookAt(mCameraToLook.transform.position);
    }
}
