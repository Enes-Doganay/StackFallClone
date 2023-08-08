using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;
    AudioSource audioSource;
    public bool sound;
    private void Awake()
    {
        MakeSingleton();
    }
    private void MakeSingleton()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
    }
    public void SoundOnOff()
    {
        sound = !sound;
    }
    public void PlaySoundFX(AudioClip audioClip,float volume)
    {
        if (sound)
        {
            audioSource.PlayOneShot(audioClip, volume);
        }
    }
}
