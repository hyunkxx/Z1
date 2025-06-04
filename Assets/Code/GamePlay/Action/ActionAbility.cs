using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum EAbilityTarget
{
    Self,
    Single,
    Multi
}

[CreateAssetMenu(fileName = "ActionAbility", menuName = "Scriptable Objects/Actions/ActionAbility")]
public class ActionAbility : BaseAction
{
    [SerializeField]
    private GameObject m_abilityPrefab;

    [SerializeField]
    EAbilityTarget m_abilityTarget;

    [SerializeField, ShowIf("m_abilityTarget", EAbilityTarget.Multi)]
    private int m_targetCount = 1;

    [SerializeField, ShowIf("m_abilityTarget", EAbilityTarget.Self, true), Range(0.0f, 20.0f)]
    private float m_triggerDistance = 5f;

    [SerializeField]
    protected string m_animationTrigger;

    protected override bool InternalExecuteAction(ICharacterQueryable InQueryable)
    {
        Character character = InQueryable.GetCharacter();
        if (character == null)
            return false;

        if (m_abilityPrefab == null)
            return false;

        bool bActivated = false;
        switch (m_abilityTarget)
        {
            case EAbilityTarget.Self:
                {
                    GameObject obj = Instantiate(m_abilityPrefab);
                    Ability ability = obj.GetComponent<Ability>();
                    ability.Activate(character.gameObject, character.gameObject);
                    bActivated = true;
                    break;
                }
            case EAbilityTarget.Single:
                {
                    IReadOnlyList<TargetElement> targets = character.TargetingComponent.GetTargetList();
                    if (targets.Count < 1)
                        return false;

                    if (Vector2.Distance(targets[0].target.transform.position, character.transform.position) > m_triggerDistance)
                        return false;

                    GameObject obj = Instantiate(m_abilityPrefab);
                    Ability ability = obj.GetComponent<Ability>();
                    ability.Activate(character.gameObject, targets[0].target);
                    bActivated = true;
                    break;
                }
            case EAbilityTarget.Multi:
                {
                    IReadOnlyList<TargetElement> targets = character.TargetingComponent.GetTargetList();
                    if (targets.Count < 1)
                        return false;

                    
                    for (int i = 0; i < m_targetCount; ++i)
                    {
                        if (targets.Count <= i)
                            break;

                        if (Vector2.Distance(targets[i].target.transform.position, character.transform.position) > m_triggerDistance)
                            continue;

                        GameObject obj = Instantiate(m_abilityPrefab);
                        Ability ability = obj.GetComponent<Ability>();
                        ability.Activate(character.gameObject, targets[i].target);
                        bActivated = true;
                    }

                    break;
                }
        }

        if(bActivated)
        {
            if (!string.IsNullOrEmpty(m_animationTrigger))
            {
                character.Animator.SetTrigger(m_animationTrigger);
            }
            
            return true;
        }
        else
        {
            return false;
        }
    }
}
