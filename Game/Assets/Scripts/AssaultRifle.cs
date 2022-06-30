using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using TMPro;

public class AssaultRifle : Weapon
{
    [SerializeField] private ParticleSystem mf;

    [SerializeField] private GameObject bulletImpact;
    [SerializeField] private VisualEffect bloodBurst;

    [SerializeField] private GameObject bulletHoleDecal;
    [SerializeField] private GameObject bloodSplashDecal;

    //[SerializeField] private BloodSplash bloodSplash;

    [SerializeField] private TrailRenderer bulletTrail;
    
    [SerializeField] private TMP_Text currentAmmoText;
    [SerializeField] private TMP_Text magazineAmmoText;

    [SerializeField] private LayerMask defaultLayerMask;
    [SerializeField] private LayerMask dummyLayerMask;

    private bool isShooting;
    private bool isReloading;
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        currentAmmo = Mathf.Clamp(currentAmmo, 0, maxClipAmmo);
        availableAmmo = Mathf.Clamp(availableAmmo, 0, maxAmmo);

        if(Input.GetKey(KeyCode.H) && !isShooting && currentAmmo > 0 && !isReloading && !player.isRunning)
        {
            Shoot();
            OnAttack?.Invoke();
        }
        else if(Input.GetKey(KeyCode.H) && !isShooting && currentAmmo == 0 && !isReloading && !player.isRunning)
        {
            OnEmptyClip?.Invoke();
        }

        if(Input.GetKeyDown(KeyCode.R) && maxClipAmmo > 0 && !isReloading)
        {
            Reload();
        }

        if(currentAmmo == 0 && availableAmmo > 0 && !isReloading)
        {
            Reload();
        }

        currentAmmoText.text = currentAmmo.ToString();
        magazineAmmoText.text = availableAmmo.ToString();

        AnimatePlayer();
    }

    public override void Shoot()
    {
        StartCoroutine(Shooting());
    }

    public override void Reload()
    {
        StartCoroutine(Reloading());
    }

    public override void CreateBulletHole(GameObject decal, RaycastHit hit)
    {
        GameObject instance = Instantiate(decal, hit.point, Quaternion.LookRotation(hit.normal));
        instance.transform.parent = hit.collider.gameObject.transform;
    }

    public override void CreateBulletImpact(GameObject effect, RaycastHit hit)
    {
        GameObject instance = Instantiate(effect, hit.point, Quaternion.LookRotation(hit.normal));
        instance.transform.parent = hit.collider.gameObject.transform;
        Destroy(instance, 2f);
    }

    public void CreateBloodSplash(RaycastHit hit)
    {
        GameObject instance = Instantiate(bloodBurst.gameObject, hit.point, Quaternion.LookRotation(hit.normal));
        instance.transform.parent = hit.collider.gameObject.transform;
        Destroy(instance, 2f);
    }

    private IEnumerator Reloading()
    {
        animator.SetTrigger("Reload");
        isReloading = true;
        yield return new WaitForSecondsRealtime(reloadingTime);
        availableAmmo -= (maxClipAmmo - currentAmmo);
        currentAmmo += (maxClipAmmo - currentAmmo);
        isReloading = false;
    }

    private IEnumerator Shooting()
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

            if(hit.transform.gameObject.GetComponent<FlameableBarrel>())
            {
                hit.transform.gameObject.GetComponent<FlameableBarrel>().TakeDamage(damage);
            }
        
            recoilSystem.RecoilFire();

            TrailRenderer trail = Instantiate(bulletTrail, muzzle.position, Quaternion.identity);

            StartCoroutine(SpawnTrails(trail, hit));
            
            CreateBulletHole(bulletHoleDecal, hit);
        }

        currentAmmo -= shootCost;

        isShooting = true;

        yield return new WaitForSecondsRealtime(fireRate);

        isShooting = false;
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
            animator.SetFloat("Velocity", .8f);
        }
    }
}
