using UnityEngine;

public interface IAction
{
    public void Action();
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
        attackType.Action();
    }

    public void Death()
    {

    }
}