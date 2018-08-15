using UnityEditor;
using UnityEngine;

namespace ua.org.gdg.devfest
{
  [CustomPropertyDrawer(typeof(Resistances))]
  public class ResistancesListDrawer : PropertyDrawer
  {
    private const float DELETE_BUTTON_WIDTH = 20;
    
    public override void OnGUI(Rect rect, SerializedProperty property, GUIContent label)
    {
      EditorGUI.BeginProperty(rect, label, property);
      
      // to make insides of foldout clickable
      rect.height = EditorGUIUtility.singleLineHeight;
      
      property.isExpanded = EditorGUI.Foldout(rect, property.isExpanded, label);
      
      if(!property.isExpanded) return;
      
      rect = GetListElementsRect(rect);

      EditorGUI.BeginChangeCheck();

      var list = property.FindPropertyRelative("ResistancesList");
      DrawResistancesList(rect, list);

      //adjust AddButton positions
      rect.y += list.arraySize * EditorGUIUtility.singleLineHeight;
      rect.width += DELETE_BUTTON_WIDTH;
      
      if (GUI.Button(rect, "Add"))
      {
        list.arraySize++;
      }

      if(EditorGUI.EndChangeCheck())
      {
        property.serializedObject.ApplyModifiedProperties();
      }
      
      EditorGUI.EndProperty();
    }

    private void DrawResistancesList(Rect rect, SerializedProperty list)
    {
      for (int i = 0; i < list.arraySize; i++)
      {
        SerializedProperty resistance = list.GetArrayElementAtIndex(i);

        int indent = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 0;
        
        EditorGUI.PropertyField(rect, resistance);
        if (GUI.Button(GetDeleteButtonRect(rect), "X"))
        {
          list.DeleteArrayElementAtIndex(i);
        }
        
        rect.y += EditorGUIUtility.singleLineHeight;
        
        EditorGUI.indentLevel = indent;
      }
    }

    private Rect GetListElementsRect(Rect position)
    {
      return new Rect(position.x, position.y + EditorGUIUtility.singleLineHeight, 
        position.width - DELETE_BUTTON_WIDTH, EditorGUIUtility.singleLineHeight);
    }

    private Rect GetDeleteButtonRect(Rect position)
    {
      return new Rect(position.x + position.width, position.y, 
        DELETE_BUTTON_WIDTH, EditorGUIUtility.singleLineHeight);
    }
    
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
      var list = property.FindPropertyRelative("ResistancesList");
      
      if (property.isExpanded)
      {
        //line for each item of the list + line for AddButton + line for Header
        return (list.arraySize + 2) * EditorGUIUtility.singleLineHeight;
      }

      return EditorGUIUtility.singleLineHeight;
    }
  }
}