using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiSensor : MonoBehaviour
{
    public float radius;
    [Range(0, 360)]
    public float angle;
    
    public GameObject player;
    public Transform origin;

    public LayerMask targetLayer;

    public bool canSee;
    public bool aimOnPlayer;

    private RaycastHit hit;
    public Vector3 lastTargetPoint { get; private set; }

    private void Start()
    {
        StartCoroutine(FOVRoutine());
    }

    private IEnumerator FOVRoutine()
    {
        WaitForSeconds wait = new WaitForSeconds(0.2f);

        while(true)
        {
            yield return wait;
            FOVCheck();
        }
    }

    private void FOVCheck()
    {
        Vector3 direction = player.transform.position - origin.position;
        float ang = Vector3.Angle(origin.forward, direction);
        float distance = Vector3.Distance(origin.position, player.transform.position);

        if(Physics.Raycast(origin.position, direction, out hit, distance))
        {
            if(ang < angle / 2f && hit.transform.gameObject == player)
            {
                canSee = true;
                lastTargetPoint = player.transform.position;
                Aim();
            }
            else
            {
                canSee = false;
            }
        }
        else
        {
            canSee = false;
        }
    }

    private void Aim()
    {
        RaycastHit raycastHit;

        if(Physics.Raycast(origin.position, origin.forward, out raycastHit, Vector3.Distance(origin.position, player.transform.position) + 1f))
        {
            if(raycastHit.transform.gameObject == player)
            {
                aimOnPlayer = true;
            }
            else
            {
                aimOnPlayer = false;
            }
        }
        else
        {
            aimOnPlayer = false;
        }
    }
}
