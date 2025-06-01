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

        LinkedSMB<StateMachine>.Initialize(animator, this);
    }

    void Start()
    {

    }

    void Update()
    {

    }
}
