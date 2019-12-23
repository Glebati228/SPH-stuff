using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using EventAggregation;
using UnityEngine.Jobs;
using Unity.Jobs;

public class Sample : MonoBehaviour
{
    private TransformAccessArray transforms;
    private MovementJob moveJob;
    private JobHandle handle;


    [SerializeField]
    private int enemyCount;
    [SerializeField]
    private Vector3 firstPosition;
    [SerializeField]
    private Vector3 endPostion;
    [SerializeField]
    private GameObject enemyPrefab;
    [SerializeField]
    private float maxDistance;

    private Vector3 firstPos;
    [SerializeField]
    private float step;

    void Awake()
    {
        EventAggregator.Subscribe<SpaceClicked>(OnSpaceClicked);
    }

    // Start is called before the first frame update
    void Start()
    {
        transforms = new TransformAccessArray(enemyCount);
        firstPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
      
        moveJob = new MovementJob()
        {
            maxDistance = this.maxDistance,
            firstPos = this.firstPos.magnitude,
            step = this.step,
            time = Time.deltaTime
        };
        handle = moveJob.Schedule(transforms); 
        JobHandle.ScheduleBatchedJobs();

        handle.Complete();

        if (handle.IsCompleted) transforms.Dispose();
    }

    private void OnSpaceClicked(IEventBase eventBase)
    {
        SpaceClicked ev = eventBase as SpaceClicked;
        if(ev != null)
        {
            handle.Complete();
            for (int i = 0; i < enemyCount; i++)
            {
                float x = UnityEngine.Random.Range(transform.position.x + firstPosition.x, transform.position.x + endPostion.x);
                float y = UnityEngine.Random.Range(firstPosition.y + transform.position.y, endPostion.y + transform.position.y);

                var obj = Instantiate(enemyPrefab, new Vector3(x, y, transform.position.z), Quaternion.identity) as GameObject;
            
                if (obj is null)
                {
                    Debug.LogError($"failed to create enemy!");
                }
                else
                {
                    transforms.Add(obj.transform);

                }
            }
        }
    }  
}
