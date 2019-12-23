using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Transforms;
using Unity.Collections;
using Unity.Rendering;
using Unity.Mathematics;

public class TestECS : MonoBehaviour
{
    //System props
    [SerializeField] private Mesh mesh;
    [SerializeField] private Material material;
    [SerializeField] private float maxSpeed;
    [SerializeField] private float maxDistance;
    [SerializeField] private int count;

    // Start is called before the first frame update
    void Start()
    {
        EntityManager manager = World.Active.EntityManager;

        EntityArchetype archetype = manager.CreateArchetype(
            typeof(LevelComponent),
            typeof(Translation),
            typeof(RenderMesh),
            typeof(LocalToWorld),
            typeof(SpeedComponent)
            );
        //Entity entity = manager.CreateEntity(typeof(LevelComponent));

        NativeArray<Entity> entities = new NativeArray<Entity>(count, Allocator.Temp);
        manager.CreateEntity(archetype, entities);

        for (int i = 0; i < entities.Length; i++)
        {
            Entity entity = entities[i];
            manager.SetComponentData(entity, new LevelComponent {
                level = 10f
            });

            manager.SetComponentData(entity, new Translation()
            {
                Value = new float3(UnityEngine.Random.Range(-maxDistance, maxDistance), 0f, 0f)
            });

            manager.SetComponentData(entity, new SpeedComponent()
            {
                speed = UnityEngine.Random.Range(1f, maxSpeed)
            });

            manager.SetSharedComponentData(entity, new RenderMesh()
            {
                mesh = mesh,
                material = material
            });
        }
       
        entities.Dispose();
    }
}
