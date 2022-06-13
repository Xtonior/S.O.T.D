using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSoundManager : MonoBehaviour
{
    [SerializeField] AudioClip singleShootClip;
    [SerializeField] Weapon weapon;
    AudioSource source;

    void OnEnable()
    {
        weapon.OnAttack += ShootSingle;
    }

    void OnDisable()
    {
        weapon.OnAttack -= ShootSingle;
    }

    void Start()
    {
        source = GetComponent<AudioSource>();
    }

    void ShootSingle()
    {
        source.clip = singleShootClip;
        source.Play();
    }
}
