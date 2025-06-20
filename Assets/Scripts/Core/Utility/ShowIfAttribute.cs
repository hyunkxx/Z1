using System;
using UnityEngine;


[AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
public class ShowIfAttribute : PropertyAttribute
{
    public string ConditionFieldName;
    public object CompareValue;
    public bool Invert;

    public ShowIfAttribute(string conditionFieldName, object compareValue, bool invert = false)
    {
        ConditionFieldName = conditionFieldName;
        CompareValue = compareValue;
        Invert = invert;
    }
}