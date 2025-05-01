using System;
using UnityEngine;


public class PlayerController 
    : Z1Object
{
    protected Character character;
    public Action<Character> OnChangedCharacter;

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

    public void BindCharacter(Character target)
    {
        if (character == target)
            return;

        character = target;
        OnChangedCharacter?.Invoke(character);
    }
    public void UnBindCharacter()
    {
        if (!character)
            return;

        character = null;
    }
}
