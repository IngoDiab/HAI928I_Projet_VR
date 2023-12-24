using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public enum VFX_NAME
{
    DEFAULT,
    MUZZLE_1911,
    HOLE
};

[Serializable]
public class PairNameVFX
{
    [SerializeField] VFX_NAME name = VFX_NAME.DEFAULT;
    [SerializeField] GameObject vfx = null;
    [SerializeField] List<GameObject> vfxs = new List<GameObject>();
    [SerializeField] Vector3 scale = Vector3.one;
    [SerializeField] bool autoDestroy = true;
    [SerializeField] float lifetime = 0.5f;

    public VFX_NAME Name() => name;
    public GameObject VFX() => vfx;
    public List<GameObject> VFXS() => vfxs;
    public Vector3 Scale() => scale;
    public bool IsAutoDestroying() => autoDestroy;
    public float Lifetime() => lifetime;
}

public class VFXManager : Singleton<VFXManager>
{
    [SerializeField] List<PairNameVFX> allVFX = new List<PairNameVFX>();

    public List<PairNameVFX> GetAllVFX() => allVFX;

    PairNameVFX GetSoundByName(VFX_NAME _name)
    {
        foreach (PairNameVFX _vfxData in allVFX)
            if (_vfxData.Name() == _name) return _vfxData;
        return null;
    }

    public GameObject InstantiateVFX(VFX_NAME _name, Vector3 _vfxPos, Quaternion _vfxRotation, Transform _parent)
    {
        PairNameVFX _vfxData = GetSoundByName(_name);
        if (_vfxData == null) return null;

        GameObject _vfx = _vfxData.VFX();
        if (!_vfx) return null;

        GameObject _instance = Instantiate(_vfx, _vfxPos, _vfxRotation, _parent);
        if (!_instance) return null;

        _instance.transform.localScale = _vfxData.Scale();
        if (!_vfxData.IsAutoDestroying()) return _instance;

        StartCoroutine(DestroyVFX(_instance, _vfxData.Lifetime()));

        return _instance;
    }

    public GameObject InstantiateRandomVFXFromPool(VFX_NAME _name, Vector3 _vfxPos, Quaternion _vfxRotation, Transform _parent)
    {
        PairNameVFX _vfxData = GetSoundByName(_name);
        if (_vfxData == null) return null;

        List<GameObject> _vfxs = _vfxData.VFXS();
        if (_vfxs.Count == 0) return null;

        int _randomIndex = UnityEngine.Random.Range(0, _vfxs.Count - 1);
        GameObject _instance = Instantiate(_vfxs[_randomIndex], _vfxPos, _vfxRotation, _parent);
        if (!_instance) return null;

        _instance.transform.localScale = _vfxData.Scale();
        if (!_vfxData.IsAutoDestroying()) return _instance;

        StartCoroutine(DestroyVFX(_instance, _vfxData.Lifetime()));

        return _instance;
    }

    private IEnumerator DestroyVFX(GameObject _instance, float _lifetime)
    {
        yield return new WaitForSeconds(_lifetime);
        Destroy(_instance);
    }
}
