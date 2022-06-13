using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoliderShooting : MonoBehaviour
{
    //shooting options
    public float shootDistance;
    public float viewDistance;
    public Vector3 bulletSpreadVariance;
    public float fireRate;
    public float shootCost;
    public float currentAmmo;
    public short damage;

    //references
    public Transform shootPoint;
    public ParticleSystem mf;
    public TrailRenderer bulletTrail;


    private RaycastHit hit;
    [SerializeField] private bool isShooting;

    public void Shoot()
    {
        StartCoroutine(IEShoot());
    }

    IEnumerator IEShoot()
    {
        if(!isShooting)
        {
            mf.Play();
            Vector3 direction = GetDirection();

            if(Physics.Raycast(shootPoint.position, direction, out hit, shootDistance))
            {      
                if(hit.transform.gameObject.GetComponent<PlayerStats>() || hit.transform.gameObject.GetComponentInParent<PlayerStats>())
                {
                    PlayerStats playerStats = hit.transform.GetComponent<PlayerStats>();
                    playerStats.TakeDamage(damage);
                }

                TrailRenderer trail = Instantiate(bulletTrail, shootPoint.position, Quaternion.identity);
                StartCoroutine(SpawnTrails(trail, hit));
            }

            currentAmmo -= shootCost;

            isShooting = true;

            yield return new WaitForSecondsRealtime(fireRate);

            isShooting = false;
        }
    }

    private IEnumerator SpawnTrails(TrailRenderer trailRenderer, RaycastHit hit)
    {
        float time = 0f;
        Vector3 startPosition = trailRenderer.transform.position;

        while(time < 1)
        {
            if(hit.distance > 1f)
            {
                trailRenderer.transform.position = Vector3.Lerp(startPosition, hit.point, time);
                time += Time.deltaTime / trailRenderer.time;
            }
            else
            {
                trailRenderer.transform.position = Vector3.Lerp(startPosition, startPosition * shootDistance, time);
                time += Time.deltaTime / trailRenderer.time;
            }

            yield return null;
        }

        trailRenderer.transform.position = hit.point;

        Destroy(trailRenderer.gameObject, trailRenderer.time);
    }

    private Vector3 GetDirection()
    {
        Vector3 dir = shootPoint.forward;

        dir += new Vector3
        (
            UnityEngine.Random.Range(-bulletSpreadVariance.x, bulletSpreadVariance.x),
            UnityEngine.Random.Range(-bulletSpreadVariance.y, bulletSpreadVariance.y),
            UnityEngine.Random.Range(-bulletSpreadVariance.z, bulletSpreadVariance.z)
        );

        dir.Normalize();

        return dir;
    }
}
