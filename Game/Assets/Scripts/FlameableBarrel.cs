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
            TakeDamage(10);
        }
    }

    void Update()
    {
        if(health <= 80)
        {
            Burn();
        }

        if(health <= 0)
        {
            Explode();
        }
    }
}
