using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public abstract class Shooting : MonoBehaviour
{
    [Header("Visuals")]
    [SerializeField] private ParticleSystem mf;
    [SerializeField] private GameObject bulletImpact;
    [SerializeField] private GameObject bulletHoleDecal;
    [SerializeField] private TrailRenderer bulletTrail;

    [Header("Weapon Parameters")]
    [SerializeField] private float damage;
    [SerializeField] private float maxDistance;
    [SerializeField] private float fireRate;
    [SerializeField] private float impactForce;
    [SerializeField] private float reloadingTime;
    [SerializeField] private float shootCost;
    [SerializeField] private float maxAmmo;
    [SerializeField] private float availableAmmo;
    [SerializeField] private float maxClipAmmo;
    [SerializeField] private Vector3 bulletSpreadVariance = new Vector3(0.1f, 0.1f, 0.1f);
    public float currentAmmo { get; private set; }

    [Header("Shooting Parameters")]
    [SerializeField] private Transform shootPoint;
    [SerializeField] private Transform muzzle;
    [SerializeField] private GameObject recoil;
    [SerializeField] private GameObject kickback;
    [SerializeField] private PlayerMovement player;

    [Header("UI")]
    [SerializeField] TMP_Text currentAmmoText;
    [SerializeField] TMP_Text magazineAmmoText;

    bool isShooting;
    bool isReloading;

    Animator animator;

    //Actions
    public Action OnAttack;
    public Action OnEmptyClip;

    private void Start()
    {
        animator = GetComponent<Animator>();
        currentAmmo = maxClipAmmo;
    }

    private void Update()
    {
        currentAmmo = Mathf.Clamp(currentAmmo, 0, maxClipAmmo);
        availableAmmo = Mathf.Clamp(availableAmmo, 0, maxAmmo);

        if(Input.GetKey(KeyCode.H) && !isShooting && currentAmmo > 0 && !isReloading && !player.isRunning)
        {
            StartCoroutine(Shoot());
            OnAttack?.Invoke();
        }
        else if(Input.GetKey(KeyCode.H) && !isShooting && currentAmmo == 0 && !isReloading && !player.isRunning)
        {
            OnEmptyClip?.Invoke();
        }

        if(Input.GetKeyDown(KeyCode.R) && maxClipAmmo > 0 && !isReloading)
        {
            StartCoroutine(Reload());
        }

        if(currentAmmo == 0 && availableAmmo > 0 && !isReloading)
        {
            StartCoroutine(Reload());
        }

        currentAmmoText.text = currentAmmo.ToString();
        magazineAmmoText.text = availableAmmo.ToString();

        AnimatePlayer();
    }

    private IEnumerator Shoot()
    {
        mf.Play();


        
        RaycastHit hit;
        Vector3 direction = GetDirection();

        if(Physics.Raycast(shootPoint.position, direction, out hit, maxDistance))
        {
            if(hit.rigidbody != null)
            {
                hit.rigidbody.AddForce(direction * impactForce);
            }
            
            TrailRenderer trail = Instantiate(bulletTrail, muzzle.position, Quaternion.identity);

            StartCoroutine(SpawnTrails(trail, hit));
            CreateBulletImpact(bulletImpact, hit);
            CreateBulletHole(bulletHoleDecal, hit);
        }

        currentAmmo -= shootCost;

        isShooting = true;

        yield return new WaitForSecondsRealtime(fireRate);

        isShooting = false;
    }

    private IEnumerator Reload()
    {
        animator.SetTrigger("Reload");
        isReloading = true;
        yield return new WaitForSecondsRealtime(reloadingTime);
        availableAmmo -= (maxClipAmmo - currentAmmo);
        currentAmmo += (maxClipAmmo - currentAmmo);
        isReloading = false;
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
                trailRenderer.transform.position = Vector3.Lerp(startPosition, startPosition * maxDistance, time);
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

    private void CreateBulletHole(GameObject decal, RaycastHit hit)
    {
        GameObject instance = Instantiate(decal, hit.point, Quaternion.LookRotation(hit.normal));
        instance.transform.parent = hit.collider.gameObject.transform;
    }

    private void CreateBulletImpact(GameObject effect, RaycastHit hit)
    {
        GameObject instance = Instantiate(effect, hit.point, Quaternion.LookRotation(hit.normal));
        instance.transform.parent = hit.collider.gameObject.transform;
        Destroy(instance, 2f);
    }

    private void AnimatePlayer()
    {
        if(player.isRunning)
        {
            animator.SetBool("Running", true);
            animator.SetFloat("Velocity", 1f);
        }
        else if(!player.isRunning && animator.GetBool("Running") == true)
        {
            animator.SetBool("Running", false);
            animator.SetFloat("Velocity", .5f);
        }
    }
}
