using UnityEngine;

public interface IAction
{ 
    public void ExcuteAction();
}

// ----------------------------------------------------------------

interface IMonster
{
    public void Spawn();
    public void Move();
    public void Death();
}

public class AICharacter : MonoBehaviour, IMonster
{
    public MovementComponent movement;

    public void Spawn()
    {

    }

    public void Action(IAction attackType)
    {
        attackType.ExcuteAction();
    }

    public void Move()
    {

    }

    public void Death()
    {

    }
}