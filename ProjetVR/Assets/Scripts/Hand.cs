using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Animations.Rigging;
using System.Collections.Generic;
using System;
using UnityEngine.InputSystem.EnhancedTouch;
using static UnityEngine.InputSystem.LowLevel.InputStateHistory;

public enum HAND
{
    NONE,
    RIGHT,
    LEFT
};

[Serializable]
public class Constraint
{
    [SerializeField] FINGERS mFinger = FINGERS.NONE;
    [SerializeField] TwoBoneIKConstraint mConstraint = null;

    public FINGERS Finger() => mFinger;
    public TwoBoneIKConstraint ConstraintIK() => mConstraint;
}

public class Hand : MonoBehaviour
{
    public Action<Weapon> OnWeaponTaken = null;
    public Action<Weapon> OnWeaponLost = null;

    [SerializeField] HAND mHand = HAND.NONE;
    [SerializeField] bool mHasWeapon = false;

    [SerializeField] List<Collider> mColliders = new List<Collider>();

    [SerializeField] XRRayInteractor mXRRayInteractor = null;
    [SerializeField] XRDirectInteractor mXRDirectInteractor = null;
    [SerializeField] InputActionReference pushButtonReference = null;
    [SerializeField] InputActionReference primaryButtonReference = null;

    [SerializeField] Weapon mWeapon = null;
    [SerializeField] GPE mGPEUsing = null;

    [SerializeField] RigBuilder mRigBuilder = null;
    [SerializeField] List<Constraint> mConstraints = new List<Constraint>();

    private void Awake()
    {
        InitializeRayInteractor();
        InitializeDirectInteractor();

        pushButtonReference.action.performed += UseWeapon;
        primaryButtonReference.action.performed += ReloadWeapon;

        OnWeaponTaken += (Weapon _weapon) =>
        {
            if (!mWeapon || !mRigBuilder) return;
            foreach (Constraint _constraint in mConstraints)
            {
                FINGERS _finger = _constraint.Finger();
                Transform _handle = mWeapon.GetHandle(mHand, _finger);
                if (!_handle) continue;
                TwoBoneIKConstraint _constraintIK = _constraint.ConstraintIK();
                if (!_constraintIK) continue;
                _constraintIK.data.target = _handle;
            }
            mRigBuilder.Build();
            mHasWeapon = true;
        };

        OnWeaponLost += (Weapon _weapon) =>
        {
            if (!mWeapon || !mRigBuilder) return;
            foreach (Constraint _constraint in mConstraints)
            {
                TwoBoneIKConstraint _constraintIK = _constraint.ConstraintIK();
                if (!_constraintIK) continue;
                _constraintIK.data.target = null;
            }
            mRigBuilder.Build();
            mHasWeapon = false;
        };
    }
    private void OnDestroy()
    {
        OnWeaponTaken = null;
        OnWeaponLost = null;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (mColliders.Contains(other)) return;
        mColliders.Add(other);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!mColliders.Contains(other)) return;
        mColliders.Remove(other);
    }

    void InitializeRayInteractor()
    {
        if (!mXRRayInteractor) return;
        mXRRayInteractor.selectEntered.AddListener((nothing) =>
        {
            XRBaseInteractable _interactable = mXRRayInteractor.interactablesSelected[0] as XRBaseInteractable;
            if (!_interactable) return;
            if (_interactable.tag == "Weapon") TakeWeapon(_interactable.GetComponent<Weapon>());
            else if (_interactable.tag == "GPE") UseGPE(_interactable.GetComponent<GPE>());
        });

        mXRRayInteractor.selectExited.AddListener((nothing) =>
        {
            if (mWeapon) ReleaseWeapon();
            else if (mGPEUsing) StopUsingGPE();
        });
    }

    void InitializeDirectInteractor()
    {
        if (!mXRDirectInteractor) return;
        mXRDirectInteractor.selectEntered.AddListener((nothing) =>
        {
            XRBaseInteractable _interactable = mXRDirectInteractor.interactablesSelected[0] as XRBaseInteractable;
            if (!_interactable) return;
            if (_interactable.tag == "Weapon") TakeWeapon(_interactable.GetComponent<Weapon>());
            else if (_interactable.tag == "GPE") UseGPE(_interactable.GetComponentInParent<GPE>());
        });

        mXRDirectInteractor.selectExited.AddListener((nothing) =>
        {
            if (mWeapon) ReleaseWeapon();
            else if (mGPEUsing) StopUsingGPE();
        });
    }

    void TakeWeapon(Weapon _weapon)
    {
        mWeapon = _weapon;
        if (!mWeapon) return;
        mWeapon.Constraint = null;
        mWeapon.StopPhysics(true);
        OnWeaponTaken?.Invoke(mWeapon);
    }

    void ReleaseWeapon()
    {
        if (!mWeapon) return;
        bool _holsterFound = false;
        foreach(Collider _collider in mColliders)
        {
            if (_collider.tag != "Holster") continue;
            _holsterFound = true;
            mWeapon.StopPhysics(true);
            mWeapon.Constraint = _collider.transform;
        }
        if(!_holsterFound) mWeapon.StopPhysics(false);
        OnWeaponLost?.Invoke(mWeapon);
        mWeapon = null;
    }

    void UseWeapon(InputAction.CallbackContext obj)
    {
        if (!mWeapon || !mWeapon.CanShoot) return;
        mWeapon.TriggerPressed();
    }

    void ReloadWeapon(InputAction.CallbackContext obj)
    {
        if (!mWeapon || !mWeapon.CanShoot) return;
        mWeapon.Reload();
    }

    void UseGPE(GPE _gpe)
    {
        if (!_gpe) return;
        mGPEUsing = _gpe;
        _gpe.UseGPE(Player.Instance);
    }

    void StopUsingGPE()
    {
        if (!mGPEUsing) return;
        mGPEUsing.ExitGPE();
        mGPEUsing = null;
    }
}
