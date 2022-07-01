using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameableBarrel : Breakable
{
    [SerializeField] Explosion explosion;
    [SerializeField] private float hitEvery;
    [SerializeField] private float lifeTime;
    [SerializeField] private ParticleSystem fire;
    float timer;

    void Start()
    {
        fire.Stop();
        timer = lifeTime;
    }

    void Explode()
    {
        explosion.Explode();
        Destroy(gameObject, 0.2f);
    }

    void Burn()
    {
        timer -= hitEvery * Time.deltaTime;
        fire.Play();
        
        if(timer <= 0)
        {
            timer = lifeTime + Random.Range(-1f, 1f);
            GetDamage(10);
        }
    }

    void Update()
    {
        if(Health <= 80)
        {
            Burn();
        }

        if(Health <= 0)
        {
            Explode();
        }
    }
}
