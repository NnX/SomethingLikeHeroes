using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(HexCoordinates))]
public class HexCoordinatesDrawer : PropertyDrawer
{
    public override void OnGUI(
        Rect position, SerializedProperty property, GUIContent label
    )
    {
        position = EditorGUI.PrefixLabel(position, label);
        HexCoordinates coordinates = new HexCoordinates(
            property.FindPropertyRelative("x").intValue,
            property.FindPropertyRelative("z").intValue
        );

        GUI.Label(position, coordinates.ToString());
    }
}