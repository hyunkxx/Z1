using UnityEngine;

public class Skill : MonoBehaviour, IAction
{
    public float attackDelay = 10f;
    public float baseAttackDelay = 10f;

    public virtual void Action() 
    {
        Debug.Log("Skill Action");
        // 데미지 처리
    }
}
