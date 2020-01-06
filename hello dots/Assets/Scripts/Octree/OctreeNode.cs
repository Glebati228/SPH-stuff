using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OctreeNode
{
    public const int CHILDS_COUNT = 8;
    public const float TREE_VOLUME = 20f;
    public const int CAPACITY = 1;

#pragma warning disable 0649
    private int capacity;
    static OctreeNode root;
    public static OctreeNode Root
    {
        get
        {
            if (root == null)
                root = new OctreeNode(null, new List<OctreeItem>(), Vector3.zero, TREE_VOLUME, CAPACITY);
            return root;
        }
    }

    private Vector3 center;
    private float halfDimension;

    private OctreeNode[] childs = new OctreeNode[CHILDS_COUNT];
    public OctreeNode[] Childs
    {
        get { return childs; }
    }

    private List<OctreeItem> items = new List<OctreeItem>();
    public List<OctreeItem> Items
    {
        get { return items; }
    }

    private OctreeNode parent;
    public OctreeNode Parent
    {
        get { return parent; }
    }

    private GameObject GO;
    private LineRenderer lineRenderer;

#pragma warning restore 0649

    public OctreeNode(OctreeNode parent, List<OctreeItem> items, Vector3 center, float halfDimension, int capacity)
    {
        this.parent = parent;
        this.center = center;
        this.halfDimension = halfDimension;
        this.capacity = capacity;

        foreach (var item in items)
        {
            Insert(item);
        }

        GO = new GameObject();
        GO.hideFlags = HideFlags.HideInHierarchy;
        lineRenderer = GO.AddComponent<LineRenderer>();

        DrawTree();
    }

    public bool Insert(OctreeItem item)
    {
        if (Contains(item.transform.position))
        {
            if (ReferenceEquals(Childs[0], null))
            {
                AddItem(item);
                return true;
            }
            else
            {
                foreach (var child in childs)
                {
                    if (child.Insert(item))
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }

    private void AddItem(OctreeItem item)
    {
        if (!items.Contains(item))
        {
            items.Add(item);
            item.nodes.Add(this);
        }
        if(items.Count > capacity)
        {
            Subdivide();
        }
    }

    private void Subdivide()
    {
        foreach (var item in items)
        {
            item.nodes.Remove(this);
        }

        Vector3 halfPos = new Vector3(halfDimension / 2f, halfDimension / 2f, halfDimension / 2f);
        for (int i = 0; i < 4; ++i)
        {
            Childs[i] = new OctreeNode(this, this.items, halfPos + center, halfDimension / 2f, this.capacity);
            halfPos = Quaternion.Euler(0f, 90f, 0f ) * halfPos;
        }

        halfPos = new Vector3(halfDimension / 2f, -halfDimension / 2f, halfDimension / 2f);
        for (int i = 4; i < 8; ++i)
        {
            Childs[i] = new OctreeNode(this, this.items, halfPos + center, halfDimension / 2f, this.capacity);
            halfPos = Quaternion.Euler(0f, 90f, 0f) * halfPos;
        }

        items.Clear();
    }

    public void TryDeleteSubdivision()
    {
        if(ReferenceEquals(this, Root))
        {
            return;
        }
        items.Clear();
        if(!ReferenceEquals(Childs[0], null))
        {
            foreach (var item in Childs)
            {
                item.TryDeleteSubdivision();
            }
        }
    }

    public bool Contains(Vector3 item)
    {
        return (item.x < center.x + halfDimension &&
                item.x > center.x - halfDimension &&
                item.y < center.y + halfDimension &&
                item.y > center.y - halfDimension &&
                item.z < center.z + halfDimension &&
                item.z > center.z - halfDimension);
    }

    [RuntimeInitializeOnLoadMethod]
    static bool Init()
    {
        return Root == null;
    }

    public void DrawTree()
    {
        Vector3[] verticies = new Vector3[8];

        Vector3 corner = new Vector3(halfDimension, halfDimension, halfDimension);

        for (int i = 0; i < 4; i++)
        {
            verticies[i] = this.center + corner;
            corner = Quaternion.Euler(0f, 90f, 0f) * corner;
        }

        corner = new Vector3(halfDimension, -halfDimension, halfDimension);

        for (int i = 4; i < 8; i++)
        {
            verticies[i] = this.center + corner;
            corner = Quaternion.Euler(0f, 90f, 0f) * corner;
        }

        lineRenderer.useWorldSpace = true;
        lineRenderer.positionCount = 16;
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;

        lineRenderer.SetPosition(0, verticies[0]);
        lineRenderer.SetPosition(1, verticies[1]);
        lineRenderer.SetPosition(2, verticies[2]);
        lineRenderer.SetPosition(3, verticies[3]);
        lineRenderer.SetPosition(4, verticies[0]);
        lineRenderer.SetPosition(5, verticies[4]);
        lineRenderer.SetPosition(6, verticies[5]);
        lineRenderer.SetPosition(7, verticies[1]);

        lineRenderer.SetPosition(8,  verticies[5]);
        lineRenderer.SetPosition(9,  verticies[6]);
        lineRenderer.SetPosition(10, verticies[2]);
        lineRenderer.SetPosition(11, verticies[6]);
        lineRenderer.SetPosition(12, verticies[7]);
        lineRenderer.SetPosition(13, verticies[3]);
        lineRenderer.SetPosition(14, verticies[7]);
        lineRenderer.SetPosition(15, verticies[4]);
    }
}
