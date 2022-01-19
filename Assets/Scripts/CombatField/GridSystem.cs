using System;
using TMPro;
using Unity.Collections;
using UnityEngine;

public class GridSystem<TGridObject> {
    public event EventHandler<OnGridObjectChangedEventArgs> OnGridObjectChanged;
    public class OnGridObjectChangedEventArgs : EventArgs {
        public int x;
        public int y;
    }
    
    private int _width;
    private int _height;
    private float _cellSize;
    private int[,] _gridArray;
    private TextMeshPro[,] debugTextArray;
    private Vector3 _originPosition;
    private TGridObject[,] _gridSystemArray;
    private NativeArray<bool> _array;

    public GridSystem(
        int width, 
        int height,
        float cellSize,
        Vector3 originPosition,
        Func<GridSystem<TGridObject>, int, int, TGridObject> createGridObject)
    {
        _width = width;
        _height = height;
        _cellSize = cellSize;
        _gridArray = new int[width, height];
        _originPosition = originPosition;
        debugTextArray = new TextMeshPro[_width, _height];
        _gridSystemArray = new TGridObject[width, height];

        for (int i = 0; i < _gridArray.GetLength(0); i++)
        {
            for (int j = 0; j < _gridArray.GetLength(1); j++)
            {
                _gridSystemArray[i, j] = createGridObject( this, i, j);
                debugTextArray[i,j] = UtilsGrid.CreateWorldText(_gridSystemArray[i, j]?.ToString(), Color.white, null, GetWorldPositionWithCenterOffset(i, j), 3);
                Debug.DrawLine(GetWorldPosition(i, j), GetWorldPosition(i, j + 1), Color.white, 100f);
                Debug.DrawLine(GetWorldPosition(i, j), GetWorldPosition(i + 1, j), Color.white, 100f);
            }
        }

        Debug.DrawLine(GetWorldPosition(0, _height), GetWorldPosition(_width, _height), Color.white, 100f);
        Debug.DrawLine(GetWorldPosition(_width, 0), GetWorldPosition(_width, _height), Color.white, 100f);
        OnGridObjectChanged += (object sender, OnGridObjectChangedEventArgs eventArgs) => {
            debugTextArray[eventArgs.x, eventArgs.y].text = _gridSystemArray[eventArgs.x, eventArgs.y]?.ToString();
        };
    }


    private Vector3 GetWorldPosition(int x, int y)
    {
        return new Vector3(x, y) * _cellSize + _originPosition;
    }

    public Vector3 GetWorldPositionWithCenterOffset(int x, int y)
    {
        return ((new Vector3(x, y) * _cellSize) + _originPosition) + new Vector3(_cellSize, _cellSize) * 0.5f;
    }

    private void SetValue(int x, int y, int value)
    {
        if (x >= 0 && y >= 0 && x < _width && y < _height)
        {
            _gridArray[x, y] = value;
            debugTextArray[x, y].text = _gridArray[x, y].ToString();
        }
    }

    public void GetXY(Vector3 worldPosition, out int x, out int y)
    {
        x = Mathf.FloorToInt((worldPosition - _originPosition).x / _cellSize);
        y = Mathf.FloorToInt((worldPosition - _originPosition).y / _cellSize);
    }
    
    public void SetValue(Vector3 worldPosition, int value)
    {
        int x, y;
        GetXY(worldPosition, out x, out y);
        SetValue(x, y, value);
    }

    private string GetValue(int x, int y)
    {
        if (x >= 0 && y >= 0 && x < _width && y < _height)
        {
            return debugTextArray[x, y].text;
        }
        return "empty";
    } 
    
    public string GetValue(Vector3 worldPosition)
    {
        int x, y;
        GetXY(worldPosition, out x, out y);
        return GetValue(x, y);
    }
    
    
    public TGridObject GetGridObject(int x, int y)
    {
        if (x >= 0 && y >= 0 && x < _width && y < _height) {
            return _gridSystemArray[x, y];
        }

        return default;
    }
    public int GetWidth() {
        return _width;
    }

    public int GetHeight() {
        return _height;
    }
    
    public void TriggerGridObjectChanged(int x, int y) {
        if (OnGridObjectChanged != null)
        {
            OnGridObjectChanged(this, new OnGridObjectChangedEventArgs { x = x, y = y });
            debugTextArray[x,y].text = "-1";
        }
    }
    
    public float GetCellSize() {
        return _cellSize;
    }
    
}