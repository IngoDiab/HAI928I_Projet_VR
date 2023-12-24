using UnityEngine;
using UnityEngine.InputSystem;

public class Zipline : GPE
{
    [SerializeField] Player mPlayerAttached = null;
    [SerializeField] Hand mHandAttached = null;
    [SerializeField] Transform mUpRope = null;
    [SerializeField] Transform mDownRope = null;
    [SerializeField] float mMoveSpeed = 10;

    float mInterpolation = 0;

    public void CalculateInterpolationOnPlayerPosition()
    {
        Vector3 _handPosition = mHandAttached.transform.position;
        Vector3 _upToDown = (mDownRope.position - mUpRope.position).normalized;
        Vector3 _projection = Vector3.Project(_handPosition - mUpRope.position, _upToDown);
        mInterpolation = Mathf.Clamp01(_projection.magnitude / (mDownRope.position - mUpRope.position).magnitude);
    }

    public override void UseGPE(Player _playerAttached, Hand _handAttached)
    {
        mPlayerAttached = _playerAttached;
        mHandAttached = _handAttached;
        if (!mPlayerAttached || !mHandAttached) return;

        CalculateInterpolationOnPlayerPosition();

        MotionManager.Instance.EnableFreeMove(false);
        MotionManager.Instance.EnableFreeRotation(false);
    }

    private void Update()
    {
        MovePlayerOnRope();
    }

    public void MovePlayerOnRope()
    {
        if (!mPlayerAttached) return;

        RaycastHit _hit;
        bool _hasHit = Physics.Raycast(mPlayerAttached.transform.position, -Vector3.up, out _hit, mPlayerAttached.GetHalfSize());
        if (true)
        {
            mInterpolation += mMoveSpeed * Time.deltaTime;
            mInterpolation = Mathf.Clamp01(mInterpolation);
        }

        Vector3 _handPos = Vector3.Lerp(mUpRope.position, mDownRope.position, mInterpolation);
        Vector3 _bodyPos = _handPos - (mHandAttached.GetHandSide() == HAND.RIGHT ? mPlayerAttached.GetRightHandOffset() : mPlayerAttached.GetLeftHandOffset());
        mPlayerAttached.transform.position = _bodyPos;
    }

    public override void ExitGPE(Player _playerAttached, Hand _handAttached)
    {
        if(_playerAttached == mPlayerAttached) mPlayerAttached = null;
        if (_handAttached == mHandAttached)  mHandAttached = null;

        MotionManager.Instance.EnableFreeMove(true);
        MotionManager.Instance.EnableFreeRotation(true);
    }
}
