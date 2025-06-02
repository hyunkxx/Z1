using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;


public sealed class ActionComponent : MonoBehaviour
{
    [SerializeField]
    private Action_Skill Attack;

    [SerializeField]
    private List<Action_Skill> SkillSet;

    private Queue<Action_Skill> ActionSequence = new ();
    private Action_Skill RunningSkill;

    /**
     * �⺻ ���� �׽� �ߵ�
     * ��ų�� ���� ������ ���� ���� ������ ��� ������ ��ų�� ���������� ������
     */
    public void Update()
    {
        if (Attack != null)
        {
            Attack.TryExecute();
        }

        EnqueueReadySkills();
        TryExecuteNextSkill();
    }

    private void EnqueueReadySkills()
    {
        foreach (var skill in SkillSet)
        {
            if (!ActionSequence.Contains(skill) && !skill.IsRunning && skill.CanExcute())
            {
                ActionSequence.Enqueue(skill);
            }
        }
    }

    private void TryExecuteNextSkill()
    {
        if (RunningSkill != null && RunningSkill.IsRunning || ActionSequence.Count == 0)
            return;

        var nextSkill = ActionSequence.Dequeue();
        RunningSkill = nextSkill;
        
        nextSkill.TryExecute();
    }
}


//public class ConditionBase
//{
//    bool bInverseCondition = false;

//    public bool IsReversed() { return bInverseCondition; }
//    public bool Check(ICharacterQueryable InQueryable) { return InternalCheck(InQueryable) != bInverseCondition; }
//    protected virtual bool InternalCheck(ICharacterQueryable InQueryable) { return true; }
//}

//public class ConditionSet
//{
//    List<ConditionBase> conditions;

//    public virtual bool Check(ICharacterQueryable InQueryable)
//    {
//        foreach(ConditionBase entry in conditions)
//        {
//            if(!entry.Check(InQueryable))
//            {
//                return false;
//            }
//        }

//        return true;
//    }
//}

//public class Condition_CoolTime
//{
//}