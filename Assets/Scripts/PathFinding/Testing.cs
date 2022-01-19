using System.Collections.Generic;
using UnityEngine;

public class Testing : MonoBehaviour
{
    private GridSystem<PathNode> _gridSystem;
    private PathFinding _pathFinding;
    private void Start()
    {
        _pathFinding = new PathFinding(10, 10);
    }
    
    private void Update() {
        if (Input.GetMouseButtonDown(0)) {
            Vector3 mouseWorldPosition = UtilsGrid.GetMouseWorldPosition();
            _pathFinding.GetGridSystem().GetXY(mouseWorldPosition, out int x, out int y);
            List<PathNode> path = _pathFinding.FindPath(0, 0, x, y);
            if (path != null) {
                for (int i=0; i<path.Count - 1; i++)
                {
                    var node = path[i];
                    var nodeNext = path[i + 1];
                    var start = _pathFinding.GetGridSystem().GetWorldPositionWithCenterOffset(node.X, node.Y);
                    var end  = _pathFinding.GetGridSystem().GetWorldPositionWithCenterOffset(nodeNext.X, nodeNext.Y);
                    Debug.DrawLine(start, end, Color.green, 5f);
                }
            }
        }

        if (Input.GetMouseButtonDown(1)) {
            Vector3 mouseWorldPosition = UtilsGrid.GetMouseWorldPosition();
            _pathFinding.GetGridSystem().GetXY(mouseWorldPosition, out int x, out int y);
            _pathFinding.GetNode(x, y).SetIsWalkable(!_pathFinding.GetNode(x, y).isWalkable);
        }
    }
}
