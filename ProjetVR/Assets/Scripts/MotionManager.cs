using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class MotionManager : Singleton<MotionManager>
{
    [SerializeField] ActionBasedContinuousMoveProvider mContinousMoveProvider = null;
    [SerializeField] InputActionReference mContinousMoveAction = null;
    [SerializeField] InputActionReference mSnapTurnAction = null;
    [SerializeField] InputActionReference mContinousTurnAction = null;

    [SerializeField] bool mPlayerUsingContinousTurn = true;

    private void Start()
    {
        EnableFreeMove(true);
        EnableFreeRotation(true);
    }

    public void EnableFreeMove(bool _enable) 
    {
        if (_enable) mContinousMoveAction.action.Enable();
        else mContinousMoveAction.action.Disable();

        mContinousMoveProvider.useGravity = _enable;
    }
    public void EnableFreeRotation(bool _enable) 
    {
        if (!mPlayerUsingContinousTurn && _enable) mSnapTurnAction.action.Enable();
        else mSnapTurnAction.action.Disable();

        if (mPlayerUsingContinousTurn && _enable) mContinousTurnAction.action.Enable();
        else mContinousTurnAction.action.Disable();
    }
}
