using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Jobs;
using UnityEngine.Jobs;
using Unity.Burst;

[BurstCompile]
public struct MovementJob : IJobParallelForTransform
{
    public float maxDistance;
    public float firstPos;
    public float step;
    public float time;

    public void Execute(int index, TransformAccess transform)
    {
        if (Mathf.Abs(transform.position.magnitude - firstPos) < maxDistance)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z * time + step);
        }
    }
}
