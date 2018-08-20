using UnityEditor;
using UnityEngine;

namespace ua.org.gdg.devfest
{
  [CustomEditor(typeof(InspectorFunctionCaller))]
  public class FunctionCallerDrawer : Editor
  {
    private SerializedProperty _response;
    
    void OnEnable()
    {
      _response = serializedObject.FindProperty("ButtonPressedEvent");
    }
    
    public override void OnInspectorGUI()
    {
      serializedObject.Update();
      InspectorFunctionCaller caller = (InspectorFunctionCaller) target;
      EditorGUILayout.PropertyField(_response);

      if (GUILayout.Button("Call Function"))
      {
        caller.OnButtonPressed();
      }
      
      serializedObject.ApplyModifiedProperties();
    }
  }
}