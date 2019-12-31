using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KdTree3D<T> where T : Component
{
    private Node3D<T> root;
    public Node3D<T> Root { get => root; set => root = value; }
    public float threshold;

    public Node3D<T> Search(Node3D<T> node, Item3D<T> item)
    {
        if (node == null || Approximately(node.Item.posKey.magnitude, item.posKey.magnitude))
        {
            return node;
        }

        if (node.Left.Item.posKey.magnitude < item.posKey.magnitude)
        {
            return Search(node.Left, item);
        }

        return Search(node.Right, item);
    }

    public void insert(Node3D<T> node, Item3D<T> item)
    {
        root = Insert(node, item);
    }

    private Node3D<T> Insert(Node3D<T> node, Item3D<T> item)
    {
        if(node == null)
        {
            node = new Node3D<T>();
            node.Item = item ?? throw new System.NullReferenceException();
            return node;
        }
        
        if (item.posKey.magnitude < node.Item.posKey.magnitude)
        {
            node = Insert(node.Left, item);
        }
        else if (item.posKey.magnitude > node.Item.posKey.magnitude)
        {
            node = Insert(node.Right, item);
        }

        return node;
    }

    public void remove()
    {

    }

    private Node3D<T> Remove(Node3D<T> node, Item3D<T> item)
    {
        if (node == null)
        {
            return node;
        }

        if (node == null)
        {
            return node;
        }

        if (item.posKey.magnitude < node.Item.posKey.magnitude)
        {
            node.Left = Remove(node, item);
        }
        else if (item.posKey.magnitude > node.Item.posKey.magnitude)
        {
            node.Right = Remove(node, item);
        }
        else
        {
            if (node.Left == null)
            {
                return root.Right;
            }
            else if (node.Right == null)
            {
                return root.Left;
            }
            else
            {
             //   node.Item = MinValue(node.Right);

                node.Right = Remove(node.Right, node.Item);
            }
        }

        return root;
    }

    private bool Approximately(float first, float second)
    {
        return Mathf.Abs(first - second) < this.threshold;    
    }

    public class Node3D<K> where K : Component
    {
        private Item3D<T> item;
        public Item3D<T> Item { get => item; set => item = value; }
        private Node3D<T> left;
        public Node3D<T> Left { get => left; set => left = value; }
        private Node3D<T> right;
        public Node3D<T> Right { get => right; set => right = value; }
    }

    public class Item3D<K> where K : Component
    {
        private T item;
        public T Item { get => item; set => item = value; }
        public Vector3 posKey;
    }
}


