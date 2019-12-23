using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

[System.Serializable]
public struct ColliderSPH : IComponentData
{
    public float3   position;
    public float3   right;
    public float3   up;
    public float2   scale;
}

[System.Serializable]
public struct ParticleSPH : IComponentData
{
    public float3   velocity;
    public float    radius;
    public float    smoothingRadius;
    public float    smoothingRadiusSq;
    public float    mass;
    public float    restDencity;
    public float    viscosity;
    public float    gravityMult;
    public float    drag;
}