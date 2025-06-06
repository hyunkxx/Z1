using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public sealed class ActionComponent 
    : MonoBehaviour
    , ICharacterQueryable
{
    [SerializeField]
    private ActionSet m_actionSet;

    public Character OwnerCharacter { get; private set; }
    public BaseAction CurrentAction { get; private set; }

    private Dictionary<EActionType, float> remainingTimes = new();
    public bool IsPlayerControlled { get; private set; }

    public void Awake()
    {
        OwnerCharacter = GetComponent<Character>();
        IsPlayerControlled = !OwnerCharacter.IsNPC && GameManager.Instance.GameMode.GameType == EGameType.Survival;
    }

    public void Update()
    {
        /* temp */
        if (IsPlayerControlled && IsCooldownRunning(EActionType.ATTACK))
        {
            TryExecute(EActionType.ATTACK);
        }

        /* temp */
        if(Input.GetKeyDown(KeyCode.E))
        {
            TryExecute(EActionType.ABILITY1);
        }

        /* temp */
        if (Input.GetKeyDown(KeyCode.R))
        {
            TryExecute(EActionType.ABILITY2);
        }

        UpdateRemainingTimes();
    }

    public bool IsCurrent(EActionType type)
    {
        return CurrentAction == GetAction(type);
    }

    public bool IsCooldownRunning(EActionType actionType)
    {
        return !remainingTimes.ContainsKey(actionType);
    }

    public bool TryExecute(EActionType actionType)
    {
        if (!IsCooldownRunning(actionType))
            return false;

        BaseAction action = m_actionSet.GetAction(actionType);
        if (action.TryExecute(OwnerCharacter))
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

    public Character GetCharacter()
    {
        return OwnerCharacter;
    }
}