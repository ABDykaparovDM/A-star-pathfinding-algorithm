using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
using System;

public class Pathfinding : MonoBehaviour
{
    PathRequestManager requestManager;
    Grid grid;

    void Awake()
    {
        requestManager = GetComponent<PathRequestManager>();
        grid = GetComponent<Grid>();
    }


    public void StartFindPath(Vector3 startPos, Vector3 targetPos)
    {
        StartCoroutine(FindPath(startPos, targetPos));
    }

    IEnumerator FindPath(Vector3 startPos, Vector3 targetPos)
    {

        Vector3[] waypoints = new Vector3[0];
        bool pathSuccess = false;

        Node startNode = grid.NodeFromWorldPoint(startPos);         // Starting point A (NPC/ Player position)
        Node targetNode = grid.NodeFromWorldPoint(targetPos);       //Ending point B (Strategicall position/Entity)


        if (startNode.walkable && targetNode.walkable)
        {
            Heap<Node> openSet = new Heap<Node>(grid.MaxSize);
            HashSet<Node> closedSet = new HashSet<Node>();
            openSet.Add(startNode);

            while (openSet.Count > 0)
            {
                Node currentNode = openSet.RemoveFirst();
                closedSet.Add(currentNode);

                if (currentNode == targetNode)
                {
                    pathSuccess = true;
                    break;
                }

                foreach (Node neighbour in grid.GetNeighbours(currentNode))
                {
                    if (!neighbour.walkable || closedSet.Contains(neighbour))
                    {
                        continue;
                    }

                    int newMovementCostToNeighbour = currentNode.gCost + GetDistance(currentNode, neighbour);
                    if (newMovementCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour))       // if new neighbouring node's movement cost is cheaper than current neighbouring node's cost
                        {
                        neighbour.gCost = newMovementCostToNeighbour;
                        neighbour.hCost = GetDistance(neighbour, targetNode);
                        neighbour.parent = currentNode;

                        if (!openSet.Contains(neighbour))
                            openSet.Add(neighbour);
                    }
                }
            }
        }
        yield return null;
        if (pathSuccess)
        {
            waypoints = RetracePath(startNode, targetNode);
        }
        requestManager.FinishedProcessingPath(waypoints, pathSuccess);

    }

    Vector3[] RetracePath(Node startNode, Node endNode)
    {
        List<Node> path = new List<Node>();
        Node currentNode = endNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }
        Vector3[] waypoints = SimplifyPath(path);
        Array.Reverse(waypoints);
        return waypoints;

    }

    Vector3[] SimplifyPath(List<Node> path)
    {
        List<Vector3> waypoints = new List<Vector3>();
        Vector2 directionOld = Vector2.zero;

        for (int i = 1; i < path.Count; i++)
        {
            /**/Vector2 directionNew = new Vector2(path[i - 1].gridX - path[i].gridX, path[i - 1].gridY - path[i].gridY);
            if (directionNew != directionOld)
            {/**/
                waypoints.Add(path[i].nodeWorldPosition);
            /**/}
            directionOld = directionNew;/**/
        }
        return waypoints.ToArray();
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


/*
public Transform seeker;
public Transform target;
Grid grid;

void Start()
{
    grid = GetComponent<Grid>();
}

void Update()
{
    FindPath(seeker.position, target.position);
}

void FindPath(Vector3 startPos, Vector3 targetPos)
{
    //Stopwatch;
    Node startNode = grid.NodeFromWorldPoint(startPos);         //Starting point A (NPC/Player position)
    Node targetNode = grid.NodeFromWorldPoint(targetPos);       //Ending point B (Strategicall position/Entity)

    List<Node> openSet = new List<Node>();                      //available nodes
    HashSet<Node> closedSet = new HashSet<Node>();              //checked nodes
    openSet.Add(startNode);

    while (openSet.Count > 0)
    {
        Node node = openSet[0];
        for (int i = 1; i < openSet.Count; i++)
        {
            if (openSet[i].fCost < node.fCost || openSet[i].fCost == node.fCost)       //finds node with lowest movement cost
            {
                if (openSet[i].hCost < node.hCost)
                    node = openSet[i];
            }
        }

        openSet.Remove(node);               //deletes checked node from list of available nodes
        closedSet.Add(node);                //adds it to the list of checked nodes

        if (node == targetNode)             //builds path of nodes
        {
            RetracePath(startNode, targetNode);
            return;
        }

        foreach (Node neighbour in grid.GetNeighbours(node))
        {
            if (!neighbour.walkable || closedSet.Contains(neighbour))   //skips unwalkable and checked nodes
            {
                continue;
            }

            int newCostToNeighbour = node.gCost + GetDistance(node, neighbour);
            if (newCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour))       // if new neighbouring node's movement cost is cheaper than current neighbouring node's cost
            {                                                                               // or current neighbouring node isn't in available node list
                neighbour.gCost = newCostToNeighbour;                                       // distance between current neighbouring node and starting point = distance between new neighbouring node and starting point
                neighbour.hCost = GetDistance(neighbour, targetNode);                       // distance between current neighbouring node and ending point = distance between new neighbouring node and ending point
                neighbour.parent = node;

                if (!openSet.Contains(neighbour))                                           //add's current neighbouring node to the list of available node
                {
                    openSet.Add(neighbour);
                }
            }
        }
    }
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

    grid.path = path;
}

int GetDistance(Node nodeA, Node nodeB)
{
    int dstX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
    int dstY = Mathf.Abs(nodeA.gridY - nodeB.gridY);

    if (dstX > dstY)                                    //increases movement cost between diagonal nodes to 14 [~sqrt(10^2 + 10^2)]; Horizontal and Vertical nodes cost 10
    {
        return 14 * dstY + 10 * (dstX - dstY);
    }
    return 14 * dstX + 10 * (dstY - dstX);
}
}
*/
