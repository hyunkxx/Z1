using UnityEngine;

public interface IAction
{ 
    public void ExcuteAction();
}

// ----------------------------------------------------------------

interface IMonster
{
    public void Spawn();
    public void Death();
}

public class AICharacter : IMonster
{
    IAction AttackType;
    public float hp = 100;

    public void Spawn()
    {

    }

    public void Action(IAction attackType)
    {
        attackType.ExcuteAction();
    }

    public void Death()
    {

    }
}