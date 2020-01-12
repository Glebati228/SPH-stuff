using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Trees;
using System;

public class TestParticle : MonoBehaviour
{
#pragma warning disable 0649
    [SerializeField] private float                      maxSize;
    [SerializeField] private float                      minSize;
    [SerializeField] private int                        pointsCount;
    [SerializeField] private GameObject                 cubePrefab;
    [SerializeField, Range(0f, 10f)] private float      updateRate;
    private PointOctree<GameObject>                     pointOctree;
    private GameObject[]                                items;
    private float                                       timer;
#pragma warning restore 0649

    private void Start()
    {
        items = new GameObject[pointsCount];
        pointOctree = new PointOctree<GameObject>(maxSize, transform.position, minSize);

        for (int i = 0; i < pointsCount; i++)
        {
            Vector3 pos = new Vector3(
                    UnityEngine.Random.Range(transform.position.x - maxSize, transform.position.x + maxSize),
                    UnityEngine.Random.Range(transform.position.y - maxSize, transform.position.y + maxSize),
                    UnityEngine.Random.Range(transform.position.z - maxSize, transform.position.z + maxSize));
            items[i] = Instantiate(cubePrefab, pos, Quaternion.identity) as GameObject;
            items[i].name = i.ToString();
            pointOctree.Add(items[i], pos);
        }
    }

    void OnDrawGizmos()
    {
        if (!Application.isPlaying && pointOctree is null) return;

        pointOctree.DrawAllBounds();
        pointOctree.DrawAllObjects();
    }

    private void Update()
    {
        UpdateTree();
    }

    private void UpdateTree()
    {
        timer += Time.deltaTime;
        if (timer >= updateRate)
        {
            timer = 0f;
            pointOctree.ClearTree();

            for (int i = 0; i < pointsCount; i++)
            {
                Vector3 pos = new Vector3(
                        UnityEngine.Random.Range(transform.position.x - maxSize, transform.position.x + maxSize),
                        UnityEngine.Random.Range(transform.position.y - maxSize, transform.position.y + maxSize),
                        UnityEngine.Random.Range(transform.position.z - maxSize, transform.position.z + maxSize));
                items[i].transform.position = pos;
                pointOctree.Add(items[i], pos);
            }
        }
    }
}

