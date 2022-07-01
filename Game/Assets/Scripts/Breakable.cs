using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Breakable : MonoBehaviour
{
    public GameObject fracturedVersion;
    public GameObject loot;
    public float Health;
    public float breakForce;
    public AudioSource audioSource;
    public AudioClip damageSound;
    public AudioClip breakSound;

    public GameObject fractured { get; protected set; }

    public void GetDamage(float damage)
    {
        Health -= damage;
        audioSource.PlayOneShot(damageSound);
    }

    public virtual void Break()
    {
        fractured = Instantiate(fracturedVersion, transform.position, transform.rotation);
        fractured.GetComponent<SimpleAudioPlayer>().PlayOneShot(breakSound);
        Instantiate(loot, transform.position, transform.rotation);
    }
}
