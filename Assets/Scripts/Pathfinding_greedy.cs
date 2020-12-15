using UnityEngine;
using System.Collections.Generic;

public class Pathfinding_greedy : PathfindingAlgorithm
{
    public override List<PathfindingNode> FindPath(LevelGenerator tempLevelNodes, PathfindingNode tempStartNode,
        PathfindingNode tempTargetNode)
    {
        StartPathfindingSteps(tempLevelNodes, tempStartNode, tempTargetNode);
        while (openSet.Count > 0)
        {
            PathfindingNode node = openSet[0];
            for (int i = 1; i < openSet.Count; i++)
            {
                float distNode = GetDistance(node, targetNode);
                float newDist = GetDistance(openSet[i], targetNode);
                if (newDist <= distNode)
                {
                    // favor y movement (down movement)
                    if (openSet[i].gridY < node.gridY)
                        node = openSet[i];
                }
            }
            openSet.Remove(node);
            closedSet.Add(node);
            if (node == targetNode)
            {
                return RetracePath(startNode, targetNode);
            }
            foreach (PathfindingNode neighbour in GetNeighbours(node))
            {
                if (!neighbour.walkable || closedSet.Contains(neighbour))
                {
                    continue;
                }
                if (!openSet.Contains(neighbour))
                {
                    neighbour.parent = node;
                    if (!openSet.Contains(neighbour))
                        openSet.Add(neighbour);
                }
            }
        }
        return null;
    }
    

    public override bool PathfindingStep()
    {
//        if (maxCycles < cycleNum)
//            return true;
//        if (!pathfindingActive || pathfindingDone)
//            return true;

        if (openSet.Count <= 0)
            return true;

        PathfindingNode node = openSet[0];
        for (int i = 1; i < openSet.Count; i++)
        {
            float distNode = GetDistance(node, targetNode);
            float newDist = GetDistance(openSet[i], targetNode);
            if (newDist <= distNode)
            {
                // favor y movement (down movement)
                if (openSet[i].gridY < node.gridY)
                    node = openSet[i];
            }
        }
        openSet.Remove(node);
        closedSet.Add(node);
        if (node == targetNode)
        {
            pathfindingDone = true;
            return true;
        }
        for (int index = 0; index < GetNeighbours(node).Count; index++)
        {
            PathfindingNode neighbour = GetNeighbours(node)[index];
            if (!neighbour.walkable || closedSet.Contains(neighbour))
            {
                continue;
            }
            if (!openSet.Contains(neighbour))
            {
                neighbour.parent = node;
                if (!openSet.Contains(neighbour))
                    openSet.Add(neighbour);
            }
        }
        return false;
    }
    
}