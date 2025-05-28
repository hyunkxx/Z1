using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterStateMachine : MonoBehaviour
{
    public IAction ActionType;
    public AICharacter monster;
    public Animator animator;
    public GameObject target;

    public AttackAction nomalAttack;
    public AttackAction activeSkill;
    public AttackAction[] skill;
    
    private void Awake()
    {
        LinkedSMB<MonsterStateMachine>.Initialize(animator, this);
    }

    public void Initialize()
    {

    }

    void Start()
    {

    }

    void Update()
    {

    }

    public void TransMove()
    {
        animator.SetBool("isMove", true);
    }

    public bool TransAttack(string _paramName, ref float _delay, float _baseDelay, float _range)
    {
        if (target == null) return false;
        if (_delay > 0f) return false;
        if (Vector2.Distance(target.transform.position, transform.position) > _range) return false;
      
        _delay = _baseDelay;
        animator.SetTrigger(_paramName);
        animator.SetBool("isMove", false);
        return true;
    }

    // ∆–≈œ
    public void SkillPatern()
    {

    }

    public void ChangeStateClip()
    {
        AnimatorOverrideController overrideController = new AnimatorOverrideController(animator.runtimeAnimatorController);

        var overrides = new List<KeyValuePair<AnimationClip, AnimationClip>>();
        overrideController.GetOverrides(overrides);

        string targetClipName = "Skill";
        
        for (int i = 0; i < overrides.Count; i++)
        {
            if (overrides[i].Key.name == targetClipName)
            {
                overrides[i] = new KeyValuePair<AnimationClip, AnimationClip>(overrides[i].Key, activeSkill.clip);
                break;
            }
        }

        overrideController.ApplyOverrides(overrides);
        animator.runtimeAnimatorController = overrideController;
    }

    public void Action()
    {
        if (ActionType == null) return;

        monster.Action(ActionType);
    }

    Coroutine YSortingCoroutine;
    
    private void OnBecameVisible()
    {
        if (YSortingCoroutine != null) return;

        YSortingCoroutine = StartCoroutine(YSorting());
    }
    private void OnBecameInvisible()
    {
        if (YSortingCoroutine == null) return;

        StopCoroutine(YSortingCoroutine);
    }

    private IEnumerator YSorting()
    {
        while(gameObject.activeSelf)
        {
            if (transform.position.y < target.transform.position.y - 0.1f)
            {
                gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>().sortingOrder = 2;
            }
            else
            {
                gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>().sortingOrder = 0;
            }

            yield return new WaitForSeconds(0.25f);
        }
    }
}