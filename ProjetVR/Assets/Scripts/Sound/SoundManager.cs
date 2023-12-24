using System;
using System.Collections.Generic;
using UnityEngine;


public enum SOUND_NAME
{
    DEFAULT,
    OUT_OF_AMMO,
    SHOT_1911,
    RELOAD_1911
};

[Serializable]
public class PairNameSound
{
    [SerializeField] SOUND_NAME name = SOUND_NAME.DEFAULT;
    [SerializeField] AudioClip sound = null;

    public SOUND_NAME Name() => name;
    public AudioClip Sound() => sound;
}

public class SoundManager : Singleton<SoundManager>
{
    [SerializeField]List<PairNameSound> allSounds = new List<PairNameSound>();

    public List<PairNameSound> GetAllSounds() => allSounds;

    AudioClip GetSoundByName(SOUND_NAME _name)
    {
        foreach (PairNameSound _soundData in allSounds)
            if (_soundData.Name() == _name) return _soundData.Sound();
        return null;
    }

    public void PlaySound(SOUND_NAME _soundName, Vector3 _soundPos)
    {
        AudioClip _sound = GetSoundByName(_soundName);
        if (!_sound) return;
        AudioSource.PlayClipAtPoint(_sound, _soundPos);
    }
}
