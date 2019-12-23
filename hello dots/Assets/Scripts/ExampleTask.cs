using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;
using Unity.Jobs;
using Unity.Collections;
using Unity.Burst;
using UnityEngine.Jobs;

public class ExampleTask : MonoBehaviour
{
    [SerializeField] private bool useJob;
    [SerializeField] private Transform unitPrefab;
    [SerializeField] private int unitCount;
    [SerializeField] private List<Unit> units;

    public class Unit
    {
        public Transform transform;
        public float speed;
    }

    // Start is called before the first frame update
    void Start()
    {
        units = new List<Unit>();
        for (int i = 0; i < unitCount; i++)
        {
            Transform unitTransform = Instantiate(unitPrefab, new Vector3(UnityEngine.Random.Range(-i * .0f, i * .0f), 1f, UnityEngine.Random.Range(-i * .0f, i * .0f)), Quaternion.identity);
            units.Add(new Unit
            {
                transform = unitTransform,
                speed = UnityEngine.Random.Range(1f, 3f)  
            });
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (useJob)
        {
            //NativeArray<float3> poses = new NativeArray<float3>(unitCount, Allocator.TempJob);
            NativeArray<float> speeds = new NativeArray<float>(unitCount, Allocator.TempJob);
            TransformAccessArray array = new TransformAccessArray(unitCount);

            for (int i = 0; i < units.Count; i++)
            {
                //poses[i] = units[i].transform.position;
                speeds[i] = units[i].speed;
                array.Add(units[i].transform);
            }

            CalculateCircularPosTransformJob job = new CalculateCircularPosTransformJob()
            {
                speeds = speeds,
                time = Time.time
            };

            JobHandle handle = job.Schedule(array);
            handle.Complete();

            for (int i = 0; i < units.Count; i++)
            {
                units[i].speed = speeds[i];
            }

            array.Dispose();
            speeds.Dispose();
        }
        else
        {
            for (int i = 0; i < units.Count; i++)
            {
                float x = Mathf.Cos((units[i].speed + Time.time) * 10f);
                float z = Mathf.Sin((units[i].speed + Time.time) * 10f);
                float y = default;
                units[i].transform.position += new Vector3(x, y, z);
            }
        }
    }

    [BurstCompile]
    public struct CalculateCircularPosTransformJob : IJobParallelForTransform
    {
        public NativeArray<float> speeds;
        public float time;

        public void Execute(int index, TransformAccess array)
        {

            array.position += new Vector3(math.cos((time + speeds[index]) * 10f), default, math.sin((time + speeds[index]) * 10f));
        }
    }
}
