using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : Singleton<Player>
{
    [SerializeField] TextMeshProUGUI mTargetDestroyedUI = null;
    [SerializeField] TextMeshProUGUI mTargetTotalUI = null;

    [SerializeField] Hand mRightHand = null;
    [SerializeField] Hand mLeftHand = null;
    [SerializeField] CharacterController mController = null;

    private void Start()
    {
        RefreshUIScore();
    }

    public void RefreshUIScore()
    {
        TargetManager _tManager = TargetManager.Instance;
        mTargetDestroyedUI.text = _tManager.GetNbTargetsDestroyed().ToString();
        mTargetTotalUI.text = "/" + _tManager.GetNbTargets().ToString();
    }
}
