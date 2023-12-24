using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.InputSystem;

public enum FINGERS
{
    NONE,
    THUMB,
    INDEX,
    MIDDLE,
    RING,
    PINKY
};

[Serializable]
public class HandlesFingers
{
    [SerializeField] FINGERS mFinger = FINGERS.NONE;
    [SerializeField] Transform mHandle = null;

    public FINGERS Finger() => mFinger;
    public Transform Handle() => mHandle;
}

public class Weapon : MonoBehaviour
{
    public Action OnWeaponShot = null;
    public Action OnTopMaxSlide = null;
    public Action OnTopEndSlide = null;

    [SerializeField] bool mCanShoot = true;

    [SerializeField] uint mAmmo = 0;
    [SerializeField] uint mMaxAmmo = 9;
    [SerializeField] bool mBeingReload = false;
    [SerializeField] GameObject mMagazine = null;
    [SerializeField] GameObject mRealMagazine = null;
    [SerializeField] float mMagazineForceExpulsion = 1;

    [SerializeField] Animator mAnimator = null;
    [SerializeField] Rigidbody mRigidbody = null;
    [SerializeField] bool mIsFiring = false;

    [SerializeField] HAND mHandHolding = HAND.NONE;

    [SerializeField] Transform mConstraintedTransform = null;
    [SerializeField] Transform mBarrel = null;
    [SerializeField] GameObject mBulletCase = null;
    [SerializeField] Transform mBulletCaseLocation = null;
    [SerializeField] float mBulletCaseForceExpulsion = 2;

    [SerializeField] List<HandlesFingers> mHandlesLeftHand = new List<HandlesFingers>();
    [SerializeField] List<HandlesFingers> mHandlesRightHand = new List<HandlesFingers>();

    [SerializeField] Canvas mUIWeapon = null;
    [SerializeField] TextMeshProUGUI mCurrentAmmoUI = null;
    [SerializeField] TextMeshProUGUI mMaxAmmoUI = null;

    public bool CanShoot
    {
        get => mCanShoot;
        set => mCanShoot = value;
    }

    public Transform Constraint
    {
        get => mConstraintedTransform;
        set => mConstraintedTransform = value;
    }

    public void SetHandHolding(HAND _hand) { mHandHolding = _hand; }
    public void ShowUIWeapon(bool _show) { mUIWeapon.gameObject.SetActive(_show); }

    private void Awake()
    {
        OnWeaponShot += () =>
        {
            mCanShoot = true;
            --mAmmo;
            RefreshUIAmmo();

            if (!mBarrel) return;
            SoundManager.Instance.PlaySound(SOUND_NAME.SHOT_1911, mBarrel.position);
            VFXManager.Instance.InstantiateVFX(VFX_NAME.MUZZLE_1911, mBarrel.position, mBarrel.rotation, mBarrel);

            if (!mAnimator) return;
            mAnimator.SetTrigger("shot");
        };

        OnTopMaxSlide += () =>
        {
            DropBulletCasing();
        };

        OnTopEndSlide += () =>
        {
            mCanShoot = true;
        };
    }

    private void Start()
    {
        RefreshUIAmmo();
    }

    private void Update()
    {
        if (!mConstraintedTransform) return;
        transform.position = mConstraintedTransform.position;
        transform.rotation = mConstraintedTransform.rotation;
    }

    private void OnDestroy()
    {
        OnWeaponShot = null;
        OnTopMaxSlide = null;
        OnTopEndSlide = null;
    }

    public void TriggerPressed()
    {
        if (!mBarrel) return;
        if (mAmmo > 0 && !mBeingReload && mCanShoot) FireShot();
        else SoundManager.Instance.PlaySound(SOUND_NAME.OUT_OF_AMMO, mBarrel.position);
    }

    public void FireShot()
    {
        OnWeaponShot?.Invoke();
        RaycastHit _hit;
        bool _hasHit = Physics.Raycast(mBarrel.position, mBarrel.forward, out _hit, Mathf.Infinity);
        if (!_hasHit) return;

        GameObject _vfx = VFXManager.Instance.InstantiateRandomVFXFromPool(VFX_NAME.HOLE, _hit.point, Quaternion.identity, null);
        _vfx.transform.LookAt(_hit.point + _hit.normal);
        _vfx.transform.SetParent(_hit.transform);

        Target _target = _hit.transform.GetComponent<Target>();
        if (!_target) return;
        _target.Shot((_hit.point - mBarrel.position).normalized * 100, _hit.point);
    }

    public void Reload()
    {
        if (!mCanShoot || mBeingReload) return;
        mBeingReload = true;
        mCanShoot = false;
        DropMagazine();
        SoundManager.Instance.PlaySound(SOUND_NAME.RELOAD_1911, mBarrel.position);
        StartCoroutine(FinishReload());
    }

    IEnumerator FinishReload()
    {
        yield return new WaitForSeconds(2);
        mRealMagazine.SetActive(true);
        mAmmo = mMaxAmmo;
        mCanShoot = true;
        mBeingReload = false;
        RefreshUIAmmo();
    }

    void DropMagazine()
    {
        if (!mMagazine || !mRealMagazine) return;
        mRealMagazine.SetActive(false);
        GameObject _instance = Instantiate(mMagazine, mRealMagazine.transform.position, transform.rotation);
        if (!_instance) return;
        Rigidbody _physicInstance = _instance.GetComponent<Rigidbody>();
        if (!_physicInstance) return;
        _physicInstance.AddForce(-mRealMagazine.transform.forward * mBulletCaseForceExpulsion);
    }

    public void DropBulletCasing()
    {
        if (!mBulletCase || !mBulletCaseLocation) return;
        float _angleX = UnityEngine.Random.Range(-60,-80);
        int _randSign = UnityEngine.Random.value < 0.5f ? -1 : 1;
        float _angleZ = UnityEngine.Random.Range(15, 20);
        Vector3 _direction = Quaternion.AngleAxis(_angleX, transform.right) * transform.up;
        Vector3 _newZ = Vector3.Cross(transform.right, _direction.normalized);
        _direction = Quaternion.AngleAxis(_randSign * _angleZ, _newZ) * _direction;

        GameObject _instance = Instantiate(mBulletCase, mBulletCaseLocation.position, transform.rotation);
        if (!_instance) return;
        Rigidbody _physicInstance = _instance.GetComponent<Rigidbody>();
        if (!_physicInstance) return;
        _physicInstance.AddForce(_direction.normalized * mBulletCaseForceExpulsion);
    }

    /// <summary>
    /// Called in Fire animation
    /// </summary>
    public void TopMaxSlide() => OnTopMaxSlide?.Invoke();
    /// <summary>
    /// Called in Fire animation
    /// </summary>
    public void TopEndSlide() => OnTopEndSlide?.Invoke();

    public List<HandlesFingers> GetHandles(HAND _hand) { return _hand == HAND.LEFT ? mHandlesLeftHand : mHandlesRightHand; }
    public Transform GetHandle(HAND _hand, FINGERS _finger)
    {
        List<HandlesFingers> _handles = _hand == HAND.LEFT ? mHandlesLeftHand : mHandlesRightHand;
        foreach (HandlesFingers _handle in _handles)
            if (_handle.Finger() == _finger) return _handle.Handle();
        return null;
    }

    public void StopPhysics(bool _stop)
    {
        if (!mRigidbody) return;
        mRigidbody.isKinematic = _stop;
    }

    public void RefreshUIAmmo()
    {
        mCurrentAmmoUI.SetText(mAmmo.ToString());
        mMaxAmmoUI.SetText("/" + mMaxAmmo.ToString());
    }
}
