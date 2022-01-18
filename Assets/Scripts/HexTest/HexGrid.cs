using TMPro;
using UnityEngine;
using DG.Tweening;

public class HexGrid : MonoBehaviour
{
    public int width = 6;
    public int height = 6;
    public Color defaultColor = Color.white;
    public Color touchedColor = Color.magenta;
    
    [SerializeField]  private Transform ship;
    [SerializeField]  private HexCell cellPrefab;
    [SerializeField]  private TextMeshProUGUI cellLabelPrefab;

    private Canvas _gridCanvas;
    private HexCell[] _cells;
    private HexMesh _hexMesh;

    private void Awake () {
        _gridCanvas = GetComponentInChildren<Canvas>();
        _hexMesh = GetComponentInChildren<HexMesh>();
        _cells = new HexCell[height * width];

        for (int z = 0, i = 0; z < height; z++) {
            for (var x = 0; x < width; x++) {
                CreateCell(x, z, i++);
            }
        }
    }
    
    void Start () {
        _hexMesh.Triangulate(_cells);
    }

    private void CreateCell (int x, int z, int i) {
        Vector3 position;
        /*position.x = x * 10f; // without offset
        position.y = 0f;
        position.z = z * 10f;*/
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
    
    /*
    void Update () {
        if (Input.GetMouseButton(0)) {
            HandleInput();
        }
    }

    void HandleInput () {
        Ray inputRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(inputRay, out hit)) {
            TouchCell(hit.point);
        }
    }*/
	
    /*public void TouchCell (Vector3 position) {
        position = transform.InverseTransformPoint(position);
        Debug.Log("touched at " + position);
        position = transform.InverseTransformPoint(position);
        HexCoordinates coordinates = HexCoordinates.FromPosition(position);
        int index = coordinates.X + coordinates.Z * width + coordinates.Z / 2;
        HexCell cell = _cells[index];
        cell.color = touchedColor;
        _hexMesh.Triangulate(_cells);
    }*/
    
    public void ColorCell (Vector3 position, Color color) {
        position = transform.InverseTransformPoint(position);
        HexCoordinates coordinates = HexCoordinates.FromPosition(position);
        int index = coordinates.X + coordinates.Z * width + coordinates.Z / 2;
        HexCell cell = _cells[index];

        ship.DOMove(cell.transform.localPosition, 1f);
        cell.color = color;
        _hexMesh.Triangulate(_cells);
    }
}
