using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class KdTree3D<T> where T : Component
{
    private const int CAPACITY = 2;
    private const int DIMENSION = 3;

    private static KDNode root;
    public static KDNode Root
    {
        get
        {
            if(root == null)
            {
                root = new KDNode(new Boundary(Vector3.zero, Vector3.one), DIMENSION + 1);
            }
            return root;
        }
    }

    /// <summary>
    /// Empty tree
    /// </summary>
    public KdTree3D()
    {

    }

    public KdTree3D(IEnumerable<T> data)
    {
        float minX = float.MaxValue;
        float minY = float.MaxValue;
        float maxX = float.MinValue;
        float maxY = float.MinValue;
        float maxZ = float.MinValue;
        float minZ = float.MaxValue;

        foreach (var item in data)
        {
            Vector3 pos = item.transform.position;
            if (pos.x < minX)
            {
                minX = pos.x;
            }
            if (pos.y < minY)
            {
                minY = pos.y;
            }
            if (pos.x > maxX)
            {
                maxX = pos.x;
            }
            if (pos.y > maxY)
            {
                maxY = pos.y;
            }
            if (pos.z < minZ)
            {
                minZ = pos.z;
            }
            if (pos.z > maxZ)
            {
                maxZ = pos.z;
            }
        }

        Root.boundary = new Boundary(new Vector3(minX, minY, minZ), new Vector3(maxX, maxY, maxZ));

        foreach (var item in data)
        {
            Insert(item, Root.depth, Root);
        }
    }

    //public bool Insert(T item, KDNode root)
    //{
    //    if (!Root.boundary.Contains(item.transform.position) && root is null)
    //        return false;

    //    root.components.Add(item);

    //    if(root.components.Count > CAPACITY)
    //    {
    //        Subdivide();
    //    }

    //    return false;
    //}

    //private void Subdivide()
    //{
    //    KDNode left;
    //    KDNode right;


    //}

    public bool Insert(T data, int depth, KDNode node)
    {
        if (!Root.boundary.Contains(data.transform.position) && Root is null)
            return false;

        int axis = node.depth % DIMENSION;
        node.components.Add(data);

        if (node.components.Count > CAPACITY)
        {

            switch (axis)
            {
                case 1:
                    XSubdivide(node);
                    break;
                case 2:
                    YSubdivide(node);
                    break;
                case 3:
                    ZSubdivide(node);
                    break;
                default:
                    break;
            }
        }


        return false;
    }

    private void XSubdivide(KDNode node)
    {
        KDNode left;
        KDNode right;

        float median = 0f;
        node.components.ForEach(item => median += item.transform.position.x / node.components.Count);

        Boundary boundary1 = node.boundary;
        Vector3 result = new Vector3(boundary1.max.x - median, boundary1.max.y, boundary1.max.z);
        Boundary boundary = new Boundary(boundary1.min, result);

        left = new KDNode(boundary, node.depth + 1);

        result = new Vector3(boundary1.min.x + median, boundary1.min.y, boundary1.min.z);
        boundary = new Boundary(result, boundary1.max);

        right = new KDNode(boundary, node.depth + 1);
    }

    private void YSubdivide(KDNode node)
    {

    }

    private void ZSubdivide(KDNode node)
    {

    }

    public void DrawTree(KDNode root)
    {
        if(root != null)
        {
            DrawTree(root.left);
            DrawTree(root.right);

           // Gizmos.DrawSphere(root.Component.transform.position, 1f);

            Vector3[] corners = root.boundary.corners;

            Gizmos.DrawLine(corners[0], corners[1]);
            Gizmos.DrawLine(corners[1], corners[2]);
            Gizmos.DrawLine(corners[2], corners[3]);
            Gizmos.DrawLine(corners[3], corners[0]);
            Gizmos.DrawLine(corners[4], corners[5]);
            Gizmos.DrawLine(corners[5], corners[6]);
            Gizmos.DrawLine(corners[6], corners[7]);
            Gizmos.DrawLine(corners[7], corners[4]);
            Gizmos.DrawLine(corners[0], corners[6]);
            Gizmos.DrawLine(corners[1], corners[5]);
            Gizmos.DrawLine(corners[2], corners[4]);
            Gizmos.DrawLine(corners[3], corners[7]);
        }
    }

    public class KDNode
    {
        internal Boundary boundary;
        internal List<T> components;
        internal KDNode left;
        internal KDNode right;
        internal int depth;

        public KDNode(Boundary boundary, int depth)
        {
            components = new List<T>();
            this.depth = depth;
            this.boundary = boundary;
        }
    }

    public struct Boundary
    {
        public Vector3[] corners;
        public Vector3 min;
        public Vector3 max;

        public Boundary(Vector3 min, Vector3 max)
        {
            corners = new Vector3[8];

            corners[0] = min;
            corners[1] = new Vector3(min.x, max.y, min.z);
            corners[2] = new Vector3(max.x, max.y, min.z);
            corners[3] = new Vector3(max.x, min.y, min.z);
            corners[4] = max;
            corners[5] = new Vector3(min.x, max.y, max.z);
            corners[6] = new Vector3(min.x, min.y, max.z);
            corners[7] = new Vector3(max.x, min.y, max.z);

            this.min = min;
            this.max = max;
        }

        public bool Contains(Vector3 point)
        {
            return point.x > min.x &&
                   point.y > min.y &&
                   point.z > min.z &&
                   point.x < max.x &&
                   point.y < max.y &&
                   point.z < max.z;
        }
    }
}
