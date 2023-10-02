using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    
    [SerializeField] public AudioSource _musicSource;
    [SerializeField] public AudioSource _effectSource;
    [SerializeField] public AudioClip ost;

    [SerializeField] public AudioClip pick;
    [SerializeField] public AudioClip place;
    [SerializeField] public AudioClip wrong;
    public static SoundManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        
    }

    private void Start()
    {
        _musicSource.loop = true;
        _musicSource.clip = ost;
        _musicSource.Play();
    }

    public void PlaySound(AudioClip clip)
    {
        _effectSource.PlayOneShot(clip);
    }

    public void PlayTrack(AudioClip clip)
    {
        _effectSource.PlayOneShot(clip);
    }

    public void Pick()
    {
        _effectSource.PlayOneShot(pick);
    }

    public void Place()
    {
        _effectSource.PlayOneShot(wrong);
    }

    public void Wrong()
    {
        _effectSource.PlayOneShot(place);
    }

    public void ToggleAudio() { 
        _effectSource.mute = !_effectSource.mute;
    }

    public void ToggleMusic()
    {
        _musicSource.mute = !_musicSource.mute;
    }

    public void ChangeMasterVolume(float v) {
        AudioListener.volume = v;   
    }
}
