﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinding : MonoBehaviour
{
    public static PathFinding instance;

    public Node startPoint;
    public Node endPoint;
    public static List<Node> path;
    
    Material PathMat;
    Material PathEndMat;
    Material NormMat;
    Material Ground;
    Material Water;
    Material Mountain;
    // Start is called before the first frame update

    public void Start()
    {
        if (instance != null)
            Destroy(gameObject); 
        else
            instance = this; 

        path = new List<Node>();
        PathEndMat = Resources.Load<Material>("EndPoint");
        PathMat = Resources.Load<Material>("TileOuter");
        NormMat = Resources.Load<Material>("TileInnerDebug");
        Ground = Resources.Load<Material>("Ground");
        Water = Resources.Load<Material>("Water");
        Mountain = Resources.Load<Material>("Mountain");
    }

    public void ClearPathEnds()
    {
        startPoint = null;
        endPoint = null;
    }

    public void Update()
    {
        if(Input.GetKeyDown(KeyCode.P) && endPoint != null)
        {
            FindPath();
        }
    }

    public void TracePath(Node node)
    {
        path.Clear();
        Debug.Log("Pathing:");
        path.Add(node);
        while(node.parent != null)
        {
            path.Add(node.parent);
            node = node.parent;
            //Debug.Log(hex.totalMoveDifficulty);
        }
        foreach (Node hexagon in path)
        {
            node.gCost = int.MaxValue;
            node.hCost = int.MaxValue;
            hexagon.parent = null;
        }
    }

    public void FindPath()
    {
        Heap<Node> openList = new Heap<Node>(TileGeneration.GetNumHexes());
        HashSet<Node> closedList = new HashSet<Node>();

        openList.Add(startPoint);

        bool foundDest = false;
        while (openList.Count != 0 && !foundDest)
        {
            Node currentNode = openList.RemoveFirst();
            closedList.Add(currentNode);

            if (currentNode == endPoint)
            {
                TracePath(currentNode);
                return;
            }

            foreach(Node neighbor in currentNode.connections)
            {
                if (!neighbor.walkable || closedList.Contains(neighbor))
                    continue;

                float newMovementCostToNeighbor = currentNode.gCost + GetDistance(currentNode, neighbor);

                if (newMovementCostToNeighbor < neighbor.gCost || openList.Contains(neighbor))
                {
                    neighbor.gCost = newMovementCostToNeighbor;
                    neighbor.hCost = GetDistance(neighbor, endPoint);
                    neighbor.parent = currentNode;
                    
                    if(!openList.Contains(neighbor))
                        openList.Add(neighbor);

                }
            }


        }
        if (!foundDest)
        {
            Debug.Log("Failed to find destination");
            return;
        }
        
    }

    public static void SetStart(Node startNode)
    {
        instance.startPoint = startNode;
    }

    public static void SetEnd(Node endNode)
    {
        instance.endPoint = endNode;
    }

    public static Node GetStartNode()
    { return instance.startPoint; }
    public static Node GetEndPoint()
    { return instance.endPoint; }

    float GetDistance(Node a, Node b)
    {
        return (b.worldPos - a.worldPos).magnitude;
    }
}