using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameableBarrel : MonoBehaviour
{
    [SerializeField] Explosion explosion;
    [SerializeField] private float hitEvery;
    [SerializeField] private float lifeTime;
    [SerializeField] private ParticleSystem fire;
    [field: SerializeField] private float health;
    float timer;

    void Start()
    {
        fire.Stop();
        timer = lifeTime;
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
    }

    void Explode()
    {

    }

    void Burn()
    {
        
    }

    void Update()
    {
        if(health <= 80)
        {
            timer -= hitEvery * Time.deltaTime;
            fire.Play();
        }

        if(timer <= 0)
        {
            timer = lifeTime;
            TakeDamage(10);
        }

        if(health <= 0)
        {
            explosion.Explode();
            Destroy(gameObject, 0.2f);
        }
    }
}
