using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Trees;

public class TestParticle : MonoBehaviour
{
    BinaryTree<Transform> binaryTree = new BinaryTree<Transform>();

    private void Start()
    {
        binaryTree.print(binaryTree.GetRoot());
        List<Item<Transform>> items = new List<Item<Transform>>()
        {
        new Item<Transform>(null, 80),
        new Item<Transform>(null, 30),
        new Item<Transform>(null, 20),  
        new Item<Transform>(null, 100),
        new Item<Transform>(null, 150),
        };

        foreach (var item in items)
        {
            binaryTree.insert(binaryTree.GetRoot(), item);
        }
        binaryTree.print(binaryTree.GetRoot());
    }

    private void Update()
    {

    }
}
