using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : Singleton<Player>
{
    [SerializeField] float mHealth = 100;

    [SerializeField] TextMeshProUGUI mTargetDestroyedUI = null;
    [SerializeField] TextMeshProUGUI mTargetTotalUI = null;
    [SerializeField] GameObject mMessage = null;

    [SerializeField] Hand mRightHand = null;
    [SerializeField] Hand mLeftHand = null;
    [SerializeField] CharacterController mController = null;
    [SerializeField] Transform mHeadPlayer = null;

    public void AddHealth(float _value) { mHealth += _value; }

    public CharacterController GetCharacterController() { return mController; }
    public float GetHalfSize() { return mController.height; }
    public Transform GetHead() { return mHeadPlayer; }
    public Vector3 GetHeadOffset() { return mHeadPlayer.position - transform.position; }
    public Vector3 GetLeftHandOffset() { return mLeftHand.transform.position - transform.position; }
    public Vector3 GetRightHandOffset() { return mRightHand.transform.position - transform.position; }

    private void Start()
    {
        RefreshUIScore();
    }

    public void RefreshUIScore()
    {
        TargetManager _tManager = TargetManager.Instance;
        mTargetDestroyedUI.text = _tManager.GetNbTargetsDestroyed().ToString();
        mTargetTotalUI.text = "/" + _tManager.GetNbTargets().ToString();

        if(_tManager.GetNbTargetsDestroyed() == _tManager.GetNbTargets()) mMessage.SetActive(true);
    }
}
