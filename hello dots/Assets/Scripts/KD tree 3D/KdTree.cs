using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KdTree3D<T> where T : Component, IEnumerator<T>
{

    private readonly int N;

    private KDNode root;
    public KDNode Root { get => root; set => root = value; }

    private KDNode last;
    public KDNode Last { get => last; set => last = value; }

    private int counter;

    public KdTree3D(int N)
    {
        this.N = N;
    }


    //CRUD
    /// <summary>
    /// Add new item
    /// </summary>
    /// <param name="item"></param>
    public void Add(Item<T> item)
    {
        KDNode node = new KDNode();
        node.item = item;

        if (item == null)
        {
            return;
        }

        if (root == null)
        {
            root = node;
            return;
        }
        else
        {
            KDNode current = root;
            KDNode parent;
            while(true)
            {
                parent = current;
                if (true) 
                {
                    current = current.left;
                    if(current == null)
                    {
                        parent.left = node;
                        break;
                    }
                }
                else if (true)
                {
                    current = current.right;
                    if (current == null)
                    {
                        parent.right = node;
                        break;
                    }
                }
            }
            root = parent;
        }
    }

    public void Remove()
    {

    }

    public void Delete()
    {

    }

    public void GetChild()
    {

    }

    public class Item<T>
    {
        public int[] key;
        public T info;

        public Item(int[] key, T info)
        {
            this.key = key;
            this.info = info;
        }

        public Item(int N, T info)
        {
            key = new int[N];
            this.info = info;
        }
    };

    public class KDNode
    {
        public Item<T> item;
        public KDNode left { get; set; }
        public KDNode right { get; set; }
    }
}


