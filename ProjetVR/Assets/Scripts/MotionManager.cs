using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class MotionManager : Singleton<MotionManager>
{
    [SerializeField] ActionBasedContinuousMoveProvider mContinousMoveProvider = null;
    [SerializeField] ActionBasedSnapTurnProvider mSnapTurnProvider = null;
    [SerializeField] ActionBasedContinuousTurnProvider mContinousTurnProvider = null;

    [SerializeField] bool mPlayerUsingContinousTurn = true;

    public void EnableFreeMove(bool _enable) { mContinousMoveProvider.enabled = _enable; }
    public void EnableFreeRotation(bool _enable) { mSnapTurnProvider.enabled = !mPlayerUsingContinousTurn && _enable; mContinousTurnProvider.enabled = mPlayerUsingContinousTurn &&_enable; }
}
