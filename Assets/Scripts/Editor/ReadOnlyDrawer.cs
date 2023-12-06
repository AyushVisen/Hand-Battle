using UnityEditor;
using UnityEngine;

namespace Ayush
{
    [CustomPropertyDrawer(typeof(ReadOnlyAttribute))]
    public class ReadOnlyDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent content)
        {
            return EditorGUI.GetPropertyHeight(property, content, true);
        }

        public override void OnGUI(Rect rect, SerializedProperty property, GUIContent content)
        {
            GUI.enabled = false;
            EditorGUI.PropertyField(rect, property, content, true);
            GUI.enabled = true;
        }
    }
}