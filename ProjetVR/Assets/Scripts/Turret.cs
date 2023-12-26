using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Turret : MonoBehaviour
{
    [SerializeField] bool mIsActive = true;
    [SerializeField] bool mTurnOn = true;
    [SerializeField] float mDistanceDetection = 10;

    [SerializeField] Transform mTurretHead = null;
    [SerializeField] Transform mBarrel = null;

    [SerializeField] bool mIsReloading = false;

    [SerializeField] float mAmmo = 0;
    [SerializeField] float mMaxAmmo = 0;
    [SerializeField] float mReloadTime = 0;

    [SerializeField] float mRotationSpeed = 10;
    [SerializeField] float mSpeedFire = 0;

    public void SetIsActive(bool _active) { mIsActive = _active; }
    public void ToggleActive() { mIsActive = !mIsActive; }

    private void Start()
    {
        mAmmo = mMaxAmmo;
        StartCoroutine(Shoot());
    }

    private void Update()
    {
        if (!mIsActive && (Player.Instance.transform.position - transform.position).magnitude < mDistanceDetection) mTurnOn = true;
        CheckStateTuret();
    }

    public void CheckStateTuret()
    {
        if (mIsActive && mTurnOn) FollowPlayer();
        else if (!mIsActive && mTurnOn)
        {
            mIsActive = true;
            FollowPlayer();
            StartCoroutine(Shoot());
        }
        else if (mIsActive && !mTurnOn) mIsActive = false;
    }

    IEnumerator Reload()
    {
        if (mIsReloading || !mIsActive) yield break;
        mIsReloading = true;
        yield return new WaitForSeconds(mReloadTime);
        mAmmo = mMaxAmmo;
        mIsReloading = false;
        StartCoroutine(Shoot());
    }

    IEnumerator Shoot()
    {
        if (mIsReloading || !mBarrel || !mIsActive) yield break;

        --mAmmo;
        SoundManager.Instance.PlaySound(SOUND_NAME.SHOT_1911, mBarrel.position);
        GameObject _vfxShot = VFXManager.Instance.InstantiateVFX(VFX_NAME.MUZZLE_1911, mBarrel.position, mBarrel.rotation, mBarrel);
        _vfxShot.transform.localScale = new Vector3(1000, 1000, 1000);

        RaycastHit _hit;
        bool _hasHit = Physics.Raycast(mBarrel.position, mBarrel.forward, out _hit, Mathf.Infinity);
        if (_hasHit)
        {
            GameObject _vfx = VFXManager.Instance.InstantiateRandomVFXFromPool(VFX_NAME.HOLE, _hit.point, Quaternion.identity, null);
            _vfx.transform.LookAt(_hit.point + _hit.normal);
            _vfx.transform.SetParent(_hit.transform);

            if (_hit.transform.tag == "MainCamera")
            {
                Player _player = _hit.transform.GetComponentInParent<Player>();
                if (_player)
                {
                    _player.AddHealth(-1);
                }
            }
        }
        yield return new WaitForSeconds(mSpeedFire);

        if (mAmmo <= 0) StartCoroutine(Reload());
        else StartCoroutine(Shoot());
    }

    void FollowPlayer()
    {
        if (!mIsActive) return;
        Transform _target = Player.Instance.GetHead();
        Vector3 _direction = _target.position - mTurretHead.position;
        Quaternion _quatTarget = Quaternion.LookRotation(_direction);
        mTurretHead.rotation = Quaternion.Lerp(mTurretHead.rotation, _quatTarget, Time.deltaTime * mRotationSpeed);
    }

    private void OnDrawGizmos()
    {
        /*Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position, mDistanceDetection);*/
    }
}
