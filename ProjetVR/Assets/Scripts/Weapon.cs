using System;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public Action OnWeaponShot = null;
    public Action OnTopMaxSlide = null;
    public Action OnTopEndSlide = null;

    [SerializeField] Animator mAnimator = null;
    [SerializeField] bool mIsFiring = false;

    [SerializeField] Transform mBarrel = null;

    [SerializeField] GameObject mBulletCase = null;
    [SerializeField] Transform mBulletCaseLocation = null;
    [SerializeField] float mBulletCaseForceExpulsion = 2;

    [SerializeField] GameObject mVFXBulletHole = null;

    public bool IsFiring
    {
        get => mIsFiring;
        set => mIsFiring = value;
    }

    private void Awake()
    {
        OnWeaponShot += () =>
        {
            mIsFiring = true;

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
            mIsFiring = false;
        };
    }

    private void OnDestroy()
    {
        OnWeaponShot = null;
        OnTopMaxSlide = null;
        OnTopEndSlide = null;
    }

    public void TriggerPressed()
    {
        if (!mBarrel || mIsFiring) return;
        OnWeaponShot?.Invoke();
        RaycastHit _hit;
        bool _hasHit = Physics.Raycast(mBarrel.position, mBarrel.forward, out _hit, Mathf.Infinity);
        if (!_hasHit || !mVFXBulletHole) return;
        Instantiate(mVFXBulletHole, _hit.point, Quaternion.identity, _hit.transform);
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
}
