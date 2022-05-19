using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : IHeapItem<Node>
{
    public Node[] connections;
    public bool walkable;
    public Vector3 worldPos;
    public Hexagon attachedHex;

    public float gCost;
    public float hCost;
    public Node parent;
    int heapIndex;



    public Node(bool _walkable, Vector3 _worldPos, Hexagon _attachedHex)
    {
        walkable = _walkable;
        worldPos = _worldPos;
        attachedHex = _attachedHex;
        connections = new Node[6];

    }

    public float fCost
    {
        get { return gCost+hCost; }
    }

    public int HeapIndex
    {
        get { return heapIndex; }set { heapIndex = value; }
    }

    public int CompareTo(Node nodeToCompare)
    {
        int compare = fCost.CompareTo(nodeToCompare.fCost);
        if (compare == 0)
        {
            compare = hCost.CompareTo(nodeToCompare.hCost);
        }
        return -compare;
    }
}
