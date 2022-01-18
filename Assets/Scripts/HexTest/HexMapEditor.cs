using UnityEngine;
using UnityEngine.EventSystems;

public class HexMapEditor : MonoBehaviour
{
    public Color[] colors;

    public HexGrid hexGrid;

    private Color _activeColor;

    void Awake () {
        SelectColor(0);
    }

    void Update () {
        if (Input.GetMouseButtonUp(0) && !EventSystem.current.IsPointerOverGameObject()) {
            HandleInput();
        }
    }

    void HandleInput () {
        if (Camera.main is { })
        {
            Ray inputRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(inputRay, out var hit)) {
                Debug.Log("HandleInput");
                hexGrid.ColorCell(hit.point, _activeColor);
            }
        }
    }

    public void SelectColor (int index) {
        _activeColor = colors[index];
    }
}
