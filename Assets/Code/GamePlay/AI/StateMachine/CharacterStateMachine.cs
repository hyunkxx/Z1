using UnityEngine;

public class CharacterStateMachine : StateMachine
{
    private void Awake()
    {
        Initialize();
    }

    public override void Initialize()
    {
        base.Initialize();

        LinkedSMB<CharacterStateMachine>.Initialize(animator, this);
    }

    void Start()
    {
        StartCoroutine(FindTarget());
    }

    void Update()
    {

    }
}
