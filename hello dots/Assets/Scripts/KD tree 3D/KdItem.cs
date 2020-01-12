using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KdItem : MonoBehaviour
{
    private KdTree3D<CubeBehaviour> items;
    [SerializeField] private List<CubeBehaviour> cubes;
    [SerializeField] private GameObject prefab;
    [SerializeField] private int cubesCount;
    // Start is called before the first frame update
    void Start()
    {
        cubes = new List<CubeBehaviour>(cubesCount);

        for (int i = 0; i < cubesCount; i++)
        {
            GameObject go = Instantiate(
                //prefab
                prefab,
                //random pos
                new Vector3(
                    UnityEngine.Random.Range(-20f, 20f),
                    UnityEngine.Random.Range(-20f, 20f),
                    UnityEngine.Random.Range(-20f, 20f)
                    ),
                //rotation
                Quaternion.identity
                ) as GameObject;

            cubes.Add(go.AddComponent<CubeBehaviour>());
        }

        items = new KdTree3D<CubeBehaviour>(cubes);
    }

    void OnDrawGizmos()
    {
        if (!Application.isPlaying) return;

        items.DrawTree(KdTree3D<CubeBehaviour>.Root);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
