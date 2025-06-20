using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;


[Serializable]
public class ConditionSet
{
    [SerializeField] private List<Condition> conditions;

    public virtual bool Check(ICharacterQueryable InQueryable)
    {
        foreach (Condition entry in conditions)
        {
            if (!entry.Check(InQueryable))
            {
                return false;
            }
        }

        return true;
    }
}