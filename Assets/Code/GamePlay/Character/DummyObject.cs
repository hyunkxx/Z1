using UnityEngine;


public class DummyObject : MonoBehaviour
{
    private Animator animator;
    private Damageable damageable;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        damageable = GetComponent<Damageable>();

        damageable.OnDamageTaken += TakeDamage;
    }
    private void OnDestroy()
    {
        if (damageable)
        {
            damageable.OnDamageTaken -= TakeDamage;
        }
    }

    private void TakeDamage(DamageEvent info)
    {
        Debug.Log("Dummy Hit");
        animator.SetTrigger("Hit");
    }
}
