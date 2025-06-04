using UnityEditor;
using UnityEngine;
using System.Reflection;

[CustomPropertyDrawer(typeof(ShowIfAttribute))]
public class ShowIfPropertyDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        ShowIfAttribute showIf = (ShowIfAttribute)attribute;
        SerializedProperty conditionProp = property.serializedObject.FindProperty(showIf.ConditionFieldName);

        if (conditionProp == null)
        {
            EditorGUI.PropertyField(position, property, label);
            return;
        }

        bool shouldShow = false;

        switch (conditionProp.propertyType)
        {
            case SerializedPropertyType.Boolean:
                shouldShow = conditionProp.boolValue.Equals(showIf.CompareValue);
                if (showIf.Invert) shouldShow = !shouldShow;
                break;
            case SerializedPropertyType.Enum:
                shouldShow = conditionProp.enumValueIndex.Equals((int)showIf.CompareValue);
                if (showIf.Invert) shouldShow = !shouldShow;
                break;
            default:
                Debug.LogWarning($"[ShowIf] Unsupported property type: {conditionProp.propertyType}");
                shouldShow = true;
                break;
        }

        if (shouldShow)
        {
            EditorGUI.PropertyField(position, property, label, true);
        }
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        ShowIfAttribute showIf = (ShowIfAttribute)attribute;
        SerializedProperty conditionProp = property.serializedObject.FindProperty(showIf.ConditionFieldName);

        bool shouldShow = false;

        if (conditionProp != null)
        {
            switch (conditionProp.propertyType)
            {
                case SerializedPropertyType.Boolean:
                    shouldShow = conditionProp.boolValue.Equals(showIf.CompareValue);
                    if (showIf.Invert) shouldShow = !shouldShow;
                    break;
                case SerializedPropertyType.Enum:
                    shouldShow = conditionProp.enumValueIndex.Equals((int)showIf.CompareValue);
                    if (showIf.Invert) shouldShow = !shouldShow;
                    break;
                default:
                    shouldShow = true;
                    break;
            }
        }

        return shouldShow ? EditorGUI.GetPropertyHeight(property, label, true) : 0f;
    }
}