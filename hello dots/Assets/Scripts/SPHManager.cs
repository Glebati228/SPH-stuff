using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.Rendering;

public class SPHManager : MonoBehaviour
{
    private EntityManager manager;
    [SerializeField] private GameObject colliderPrefab;
    [SerializeField] private GameObject particlePrefab;
    [SerializeField] private int count;


    // Start is called before the first frame update
    void Start()
    {
        manager = World.Active.EntityManager;

        AddColliders();
        AddParticles(count);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void AddColliders()
    {
        GameObject[] colliders = GameObject.FindGameObjectsWithTag("SPHCollider");

        NativeArray<Entity> entities = new NativeArray<Entity>(colliders.Length, Allocator.Temp);
        manager.Instantiate(colliderPrefab, entities);

        for (int i = 0; i < colliders.Length; i++)
        {
            manager.SetComponentData(entities[i], new SPHCollider
            {
                position = colliders[i].transform.position,
                right = colliders[i].transform.right,
                up = colliders[i].transform.up,
                scale = new float2(colliders[i].transform.localScale.x / 2f, colliders[i].transform.localScale.y / 2f)
            });
        }

        entities.Dispose();
    }

    void AddParticles(int count)
    {
        NativeArray<Entity> entities = new NativeArray<Entity>(count, Allocator.Temp);
        manager.Instantiate(particlePrefab, entities);

        for (int i = 0; i < count; i++)
        {
            manager.SetComponentData(entities[i], new Translation { Value = new float3(
                i % 16 + UnityEngine.Random.Range(-0.1f, 0.1f), 
                2 + (i / 16 / 16) * 1.1f, 
                (i / 16) % 16) + UnityEngine.Random.Range(-0.1f, 0.1f) });
        }

        entities.Dispose();
    }
}
