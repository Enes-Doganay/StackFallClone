using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : Singleton<SoundManager>
{
    AudioSource audioSource;
    public bool sound;
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
