using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Rope : GPE
{
    [SerializeField] Player mPlayerAttached = null;
    [SerializeField] Hand mHandAttached = null;
    [SerializeField] Transform mUpRope = null;
    [SerializeField] Transform mDownRope = null;
    [SerializeField] float mMoveSpeed = 10;

    [SerializeField] InputActionReference leftMoveOnRopeReference = null;
    [SerializeField] InputActionReference rightMoveOnRopeReference = null;

    float mInterpolation = 0;

    public void CalculateInterpolationOnPlayerPosition()
    {
        Vector3 _handPosition = mHandAttached.transform.position;
        Vector3 _downToUp = (mUpRope.position - mDownRope.position).normalized;
        Vector3 _projection = Vector3.Project(_handPosition - mDownRope.position, _downToUp);
        mInterpolation = Mathf.Clamp01(_projection.magnitude / (mUpRope.position - mDownRope.position).magnitude);
    }

    public override void UseGPE(Player _playerAttached, Hand _handAttached)
    {
        mPlayerAttached = _playerAttached;
        mHandAttached = _handAttached;
        if(!mPlayerAttached || !mHandAttached) return;

        CalculateInterpolationOnPlayerPosition();

        MotionManager.Instance.EnableFreeMove(false);
        MotionManager.Instance.EnableFreeRotation(false);
    }

    protected virtual void Update()
    {
        MovePlayerOnRope();
    }

    public void MovePlayerOnRope()
    {
        if (!mPlayerAttached || !mHandAttached) return;
        /*float _upValue = mHandAttached.GetHandSide() == HAND.RIGHT ? rightMoveOnRopeReference.action.ReadValue<Vector2>().y : leftMoveOnRopeReference.action.ReadValue<Vector2>().y;
        mInterpolation += _upValue * mMoveSpeed * Time.deltaTime;
        mInterpolation = Mathf.Clamp01(mInterpolation);*/

        Vector3 _handPos = Vector3.Lerp(mDownRope.position, mUpRope.position, mInterpolation);
        Vector3 _bodyPos = _handPos - (mHandAttached.GetHandSide() == HAND.RIGHT ? mPlayerAttached.GetRightHandOffset() : mPlayerAttached.GetLeftHandOffset());
        mPlayerAttached.transform.position = _bodyPos;
    }

    public override void ExitGPE(Player _playerAttached, Hand _handAttached)
    {
        if (_playerAttached == mPlayerAttached) mPlayerAttached = null;
        if (_handAttached == mHandAttached) mHandAttached = null;

        MotionManager.Instance.EnableFreeMove(true);
        MotionManager.Instance.EnableFreeRotation(true);
    }
}
