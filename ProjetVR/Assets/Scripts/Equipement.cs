using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equipement : MonoBehaviour
{
    [SerializeField] Transform mCameraPlayerTransform = null;
    [SerializeField] Transform mHolsterPos = null;
    [SerializeField] Vector3 mOffsetPosition = Vector3.zero;

    public Transform GetHolsterPos() { return mHolsterPos; }

    // Update is called once per frame
    void Update()
    {
        //transform.position = mCameraPlayerTransform.position + mOffsetPosition.x* mCameraPlayerTransform.right + mOffsetPosition.y * Vector3.up + mOffsetPosition.z * mCameraPlayerTransform.forward;
    }
}
