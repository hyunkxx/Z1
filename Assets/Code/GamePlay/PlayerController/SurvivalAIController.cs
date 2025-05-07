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
         movement.MoveToLocation(target.transform.position);
    }
}
