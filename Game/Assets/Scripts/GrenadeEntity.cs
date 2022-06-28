using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeEntity : MonoBehaviour
{
    [SerializeField] float defaultThrowForce;
    [SerializeField] float delay;
    [SerializeField] Explosion explosion;
    private Rigidbody rb;

    public void Throw(Vector3 dir, float throwForce)
    {
        rb = GetComponent<Rigidbody>();
        rb.AddForce(dir * Mathf.Max(throwForce, defaultThrowForce), ForceMode.Impulse);
        rb.AddTorque(dir * Mathf.Max(throwForce, defaultThrowForce), ForceMode.Impulse);
        StartCountdown();
    }

    public void StartCountdown()
    {
        StartCoroutine(Countdown());
    }

    IEnumerator Countdown()
    {
        yield return new WaitForSeconds(delay);
        explosion.Explode();
        Destroy(gameObject, 0.1f);
    }
}
