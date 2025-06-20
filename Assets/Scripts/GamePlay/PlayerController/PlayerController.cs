using System;
using UnityEngine;


public class PlayerController 
    : Z1Behaviour
{
    protected Character character;
    public Action<Character> OnChangedCharacter;

    public Character Character => character;
    public HUDBase HUD { get; protected set; }

    protected bool inputLock = false;
    public void SetInputLock(bool bValue)
    {
        inputLock = bValue;
    }

    protected override void Awake()
    {
        base.Awake();
    }
    protected override void Update()
    {
        base.Update();
    }

    public virtual void ConnectCharacter(Character target)
    {
        if (character == target)
            return;

        character = target;
        OnChangedCharacter?.Invoke(character);
    }
    public virtual void DisconnectCharacter()
    {
        if (!character)
            return;

        character = null;
    }
}