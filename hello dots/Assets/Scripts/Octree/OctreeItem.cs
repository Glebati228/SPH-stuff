using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OctreeItem : MonoBehaviour
{
    public List<OctreeNode> nodes = new List<OctreeNode>();
    private Vector3 previewPos;


    // Start is called before the first frame update
    void Start()  
    {
        previewPos = transform.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (previewPos != transform.position)
        {
            UpdateTree();
            previewPos = transform.position;
        }
    }

    private void UpdateTree()
    {
        OctreeNode.Root.Insert(this);

        List<OctreeNode> sNodes = new List<OctreeNode>();
        List<OctreeNode> oNodes = new List<OctreeNode>();

        foreach (var item in nodes)
        {
            if (!item.Contains(transform.position))
            {
                oNodes.Add(item);
            }
            else
            {
                sNodes.Add(item);
            }
        }

        nodes = sNodes;

        foreach (var item in oNodes)
        {
            item.TryDeleteSubdivision(this);
        }
    }
}
