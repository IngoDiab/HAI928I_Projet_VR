using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equipement : MonoBehaviour
{
    [SerializeField] Transform mCameraPlayerTransform = null;
    [SerializeField] Vector3 mOffsetPosition = Vector3.zero;

    // Update is called once per frame
    void Update()
    {
        transform.position = mCameraPlayerTransform.position + mOffsetPosition.x* mCameraPlayerTransform.right + mOffsetPosition.y * Vector3.up + mOffsetPosition.z * mCameraPlayerTransform.forward;
    }
}
