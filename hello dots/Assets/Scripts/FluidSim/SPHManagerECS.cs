using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Collections;
using Unity.Mathematics;
using Unity.Entities;
using Unity.Transforms;
using Unity.Rendering;

public class SPHManagerECS : MonoBehaviour
{
    [Header("Colliders")]
    [SerializeField] private int collidersCount;
    [SerializeField] private Mesh cMesh;
    [SerializeField] private Material cMaterial;
    [SerializeField] private float scale;
    //[SerializeField] private ColliderSPH colliderSPH;


    [Header("Particles")]
    [SerializeField] private int particlesCount;
    [SerializeField] private Mesh pMesh;
    [SerializeField] private Material pMaterial;
    [SerializeField] ParticleSPH particleSPH;

    void Start()
    {
        EntityManager manager = World.Active.EntityManager;

        EntityArchetype colliderArchetype = manager.CreateArchetype(
            typeof(Translation),
            typeof(Scale),
            typeof(RenderMesh),
            typeof(LocalToWorld)
            );

        NativeArray<Entity> colliders = new NativeArray<Entity>(collidersCount, Allocator.Temp);
        manager.CreateEntity(colliderArchetype, colliders);

        for (int i = 0; i < collidersCount; i++)
        {
            manager.SetComponentData(colliders[i], new Scale()
            {
                Value = scale
            });

            manager.SetSharedComponentData(colliders[i], new RenderMesh()
            {
                mesh = cMesh,
                material = cMaterial
            });
        }



        EntityArchetype particleArchetype = manager.CreateArchetype(
            typeof(Translation),
            typeof(ParticleSPH),
            typeof(RenderMesh),
            typeof(LocalToWorld)
            );

        NativeArray<Entity> particles = new NativeArray<Entity>(particlesCount, Allocator.Temp);
        manager.CreateEntity(particleArchetype, particles);

        for (int i = 0; i < particlesCount; i++)
        {
            manager.SetComponentData(particles[i], new Translation
            {
                Value = new float3(
                    i % 16 + UnityEngine.Random.Range(-0.1f, 0.1f),
                    2 + (i / 16 / 16) * 1.1f,
                    (i / 16) % 16) + UnityEngine.Random.Range(-0.1f, 0.1f)
            });

            manager.SetComponentData(particles[i], particleSPH);

            manager.SetSharedComponentData(particles[i], new RenderMesh()
            {
                mesh = pMesh,
                material = pMaterial
            });
        }

        colliders.Dispose();
        particles.Dispose();
    }
}
