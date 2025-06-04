using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public sealed class ActionComponent : MonoBehaviour
{
    [SerializeField]
    private ActionSet m_actionSet;

    [NonSerialized]
    private Character m_character;

    Dictionary<EActionType, float> remainingTimes = new();

    public bool IsCooldownRunning(EActionType actionType)
    {
        return !remainingTimes.ContainsKey(actionType);
    }

    public void Awake()
    {
        m_character = GetComponent<Character>();
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
        if (action.TryExecute(m_character))
        {
            remainingTimes.Add(actionType, action.CoolDown);
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

            Debug.Log($"{key} : {remainingTimes[key]}");
            if (remainingTimes[key] <= 0f)
                remainingTimes.Remove(key);
        }
    }
}