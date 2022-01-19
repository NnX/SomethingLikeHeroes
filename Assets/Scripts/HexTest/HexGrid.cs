using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;

public class HexGrid : MonoBehaviour
{
    public int width = 6;
    public int height = 6;
    public Color defaultColor = Color.white;
    public Color touchedColor = Color.magenta;
    
    [SerializeField]  private GameObject heroLeft;
    [SerializeField]  private GameObject heroRight;
    [SerializeField]  private GameObject skeletonPrefab;
    [SerializeField]  private List<Transform> unitOrderQueue;
    [SerializeField]  private List<Transform> leftUnitsList;
    [SerializeField]  private List<Transform> rightUnitsList;
    [SerializeField]  private List<int> leftStartCellIndexes;
    [SerializeField]  private List<int> rightStartCellIndexes;
    [SerializeField]  private HexCell cellPrefab;
    [SerializeField]  private TextMeshProUGUI cellLabelPrefab;

    private Canvas _gridCanvas;
    private HexCell[] _cells;
    private HexMesh _hexMesh;
    private int _activeUnitIndex;
    
    private void Awake () {
        InitHexGrid();

        InitHeroes();
        InitUnitStartPositions();

        var skeleton = Instantiate(skeletonPrefab, transform.parent);
        skeleton.transform.DOMove(_cells[145].transform.localPosition, 1f);
    }

    private void InitHexGrid()
    {
        _gridCanvas = GetComponentInChildren<Canvas>();
        _hexMesh = GetComponentInChildren<HexMesh>();
        _cells = new HexCell[height * width];

        for (int z = 0, i = 0; z < height; z++)
        {
            for (var x = 0; x < width; x++)
            {
                CreateCell(x, z, i++);
            }
        }
    }

    private void InitHeroes()
    {
        if (heroLeft.TryGetComponent<Hero>(out var leftHero))
        {
            leftHero.Init();
        }
        if (heroRight.TryGetComponent<Hero>(out var rightHero))
        {
            rightHero.Init();
        }
    }

    private void InitUnitStartPositions()
    {
        for (int i = 0; i < leftUnitsList.Count; i++)
        {
            var unit = leftUnitsList[i];
            unit.DOMove(_cells[leftStartCellIndexes[i]].transform.localPosition, 1f);
        }
        for (int i = 0; i < rightUnitsList.Count; i++)
        {
            var unit = rightUnitsList[i];
            unit.DOMove(_cells[rightStartCellIndexes[i]].transform.localPosition, 1f);
        } 
    }
    
    void Start () {
        _hexMesh.Triangulate(_cells);
    }

    private void CreateCell (int x, int z, int i) {
        Vector3 position;
        position.x = x * (HexMetrics.innerRadius * 2f);
        position.y = 0f;
        position.z = z * (HexMetrics.outerRadius * 1.5f);
        position.x = (x + z * 0.5f - z / 2) * (HexMetrics.innerRadius * 2f);
        
        var cell = _cells[i] = Instantiate(cellPrefab);
        Transform transform1;
        (transform1 = cell.transform).SetParent(transform, false);
        transform1.localPosition = position;
        cell.coordinates = HexCoordinates.FromOffsetCoordinates(x, z);
        cell.color = defaultColor;
        
        var label = Instantiate(cellLabelPrefab, _gridCanvas.transform, false);
        label.rectTransform.anchoredPosition = new Vector2(position.x, position.z);
        label.text = cell.coordinates.ToStringOnSeparateLines();
    }
    
    public void ColorCell (Vector3 position, Color color) {
        position = transform.InverseTransformPoint(position);
        HexCoordinates coordinates = HexCoordinates.FromPosition(position);
        int index = coordinates.X + coordinates.Z * width + coordinates.Z / 2;
        HexCell cell = _cells[index];
        
        // TODO change move order
        var activeUnit = unitOrderQueue[_activeUnitIndex];
        activeUnit.DOMove(cell.transform.localPosition, 1f);
        _activeUnitIndex++;

        if (_activeUnitIndex >= unitOrderQueue.Count)
        {
            _activeUnitIndex = 0;
        }
        
        /*cell.color = color;
        _hexMesh.Triangulate(_cells);*/
    }
}
