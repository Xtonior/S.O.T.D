using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class HumanBone
{
    public HumanBodyBones bone;
    public float weight;
}

public class SoliderWeaponIK : MonoBehaviour
{
    public Transform targetTransform;
    public Transform aimTransform;
    public Transform orientation;

    public float angleLimit;
    public float distanceLimit;

    public float maxDistance;

    public HumanBone[] humanBones;
    Transform[] boneTransforms;

    [Range(0, 1)]
    public float weight;

    public bool aimed { get; private set; }
    
    void Start()
    {
        Animator animator = GetComponent<Animator>();
        boneTransforms = new Transform[humanBones.Length];

        for (int i = 0; i < boneTransforms.Length; i++)
        {
            boneTransforms[i] = animator.GetBoneTransform(humanBones[i].bone);
        }
    }

    Vector3 GetTargetPosition()
    {
        Vector3 targetDir = targetTransform.position - aimTransform.position;
        Vector3 aimDir = orientation.forward;

        float blend = 0.0f;

        float targetAngle = Vector3.Angle(targetDir, aimDir);

        if(targetAngle > angleLimit)
        {
            blend += (targetAngle - angleLimit) / 50.0f;
        }

        float targetDist = targetDir.magnitude;

        if(targetDist < distanceLimit)
        {
            blend += distanceLimit - targetDist;
        }

        Vector3 dir = Vector3.Slerp(targetDir, aimDir, blend);

        return aimTransform.position + dir;
    }

    void LateUpdate()
    {
        if (aimTransform != null && targetTransform != null && Vector3.Distance(targetTransform.position, orientation.position) <= maxDistance)
        {
            Vector3 targetPosition = GetTargetPosition();
            
            for (int b = 0; b < boneTransforms.Length; b++)
            {
                Transform bone = boneTransforms[b];
                float boneWeight = humanBones[b].weight * weight;
                AimAtTarget(bone, targetPosition, boneWeight);
            }
        }
        else
        {
            return;
        }
    }

    private void AimAtTarget(Transform bone, Vector3 targetPosition, float weight)
    {
        Vector3 aimDirection = aimTransform.forward;
        Vector3 targetDirection = targetPosition - aimTransform.position;
        Quaternion aimTowards = Quaternion.FromToRotation(aimDirection, targetDirection);
        Quaternion blendedRotation = Quaternion.Slerp(Quaternion.identity, aimTowards, weight);
        bone.rotation = blendedRotation * bone.rotation;
        aimed = true;
    }

    public void SetTarget(Transform target)
    {
        targetTransform = target;
    }

    public void SetAim(Transform aim)
    {
        aimTransform = aim;
    }
}
