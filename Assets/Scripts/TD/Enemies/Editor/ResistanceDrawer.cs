using UnityEditor;
using UnityEngine;

namespace ua.org.gdg.devfest
{
  [CustomPropertyDrawer(typeof(Resistance))]
  public class ResistancePropertyDrawer : PropertyDrawer 
  {
    private const float SPACE = 5;

    public override void OnGUI(Rect rect, SerializedProperty property, GUIContent label) 
    {
      int indent = EditorGUI.indentLevel;
      EditorGUI.indentLevel = 0;

      EditorGUI.BeginChangeCheck();
      
      var firstLineRect = new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight);
      DrawMainProperties(firstLineRect, property);

      EditorGUI.indentLevel = indent;
      
      if(EditorGUI.EndChangeCheck())
      {
        property.serializedObject.ApplyModifiedProperties();
      }
    }

    private void DrawMainProperties(Rect rect, SerializedProperty resistance)
    {
      rect.width = (rect.width - 2 * SPACE) / 2; 
      DrawProperty(rect, resistance.FindPropertyRelative("Type"));
      rect.x += rect.width + SPACE;
      DrawProperty(rect, resistance.FindPropertyRelative("Amount"));
    }

    private void DrawProperty(Rect rect, SerializedProperty property)
    {
      EditorGUI.PropertyField(rect, property, GUIContent.none);
    }
  }
}
