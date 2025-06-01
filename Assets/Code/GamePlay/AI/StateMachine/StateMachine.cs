using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
    public IAction ActionType;
    public Animator animator;
    public MovementComponent movement;

    [HideInInspector] public GameObject target;
    [HideInInspector] public AttackAction AttackType;
    public AttackAction nomalAttack;
    public AttackAction[] skill;

    private void Awake()
    {

    }

    public virtual void Initialize()
    {
        movement = GetComponent<MovementComponent>();
        animator = transform.GetComponentInChildren<Animator>();
        nomalAttack = GetComponent<AttackAction>();
        skill = GetComponents<AttackAction>();
    }

    public void TransMove()
    {
        animator.SetBool("isMove", true);
    }

    public bool TransAttack(AttackAction _ationType, string _paramName, ref float _delay, float _baseDelay, float _range)
    {
        if (target == null) return false;
        if (_delay > 0f) return false;
        if (Vector2.Distance(target.transform.position, transform.position) > _range) return false;
    
        AttackType = _ationType;
        _delay = _baseDelay;
        animator.SetTrigger(_paramName);
        animator.SetBool("isMove", false);
        return true;
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
                overrides[i] = new KeyValuePair<AnimationClip, AnimationClip>(overrides[i].Key, AttackType.clip);
                break;
            }
        }

        overrideController.ApplyOverrides(overrides);
        animator.runtimeAnimatorController = overrideController;
    }

    public void Action()
    {
        if (AttackType == null) return;
    
        ActionType.ExcuteAction();
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

    private float minY = -5f;
    private float maxY = 6f;
    private float minScale = 1.0f;
    private float maxScale = 0.5f;

    private IEnumerator YSorting()
    {
        while (gameObject.activeSelf)
        {
            gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>().sortingOrder = (int)(transform.position.y * 100);

            float t = Mathf.InverseLerp(maxY, minY, transform.position.y); // Y가 클수록 t는 0
            float scale = Mathf.Lerp(maxScale, minScale, t);

            transform.localScale = new Vector3(scale, scale, 1f);
            Debug.Log(scale);

            yield return new WaitForSeconds(0.25f);
        }
    }
}
