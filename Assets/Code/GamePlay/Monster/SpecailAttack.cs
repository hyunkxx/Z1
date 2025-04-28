using UnityEngine;

public class SpecialAttack : MonoBehaviour, IAction
{
    public float attackDelay = 10f;
    public float baseAttackDelay = 10f;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        attackDelay -= Time.deltaTime;
    }

    public void Action()
    {
        Debug.Log("Specail Attack");
    }
}
