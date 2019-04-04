using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PathFinding : MonoBehaviour
{
    
    public Transform seeker,target;
    Grid1 grid;


    void Awake()
    {
        grid = GetComponent<Grid1>();
    }

   

    public List<Node> FindPath(Vector2 startPos, Vector2 targetPos)
    {
        List<Node> Path = new List<Node>();
        Node startNode = grid.NodeFromWorldPoint(startPos);
        Node targetNode = grid.NodeFromWorldPoint(targetPos);

        if (startNode.walkable && targetNode.walkable)
        {
            List<Node> openSet = new List<Node>();
            HashSet<Node> closedSet = new HashSet<Node>();
            openSet.Add(startNode);

            while (openSet.Count > 0)
            {
                Node node = openSet[0];
                for (int i = 1; i < openSet.Count; i++)
                {
                    if (openSet[i].fCost < node.fCost || openSet[i].fCost == node.fCost)
                    {
                        if (openSet[i].hCost < node.hCost)
                            node = openSet[i];
                    }
                }

                openSet.Remove(node);
                closedSet.Add(node);

                if (node == targetNode)
                {
                    RetracePath(startNode, targetNode);
                    return grid.path;
                }

                foreach (Node neighbour in grid.GetNeighbours(node))
                {
                    if (!neighbour.walkable || closedSet.Contains(neighbour))
                    {
                        continue;
                    }

                    int newCostToNeighbour = node.gCost + GetDistance(node, neighbour);
                    if (newCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour))
                    {
                        neighbour.gCost = newCostToNeighbour;
                        neighbour.hCost = GetDistance(neighbour, targetNode);
                        neighbour.parent = node;

                        if (!openSet.Contains(neighbour))
                            openSet.Add(neighbour);
                    }
                }
            
        }    
    }


        if (startNode.walkable && !targetNode.walkable)
        {
            targetNode=grid.FindNearestAvailable(targetNode);
            grid.path = FindPath(startPos, targetNode.worldPosition);
            
        }

        /*  grid.path = FindPath(startPos, FindNearestAvailable(targetNode, targetNode));*/
        return grid.path;
        
    }

    void GettingCloser(List<Node> path) // sprawdza ostatnia kostke ktora jest walkable
    {
        List<Node> closer = new List<Node>();

        for (int i = 1; i < path.Count; i++)
        {
            if (path[i].walkable)
            {
                closer.Add(path[i]);
            }

        }

        grid.path = closer;

    }



    void RetracePath(Node startNode, Node endNode)
    {
        List<Node> path = new List<Node>();
        Node currentNode = endNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }
        
        path.Reverse();
        path.Add(endNode);

        grid.path = path;
        


    }

    
   

    

    int GetDistance(Node nodeA, Node nodeB)
    {
        int dstX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
        int dstY = Mathf.Abs(nodeA.gridY - nodeB.gridY);

        if (dstX > dstY)
            return 14 * dstY + 10 * (dstX - dstY);
        return 14 * dstX + 10 * (dstY - dstX);
    }
    
}