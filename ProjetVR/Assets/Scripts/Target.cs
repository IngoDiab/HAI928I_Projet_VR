using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    [SerializeField] bool mHasBeenShot = false;
    [SerializeField] Rigidbody mRigidbody = null;

    public void Shot(Vector3 _direction, Vector3 _posShot)
    {
        if (mHasBeenShot) return;
        mHasBeenShot = true;
        TargetManager.Instance.AddDestroyed();
        Player.Instance.RefreshUIScore();

        mRigidbody.useGravity = true;
        mRigidbody.AddForceAtPosition(_direction, _posShot);
    }
}
