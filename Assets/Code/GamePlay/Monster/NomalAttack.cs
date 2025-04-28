using UnityEngine;

public class NomalAttack : MonoBehaviour, IAction
{
    public float attackDelay = 3f;
    public float baseAttackDelay = 3f;

    void Start()
    {
        
    }

    void Update()
    {
        attackDelay -= Time.deltaTime;
    }

    public void Action()
    {
        Debug.Log("Noral Attack");
    }
}
