using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

[System.Serializable]
public struct SPHCollider : IComponentData
{
    public float3 position;
    public float3 right;
    public float3 up;
    public float2 scale;
}

public class SPHColliderComponent : ComponentDataProxy<SPHCollider>
{

}
