using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KdTree3D<T> where T : Component, IEnumerator<T>
{
    private KDCollider3D collider3D;
    public readonly int N;

    public KdTree3D()
    {

    }

    public void Add()
    {

    }

    public class KDCollider3D
    {
        private Rect up = Rect.zero;
        private Rect down = Rect.zero;

        public void SetRectVerticies(Rect up, Rect down)
        {
            this.up = up;
            this.down = down;
        }
    }

    public struct Item<T>
    {
        int[] key;
        T info;

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

    public struct Node<T>
    {
        Item<T> i;
        Node<T> left;
        Node<T> right;
    }
}

