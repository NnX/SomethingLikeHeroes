public class PathNode
{
    private GridSystem<PathNode> _grid;
    public int X { get; }
    public int Y { get; }
    public bool isWalkable = true;
    public int gCost;
    public int hCost;
    public int fCost;

    public PathNode cameFromNode;
    public PathNode(GridSystem<PathNode> grid, int x, int y)
    {
        _grid = grid;
        X = x;
        Y = y;
    }

    public override string ToString()
    {
        return X + "," + Y;
    }
    
    public void CalculateFCost() {
        fCost = gCost + hCost;
    }
    public void SetIsWalkable(bool isWalkable) {
        this.isWalkable = isWalkable;
        _grid.SetValue(_grid.GetWorldPositionWithCenterOffset(X, Y), -1);
        _grid.TriggerGridObjectChanged(X,  Y);
    }
    
}
