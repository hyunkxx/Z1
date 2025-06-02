using System;
using UnityEngine;


[AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
public class ShowIfAttribute : PropertyAttribute
{
    public string ConditionFieldName;
    public object CompareValue;

    public ShowIfAttribute(string conditionFieldName, object compareValue)
    {
        ConditionFieldName = conditionFieldName;
        CompareValue = compareValue;
    }
}