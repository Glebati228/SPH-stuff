using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Trees;

public class TestParticle : MonoBehaviour
{
    BinaryTree<Transform> binaryTree = new BinaryTree<Transform>();

    [Header("Quad Tree props:")]
    [SerializeField] private int pointsCount;
    [SerializeField] private int capacity;
    [SerializeField] private Rect rect;
    private QuadTree2D tree;

    private List<Vector2> points;

    private void Start()
    {
        //binaryTree.print(binaryTree.GetRoot());
        //List<Item<Transform>> items = new List<Item<Transform>>()
        //{
        //new Item<Transform>(null, 80),
        //new Item<Transform>(null, 30),
        //new Item<Transform>(null, 20),  
        //new Item<Transform>(null, 100),
        //new Item<Transform>(null, 150),
        //};

        //foreach (var item in items)
        //{
        //    binaryTree.insert(binaryTree.GetRoot(), item);
        //}
        //binaryTree.print(binaryTree.GetRoot());

        tree = new QuadTree2D(rect, capacity);
        points = new List<Vector2>();
        for (int i = 0; i < pointsCount; i++)
        {
            float x = UnityEngine.Random.Range(tree.boundary.x, tree.boundary.width);
            float y = UnityEngine.Random.Range(tree.boundary.y, tree.boundary.height);
            Vector2 point = new Vector2(x, y);
            points.Add(point);
            tree.Insert(point);
        }

        tree.ShowData();
    }

    private void OnDrawGizmos()
    {
        if (!Application.isPlaying) return;
        Gizmos.color = Color.blue;
        tree.GraphDebug();
        Gizmos.color = Color.red;
        foreach (Vector2 item in points)
        {
            Gizmos.DrawSphere(item, 1f);
        }
    }

    private void Update()
    {

    }
}
