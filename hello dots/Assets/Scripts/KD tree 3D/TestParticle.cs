using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Trees;

public class TestParticle : MonoBehaviour
{
    BinaryTree<Transform> binaryTree = new BinaryTree<Transform>();
#pragma warning disable 0649
    [Header("Quad Tree props:")]
    [SerializeField] private int pointsCount;
    [SerializeField] private int capacity;
    [SerializeField] private Rect rect;
    [SerializeField] private float delay;
    private QuadTree2D tree;

    private List<Vector2> points;
#pragma warning restore 0649
    private float timer = default;

    private void Start()
    {
        points = new List<Vector2>();

        tree = new QuadTree2D(rect, capacity);
        for (int i = 0; i < pointsCount; i++)
        {
            float x = UnityEngine.Random.Range(tree.boundary.x, tree.boundary.width);
            float y = UnityEngine.Random.Range(tree.boundary.y, tree.boundary.height);
            Vector2 point = new Vector2(x, y);
            points.Add(point);
            tree.InsertF(point);
        }
        tree.AreaSearch(new Rect(50, 50, 50, 75)).ForEach(item => Debug.Log(item));
    }

    //private void OnDrawGizmos()
    //{
    //    if (!Application.isPlaying) return;
    //    Gizmos.color = Color.blue;
    //    tree.GraphDebug();
    //    Gizmos.color = Color.red;
    //    if (points.Count > 0)
    //        foreach (Vector2 item in points)
    //        {
    //            Gizmos.DrawSphere(item, 1f);
    //        }
    //}

    private void Update()
    {
        ChangePoints();
    }

    private void ChangePoints()
    {
        timer += Time.deltaTime;
        if (delay > 0f && timer >= delay)
        {
            timer = 0f;
            points.Clear();
            tree = new QuadTree2D(rect, capacity);
            for (int i = 0; i < pointsCount; i++)
            {
                float x = UnityEngine.Random.Range(tree.boundary.x, tree.boundary.width);
                float y = UnityEngine.Random.Range(tree.boundary.y, tree.boundary.height);
                Vector2 point = new Vector2(x, y);
                points.Add(point);
                tree.Insert(point);
            }
        }
    }

    //for comparison with a tree
    private Vector2 BadSearch(Vector2 first)
    {
        for (int i = 0; i < points.Count; i++)
        {
            if (Mathf.Abs((first - points[i]).magnitude) < 0.01f)
                return points[i];
        }

        return Vector2.zero;
    }
}
