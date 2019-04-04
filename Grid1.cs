using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid1 : MonoBehaviour
{
    
    public LayerMask UnWalkableMask;// warstwa dla kolizji
    public float nodeRadius;//wielkosc kostek
    public float space;// pomiedzy kostkami
    public Vector2 gridWorldSize;
    public Node[,] grid;

    public List<Node> path;
    private float nodeDiameter;
    private int gridSizeX, gridSizeY;

    void Awake()
    {
        nodeDiameter = nodeRadius * 2;
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);
        CreateGrid();
    }

    void CreateGrid()
    {
        grid = new Node[gridSizeX, gridSizeY];
        Vector3 worldBottomLeft = transform.position - Vector3.right * gridWorldSize.x / 2 - Vector3.up * gridWorldSize.y / 2;
        for (int i = 0; i < gridSizeX; i++){
            for (int j = 0; j < gridSizeY; j++)
            {
                Vector3 worldPoint = worldBottomLeft + Vector3.right * (i * nodeDiameter + nodeRadius) +
                                     Vector3.up * (j * nodeDiameter + nodeRadius);
                bool walkable = !(Physics2D.OverlapBox(worldPoint,new Vector2(nodeRadius,nodeRadius),0.0f,UnWalkableMask));
                grid[i,j]=new Node(walkable,worldPoint,i,j);
            }
        }

    }
    public List<Node> GetNeighbours(Node node)
    {
        List<Node> neighbours = new List<Node>();

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0)
                    continue;

                int checkX = node.gridX + x;
                int checkY = node.gridY + y;

                if (checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY)
                {
                    neighbours.Add(grid[checkX, checkY]);
                }
            }
        }

        return neighbours;
    }

    public Node NodeFromWorldPoint(Vector3 worldPosition)
    {
        float percentX = (worldPosition.x + gridWorldSize.x / 2) / gridWorldSize.x;
        float percentY = (worldPosition.y + gridWorldSize.y / 2) / gridWorldSize.y;
        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);

        int x = Mathf.RoundToInt((gridSizeX - 1) * percentX);
        int y = Mathf.RoundToInt((gridSizeY - 1) * percentY);
        return grid[x, y];
    }
    public Node FindNearestAvailable(Node targetNode)
    {
        Node WalkAble=targetNode;
        Node OldNeighbour=new Node(true,Vector2.zero,200,200);
        /*
        int i = 0;
        Node WalkAble=targetNodeode;
        bool found = false;

        
        while (found==false)
        {
            
            if (grid[node.gridX+i, node.gridY].walkable==true)
            {
                WalkAble = grid[node.gridX, node.gridY];
                found = true;
            }
            if (grid[node.gridX-i, node.gridY].walkable == true)
            {
                WalkAble = grid[node.gridX, node.gridY];
                found = true;
            }
            if (grid[node.gridX, node.gridY+i].walkable == true)
            {
                WalkAble = grid[node.gridX, node.gridY];
                found = true;
            }
            if (grid[node.gridX, node.gridY-i].walkable == true)
            {
                WalkAble = grid[node.gridX, node.gridY];
                found = true;
            }
            i++;

        }*/
        foreach (Node neighbour in grid)
        {
            if (!neighbour.walkable)
            {
                continue;
            }

            
            if (GetDistance(targetNode, neighbour) < GetDistance(targetNode, OldNeighbour))
            {
                
                OldNeighbour = neighbour;


            }
        }

        return OldNeighbour;

    }

    /*
     void OnDrawGizmos()
     {
         Gizmos.DrawWireCube(transform.position, new Vector2(gridWorldSize.x,gridWorldSize.y));
         if (grid != null)
         {
             foreach (Node n in grid)
             {
                 Gizmos.color = (n.walkable) ? Color.white : Color.red;

                 if (path!= null)
                     if (path.Contains(n))
                         Gizmos.color = Color.black;
                 Gizmos.DrawCube(n.worldPosition, Vector3.one * (nodeDiameter -space));
             }
         }
     }*/
    int GetDistance(Node nodeA, Node nodeB)
    {
        int dstX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
        int dstY = Mathf.Abs(nodeA.gridY - nodeB.gridY);

        if (dstX > dstY)
            return 14 * dstY + 10 * (dstX - dstY);
        return 14 * dstX + 10 * (dstY - dstX);
    }

}

