using UnityEngine;

public class MonsterStateMachine : MonoBehaviour
{
    public IAction ActionType;
    public AICharacter monster;
    public Animator animator;
    public GameObject target;
    public NomalAttack nomalAttack;
    public SpecialAttack specailAttack;
    
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
        bool isValidPos = false;
        float xPos = 0, yPos = 0;

        while (!isValidPos)
        {
            xPos = Random.Range(-10, 10);
            yPos = Random.Range(-6, 6);
            isValidPos = IsValidSpawnPosition(target.transform.position, new Vector3(xPos, yPos, 0));
        }

        transform.position = target.transform.position + new Vector3(xPos, yPos, 0);
    }

    void Update()
    {

    }

    public bool TransAttack(string _paramName, ref float _delay, float _baseDelay)
    {
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

    bool IsValidSpawnPosition(Vector3 _targetPos, Vector3 _position)
    {
        if (Vector3.Distance(_position, _targetPos) > 9)
            return true;

        return false;
    }
}