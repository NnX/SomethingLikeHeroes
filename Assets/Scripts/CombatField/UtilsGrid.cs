using System;
using TMPro;
using UnityEngine;

public static class UtilsGrid
{
    public static TextMeshPro CreateWorldText(string text, Color color, Transform parent = null, Vector3 localPosition = default,
        int fontSize = 40, int sortingOrder = 0)
    {
        return CreateWorldText(parent, text, localPosition, fontSize, color, sortingOrder);
    }

    private static TextMeshPro CreateWorldText(Transform parent, string text, Vector3 localPosition, int fontSize, Color color, int sortingOrder)
    {
        GameObject gameObject = new GameObject("World_text", typeof(TextMeshPro));
        Transform transform = gameObject.transform;
        transform.SetParent(parent, false);
        transform.localPosition = localPosition;
        TextMeshPro textMesh = gameObject.GetComponent<TextMeshPro>();
        textMesh.alignment = TextAlignmentOptions.Center;
        textMesh.text = text;
        textMesh.fontSize = fontSize;
        textMesh.color = color;
        textMesh.GetComponent<MeshRenderer>().sortingOrder = sortingOrder;
        return textMesh;
    }

    public static Vector3 GetMouseWorldPosition()
    {
        var mousePos = Input.mousePosition;
        mousePos.z = 10;  // select distance = 10 units from the camera
        var vector3 = Vector3.zero;
        if (Camera.main is { })
        {
            vector3 = Camera.main.ScreenToWorldPoint(mousePos);
            
        }
        Debug.Log(vector3);
        return vector3;
    }
    
    // Get Color from Hex string FF00FFAA
    public static Color GetColorFromString(string color) {
        float red = Hex_to_Dec01(color.Substring(0,2));
        float green = Hex_to_Dec01(color.Substring(2,2));
        float blue = Hex_to_Dec01(color.Substring(4,2));
        float alpha = 1f;
        if (color.Length >= 8) {
            // Color string contains alpha
            alpha = Hex_to_Dec01(color.Substring(6,2));
        }
        return new Color(red, green, blue, alpha);
    }
    

        
    // Returns 00-FF, value 0->255
    public static string Dec_to_Hex(int value) {
        return value.ToString("X2");
    }

    // Returns 0-255
    public static int Hex_to_Dec(string hex) {
        return Convert.ToInt32(hex, 16);
    }
        
    // Returns a hex string based on a number between 0->1
    public static string Dec01_to_Hex(float value) {
        return Dec_to_Hex((int)Mathf.Round(value*255f));
    }

    // Returns a float between 0->1
    public static float Hex_to_Dec01(string hex) {
        return Hex_to_Dec(hex)/255f;
    }
}
