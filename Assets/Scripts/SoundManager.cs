using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
class Audio
{
    public AudioClip clip;
    public string name;
}

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance = null;

    [SerializeField]
    Audio[] bgms, effects;

    Dictionary<string, AudioClip> _bgms;
    Dictionary<string, AudioClip> _effects;

    AudioSource bgm = null, effect = null;

    bool on_bgm = true, on_effect = true;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        bgm = gameObject.AddComponent<AudioSource>();
        effect = gameObject.AddComponent<AudioSource>();

        _bgms = new Dictionary<string, AudioClip>();
        foreach (var bgm_ in bgms)
            _bgms.Add(bgm_.name, bgm_.clip);
        _effects = new Dictionary<string, AudioClip>();
        foreach (var effect_ in effects)
            _effects.Add(effect_.name, effect_.clip);
    }

    public void PlayBgm(string name, float playspeed = 1.0f)
    {
        if (_bgms.ContainsKey(name) && on_bgm)
        {
            if (bgm.isPlaying)
                bgm.Stop();
            bgm.loop = true;
            bgm.pitch = playspeed;
            bgm.clip = _bgms[name];
            bgm.Play();
        }
        else
            Debug.Log("SoundManager/PlayBgm() Error.");
    }
    public void StopBgm()
    {
        if (bgm.isPlaying)
            bgm.Stop();
    }
    public void OnBgm()
    {
        if (bgm == null)
        {
            Debug.Log("SoundManager/OnBgm() Error.");
            return;
        }
        if (!bgm.isPlaying)
        {
            Debug.Log("브금부터 켜라.");
            return;
        }
        if (bgm.mute)
        {
            bgm.mute = false;
            on_bgm = true;
        }
    }
    public void OffBgm()
    {
        if (bgm == null)
        {
            Debug.Log("SoundManager/OffBgm() Error.");
            return;
        }
        if (!bgm.isPlaying)
        {
            Debug.Log("브금부터 켜라.");
            return;
        }
        if (!bgm.mute)
        {
            bgm.mute = true;
            on_bgm = false;
        }
    }

    public void PlayEffect(string name, float playspeed = 1.0f)
    {
        if (_effects.ContainsKey(name) && on_effect)
        {
            effect.loop = false;
            effect.pitch = playspeed;
            effect.PlayOneShot(_effects[name]);
        }
        else
            Debug.Log("SoundManager/PlayEffect() Error.");
    }
    public void OnEffect()
    {
        on_effect = true;
    }
    public void OffEffect()
    {
        on_effect = false;
    }

    public void Destroy()
    {
        if (bgm != null)
        {
            if (bgm.isPlaying)
                bgm.Stop();
            bgm = null;
        }
        if (effect != null)
        {
            if (effect.isPlaying)
                effect.Stop();
            effect = null;
        }
    }
}