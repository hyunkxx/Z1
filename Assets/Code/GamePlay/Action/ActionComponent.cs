using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public sealed class ActionComponent : MonoBehaviour
{
    [SerializeField]
    private ActionSet m_actionSet;

    public Character Character { get; private set; }
    public BaseAction CurrentAction { get; private set; }

    Dictionary<EActionType, float> remainingTimes = new();

    public bool IsCurrent(EActionType type)
    {
        return CurrentAction == GetAction(type);
    }

    public bool IsCooldownRunning(EActionType actionType)
    {
        return !remainingTimes.ContainsKey(actionType);
    }

    public void Awake()
    {
        Character = GetComponent<Character>();
    }

    public void Update()
    {
        if(Input.GetKeyDown(KeyCode.U))
        {
            TryExecute(EActionType.ATTACK);
        }

        UpdateRemainingTimes();
    }

    public bool TryExecute(EActionType actionType)
    {
        if (!IsCooldownRunning(actionType))
            return false;

        BaseAction action = m_actionSet.GetAction(actionType);
        if (action.TryExecute(Character))
        {
            remainingTimes.Add(actionType, action.CoolDown);
            CurrentAction = GetAction(actionType);
            return true;
        }
        else
        {
            return false;
        }
    }

    public BaseAction GetAction(EActionType actionType)
    {
        return m_actionSet.GetAction(actionType);
    }

    private void UpdateRemainingTimes()
    {
        var keys = remainingTimes.Keys.ToList();
        foreach(var key in keys)
        {
            remainingTimes[key] -= Time.deltaTime;

            if (remainingTimes[key] <= 0f)
                remainingTimes.Remove(key);
        }
    }
}