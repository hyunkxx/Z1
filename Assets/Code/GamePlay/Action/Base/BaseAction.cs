using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;


[System.Serializable]
public abstract class BaseAction: ScriptableObject
{
    [SerializeField] protected ConditionSet conditionSet;

    [SerializeField] protected float m_coolDown = 0f;
    public float CoolDown => m_coolDown;

    /* Register Action */
    //public void OnEnable()
    //{
    //    Debug.Log("Enable");
    //}
    public bool CheckConditions(ICharacterQueryable InQueryable)
    {
        return conditionSet.Check(InQueryable);
    }

    public bool TryExecute(ICharacterQueryable InQueryable)
    {
        if (!conditionSet.Check(InQueryable))
            return false;

        return InternalExecuteAction(InQueryable);
    }

    protected abstract bool InternalExecuteAction(ICharacterQueryable InQueryable);
}