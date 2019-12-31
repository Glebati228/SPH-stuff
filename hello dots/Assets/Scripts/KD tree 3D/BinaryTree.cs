using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Trees
{
    public class BinaryTree<T> where T : Component
    {
        Node<T> root;

        public BinaryTree(Node<T> root)
        {
            this.root = root;
        }

        public BinaryTree()
        {
            root = null;
        }

        public Node<T> GetRoot()
        {
            return root;
        }

        public Node<T> search(Node<T> node, Item<T> key)
        {
            if (node == null || node.item.key == key.key)
                return node;

            if (node.item.key > key.key)
            {
                return search(node.right, key);
            }

            return search(node.left, key);
        }

        public void insert(Node<T> node, Item<T> item)
        {
            this.root = Insert(node, item);
        }

        private Node<T> Insert(Node<T> node, Item<T> item)
        {
            if (node == null)
            {
                node = new Node<T>(item);
                return node;
            }

            if (item.key < node.item.key)
            {
                node.left = Insert(node.left, item);
            }
            else if (item.key > node.item.key)
            {
                node.right = Insert(node.right, item);
            }

            return node;
        }

        public void remove(Node<T> node, Item<T> item)
        {
            root = Remove(node, item);
        }

        private Node<T> Remove(Node<T> node, Item<T> item)
        {
            if (node == null)
            {
                return node;
            }

            if (item.key < node.item.key)
            {
                node.left = Remove(node, item);
            }
            else if (item.key > node.item.key)
            {
                node.right = Remove(node, item);
            }
            else
            {
                if (node.left == null)
                {
                    return root.right;
                }
                else if (node.right == null)
                {
                    return root.left;
                }
                else
                {
                    node.item = MinValue(node.right);

                    node.right = Remove(node.right, node.item);
                }
            }

            return root;
        }

        public Item<T> MinValue(Node<T> node)
        {
            Item<T> minv = node.item;
            while (node.left != null)
            {
                minv = node.left.item;
                node = node.left;
            }
            return minv;
        }

        public void print(Node<T> node)
        {
            if (node != null)
            {
                print(node.left);
                Debug.Log("   " + node.item.key + " / " + node.item.item);
                print(node.right);
            }
        }
    }

    public class Node<T> where T : Component
    {
        public Item<T> item;
        public Node<T> left;
        public Node<T> right;

        public Node(Item<T> item)
        {
            this.item = item;
            left = right = null;
        }
    }

    public class Item<T> where T : Component
    {
        public T item;
        public int key;

        public Item(T item, int key)
        {
            this.item = item;
            this.key = key;
        }
    }
}

