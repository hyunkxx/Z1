using System.Collections.Generic;
using UnityEngine;

public class DefencePlayerController : PlayerController
{
    public GameObject Skill_Prefab;

    [SerializeField, ShowIf("m_abilityTarget", EAbilityTarget.Multi)]
    private int m_targetCount = 1;

    protected override void Start()
    {
        base.Awake();
    }
    protected override void Update()
    {

    }

    public void ActioveSkill_0()
    {
        IReadOnlyList<TargetElement> targets = character.TargetingComponent.GetTargetList();

        for (int i = 0; i < m_targetCount; ++i)
        {
            if (targets.Count <= i)
                break;

            GameObject obj = Instantiate(Skill_Prefab);
            Ability ability = obj.GetComponent<Ability>();
            ability.Activate(character.gameObject, targets[i].target);
        }
    }

}