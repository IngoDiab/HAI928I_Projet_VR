using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Rope : GPE
{
    [SerializeField] Player mPlayerAttached = null;
    [SerializeField] Transform mUpExit = null;
    [SerializeField] Transform mUpRope = null;
    [SerializeField] Transform mDownRope = null;
    [SerializeField] float mMoveSpeed = 10;

    [SerializeField] InputActionReference leftStickReference = null;

    [SerializeField] float _interpolation = 0;

    public void CalculateInterpolationOnPlayerPosition()
    {
        Vector3 _characterPosition = mPlayerAttached.transform.position;
        Vector3 _upRope = new Vector3(0, mUpRope.position.y, 0);
        Vector3 _downRope = new Vector3(0, mDownRope.position.y, 0);
        Vector3 _downToUp = (_upRope - _downRope).normalized;
        Vector3 _projection = Vector3.Project(_characterPosition, _downToUp);
        _interpolation = Mathf.Min(Mathf.Max(_projection.sqrMagnitude/ (_upRope - _downRope).sqrMagnitude, 0), 1);
    }

    public override void UseGPE(Player _playerAttached)
    {
        mPlayerAttached = _playerAttached;
        if (!mPlayerAttached) return;

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
        float _upValue = leftStickReference.action.ReadValue<Vector2>().y;
        _interpolation += _upValue * mMoveSpeed * Time.deltaTime;

        Vector3 _upRope = new Vector3(0, mUpRope.position.y, 0);
        Vector3 _downRope = new Vector3(0, mDownRope.position.y, 0);
        float _characterHeight = Vector3.Lerp(_downRope, _upRope, _interpolation).y;
        mPlayerAttached.transform.position = new Vector3(mPlayerAttached.transform.position.x, _characterHeight, mPlayerAttached.transform.position.z);
    }

    public override void ExitGPE()
    {
        mPlayerAttached = null;
        MotionManager.Instance.EnableFreeMove(true);
        MotionManager.Instance.EnableFreeRotation(true);
    }
}
