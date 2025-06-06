using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//[System.Serializable]
//public class NamedAction
//{
//    public string Name;
//    public BaseActionEx Action;
//}

public enum EActionType
{
    IDLE,
    MOVE,
    ATTACK,
    ABILITY1,
    ABILITY2,
    ABILITY3,
    ABILITY4,
    DIE,
    MAX
}

[CreateAssetMenu(fileName = "ActionSet", menuName = "Scriptable Objects/ActionSet")]
public sealed class ActionSet : ScriptableObject
{
    [SerializeField] private BaseAction IDLE;
    [SerializeField] private BaseAction MOVE;
    [SerializeField] private BaseAction ATTACK;

    [SerializeField] private BaseAction ABILITY1;
    [SerializeField] private BaseAction ABILITY2;
    [SerializeField] private BaseAction ABILITY3;
    [SerializeField] private BaseAction ABILITY4;

    [SerializeField] private BaseAction DIE;

    public BaseAction GetAction(EActionType type)
    {
        switch(type)
        {
            case EActionType.IDLE:
                return IDLE;
            case EActionType.MOVE:
                return MOVE;
            case EActionType.ATTACK:
                return ATTACK;
            case EActionType.ABILITY1:
                return ABILITY1;
            case EActionType.ABILITY2:
                return ABILITY2;
            case EActionType.ABILITY3:
                return ABILITY3;
            case EActionType.ABILITY4:
                return ABILITY4;
            case EActionType.DIE:
                return DIE;
            default:
                return null;
        }
    }
}
