using UnityEngine;

public class MonsterStateMachine : MonoBehaviour
{
    public IAction ActionType;
    public AICharacter monster;
    public Animator animator;
    public GameObject target;

    public AttackAction nomalAttack;
    public AttackAction specailAttack;
    
    private void Awake()
    {
        LinkedSMB<MonsterStateMachine>.Initialize(animator, this);
        monster = new AICharacter();
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

    public bool TransAttack(string _paramName, ref float _delay, float _baseDelay)
    {
        if (target == null) return false;
        if (_delay > 0f) return false;

        if (Vector2.Distance(target.transform.position, transform.position) < 1f)
        {
            _delay = _baseDelay;
            animator.SetBool(_paramName, true);
            return true;
        }

        return false;
    }

    public void CheckAnimationEnd(string _anim, string _bool)
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName(_anim) && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
            animator.SetBool(_bool, false);
    }

    public void Action()
    {
        if (ActionType == null) return;

        monster.Action(ActionType);
    }
}