using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleAudioPlayer : MonoBehaviour
{
    private AudioSource audioSource;

    public void PlayOneShot(AudioClip clip)
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.PlayOneShot(clip);
    }
}
