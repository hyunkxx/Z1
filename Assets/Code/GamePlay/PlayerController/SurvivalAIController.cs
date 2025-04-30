using UnityEngine;

public class SurvivalAIController : MonoBehaviour
{
    public GameObject target;
    [SerializeField] MovementComponent movement;

    private void Awake()
    {
    }

    private void Update()
    {
        MoveToPlayer();
    }

    void MoveToPlayer()
    {
        if (Vector2.Distance(target.transform.position, transform.position) > 1f)
            movement.MoveToDirection((target.transform.position - transform.position).normalized);
    }
}
