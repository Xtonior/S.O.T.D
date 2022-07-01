using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Breakable : MonoBehaviour
{
    public GameObject fracturedVersion;
    public float Health;
    public float breakForce;
    public AudioSource audioSource;
    public AudioClip damageSound;
    public AudioClip breakSound;

    public void GetDamage(float damage)
    {
        Health -= damage;
        audioSource.PlayOneShot(damageSound);
    }

    void Break()
    {
        audioSource.PlayOneShot(breakSound);
    }
}
