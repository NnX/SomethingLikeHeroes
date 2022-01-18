using System.Collections.Generic;
using UnityEngine;

public class PathFinding
{
    
    private const int MOVE_STRAIGHT_COST = 10;
    private const int MOVE_DIAGONAL_COST = 14;
    
    public static PathFinding Instance { get; private set; }
    
    private GridSystem<PathNode> _grid;
    private List<PathNode> openList;
    private List<PathNode> closedList;


    public PathFinding(int width, int height)
    {
        _grid = new GridSystem<PathNode>(width, height, 1f, new Vector3(-4,-4,0), (GridSystem<PathNode> g, int x, int y) => new PathNode(g, x, y));
        //_grid = new GridSystem<PathNode>(width, height, 1f, Vector3.zero, (GridSystem<PathNode> g, int x, int y) => new PathNode(g, x, y));
    }
    
    public GridSystem<PathNode> GetGridSystem() {
        return _grid;
    }


    public List<PathNode> FindPath(int startX, int startY, int endX, int endY) {
        PathNode startNode = _grid.GetGridObject(startX, startY);
        PathNode endNode = _grid.GetGridObject(endX, endY);

        if (startNode == null || endNode == null) {
            // Invalid Path
            return null;
        }

        openList = new List<PathNode> { startNode };
        closedList = new List<PathNode>();

        for (int x = 0; x < _grid.GetWidth(); x++) {
            for (int y = 0; y < _grid.GetHeight(); y++) {
                PathNode pathNode = _grid.GetGridObject(x, y);
                pathNode.gCost = 99999999;
                pathNode.CalculateFCost();
                pathNode.cameFromNode = null;
            }
        }

        startNode.gCost = 0;
        startNode.hCost = CalculateDistanceCost(startNode, endNode);
        startNode.CalculateFCost();
        
        //PathfindingDebugStepVisual.Instance.ClearSnapshots();
        //PathfindingDebugStepVisual.Instance.TakeSnapshot(_grid, startNode, openList, closedList);

        while (openList.Count > 0) {
            PathNode currentNode = GetLowestFCostNode(openList);
            if (currentNode == endNode) {
                // Reached final node
                //PathfindingDebugStepVisual.Instance.TakeSnapshot(_grid, currentNode, openList, closedList);
                //PathfindingDebugStepVisual.Instance.TakeSnapshotFinalPath(_grid, CalculatePath(endNode));
                return CalculatePath(endNode);
            }

            openList.Remove(currentNode);
            closedList.Add(currentNode);

            foreach (PathNode neighbourNode in GetNeighbourList(currentNode)) {
                if (closedList.Contains(neighbourNode))
                {
                    continue;
                }
                if (!neighbourNode.isWalkable) {
                    closedList.Add(neighbourNode);
                    continue;
                }

                int tentativeGCost = currentNode.gCost + CalculateDistanceCost(currentNode, neighbourNode);
                if (tentativeGCost < neighbourNode.gCost) {
                    neighbourNode.cameFromNode = currentNode;
                    neighbourNode.gCost = tentativeGCost;
                    neighbourNode.hCost = CalculateDistanceCost(neighbourNode, endNode);
                    neighbourNode.CalculateFCost();

                    if (!openList.Contains(neighbourNode)) {
                        openList.Add(neighbourNode);
                    }
                }
                //PathfindingDebugStepVisual.Instance.TakeSnapshot(_grid, currentNode, openList, closedList);
            }
        }

        // Out of nodes on the openList
        return null;
    }
    
    /*H cost*/
    private int CalculateDistanceCost(PathNode a, PathNode b) {
        int xDistance = Mathf.Abs(a.X - b.X);
        int yDistance = Mathf.Abs(a.Y - b.Y);
        int remaining = Mathf.Abs(xDistance - yDistance);
        return MOVE_DIAGONAL_COST * Mathf.Min(xDistance, yDistance) + MOVE_STRAIGHT_COST * remaining;
    }
    
    private List<PathNode> CalculatePath(PathNode endNode) {
        List<PathNode> path = new List<PathNode>();
        path.Add(endNode);
        PathNode currentNode = endNode;
        while (currentNode.cameFromNode != null) {
            path.Add(currentNode.cameFromNode);
            currentNode = currentNode.cameFromNode;
        }
        path.Reverse();
        return path;
    }
    
    private PathNode GetLowestFCostNode(List<PathNode> pathNodeList) {
        PathNode lowestFCostNode = pathNodeList[0];
        for (int i = 1; i < pathNodeList.Count; i++) {
            if (pathNodeList[i].fCost < lowestFCostNode.fCost) {
                lowestFCostNode = pathNodeList[i];
            }
        }
        return lowestFCostNode;
    }
    
    
    private List<PathNode> GetNeighbourList(PathNode currentNode) {
        List<PathNode> neighbourList = new List<PathNode>();

        if (currentNode.X - 1 >= 0) {
            // Left
            neighbourList.Add(GetNode(currentNode.X - 1, currentNode.Y));
            // Left Down
            if (currentNode.Y - 1 >= 0) neighbourList.Add(GetNode(currentNode.X - 1, currentNode.Y - 1));
            // Left Up
            if (currentNode.Y + 1 < _grid.GetHeight()) neighbourList.Add(GetNode(currentNode.X - 1, currentNode.Y + 1));
        }
        if (currentNode.X + 1 < _grid.GetWidth()) {
            // Right
            neighbourList.Add(GetNode(currentNode.X + 1, currentNode.Y));
            // Right Down
            if (currentNode.Y - 1 >= 0) neighbourList.Add(GetNode(currentNode.X + 1, currentNode.Y - 1));
            // Right Up
            if (currentNode.Y + 1 < _grid.GetHeight()) neighbourList.Add(GetNode(currentNode.X + 1, currentNode.Y + 1));
        }
        // Down
        if (currentNode.Y - 1 >= 0) neighbourList.Add(GetNode(currentNode.X, currentNode.Y - 1));
        // Up
        if (currentNode.Y + 1 < _grid.GetHeight()) neighbourList.Add(GetNode(currentNode.X, currentNode.Y + 1));

        return neighbourList;
    }
    public PathNode GetNode(int x, int y) {
        return _grid.GetGridObject(x, y);
    }

    /*public List<Vector3> FindPath(Vector3 startWorldPosition, Vector3 endWorldPosition) {
        _grid.GetXY(startWorldPosition, out int startX, out int startY);
        _grid.GetXY(endWorldPosition, out int endX, out int endY);

        List<PathNode> path = FindPath(startX, startY, endX, endY);
        if (path == null) {
            return null;
        } else {
            List<Vector3> vectorPath = new List<Vector3>();
            foreach (PathNode pathNode in path) {
                vectorPath.Add(new Vector3(pathNode.X, pathNode.Y) * _grid.GetCellSize() + Vector3.one * _grid.GetCellSize() * .5f);
            }
            return vectorPath;
        }
    }*/
}