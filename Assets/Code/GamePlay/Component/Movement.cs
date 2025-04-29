using UnityEngine;


enum EMovementState
{
    None,
    DirectionalMovement,
    PositionalMovement,
    Aborted,
    Reached
}

public class Movement : MonoBehaviour
{
    [Header("Movement Properties")]
    [SerializeField] private float moveSpeed = 10.0f;
    [SerializeField] private float maxVelocity = 2.0f;

    EMovementState movementState = EMovementState.None;

    private float ReachedOffset = 0.5f;
    private Vector2 moveDirection;
    private Vector2 goalLocation;

    private Rigidbody2D rg2d;

    private void Awake()
    {
        rg2d = GetComponent<Rigidbody2D>();
    }
    private void FixedUpdate()
    {
        switch (movementState)
        {
            case EMovementState.DirectionalMovement:
                ApplyMovementForce();
                break;
            case EMovementState.PositionalMovement:
                if (HasReachedLocation())
                    ResetMovement();
                else
                    ApplyMovementForce();
                break;
            default:
                ResetMovement();
                break;
        }
    }

    public void ResetMovement()
    {
        movementState = EMovementState.None;
        moveDirection = Vector2.zero;
        goalLocation = Vector2.zero;
    }
    public void MoveToDirection(Vector2 direction)
    {
        PerformMovement(direction);
        movementState = EMovementState.DirectionalMovement;
    }
    public void MoveToLocation(Vector2 location)
    {
        Vector2 position = transform.position;
        Vector2 direction = (location - position).normalized;
        goalLocation = location;
        movementState = EMovementState.PositionalMovement;
        PerformMovement(direction);
    }
    public bool HasReachedLocation()
    {
        Vector2 position = transform.position;
        bool Reached = Vector2.SqrMagnitude(goalLocation - position) <= ReachedOffset;

        if (Reached)
        {
            movementState = EMovementState.Reached;
        }

        return Reached;
    }

    protected void PerformMovement(Vector2 direction)
    {
        Vector2 position = gameObject.transform.position;
        if (HelperLibrary.ApproximateEqual(direction - position, Vector2.zero))
        {
            movementState = EMovementState.Aborted;
        }
        else
        {
            moveDirection = direction.normalized;
        }
    }
    protected void ApplyMovementForce()
    {
        rg2d.AddForce(moveDirection * moveSpeed, ForceMode2D.Force);

        if (rg2d.linearVelocity.magnitude > maxVelocity)
        {
            rg2d.linearVelocity = rg2d.linearVelocity.normalized * maxVelocity;
        }
    }

    /*
    private float MoveSpeed = 1.5f;

    void Move()
    {
        this.gameObject.transform.position += new Vector3(1, 0, 0) * Time.deltaTime;
    }

    void MoveDesPos(Vector3 _destination)
    {
        if (Vector2.Distance(_destination, transform.position) < 0.5f) return;

        Vector3 direction = _destination - this.transform.position;
        direction = direction.normalized;
        this.gameObject.transform.position += direction * MoveSpeed * Time.deltaTime;
    }
    */
}
