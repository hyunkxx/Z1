using System;
using UnityEngine;
using UnityEngine.EventSystems;


enum EMovementState
{
    None,
    DirectionalMovement,
    PositionalMovement,
    Aborted,
    Reached
}

public class MovementComponent : MonoBehaviour
{
    [Header("Movement Properties")]
    [SerializeField] private float moveSpeed = 1.0f;
    [SerializeField] private float maxVelocity = 2.0f;
    EMovementState movementState = EMovementState.None;

    private float ReachedOffset = 0.01f;
    private Vector2 moveDirection;
    private Vector2 goalLocation;

    private Rigidbody2D rg2d;

    public float MaxVelocity => maxVelocity;
    public Vector2 MoveDirection => moveDirection;
    public Vector2 GoalLocation => goalLocation;
    public Rigidbody2D MovementRigidBody => rg2d;

    private Action OnReachedCallback;

    private void Start()
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
                if (!HasReachedLocation())
                    ApplyMovementForce();
                break;
            default:
                ResetMovement();
                break;
        }
    }
    public bool IsMove()
    {
        return !HelperLibrary.ApproximateEqual(moveDirection, Vector2.zero);
    }
    public void ResetMovement()
    {
        rg2d.linearVelocity = Vector2.zero;
        movementState = EMovementState.None;
        moveDirection = Vector2.zero;
        goalLocation = Vector2.zero;
    }
    public void MoveToDirection(Vector2 direction)
    {
        PerformMovement(direction);
        movementState = EMovementState.DirectionalMovement;
    }
    public void MoveToLocation(Vector2 location, Action onReachedCallback = null)
    {
        Vector2 position = transform.position;
        Vector2 direction = (location - position).normalized;
        goalLocation = location;
        movementState = EMovementState.PositionalMovement;

        OnReachedCallback = onReachedCallback;

        PerformMovement(direction);
    }

    public bool HasReachedLocation()
    {
        Vector2 position = transform.position;
        bool Reached = Vector2.SqrMagnitude(goalLocation - position) <= ReachedOffset;

        if (Reached)
        {
            ResetMovement();
            movementState = EMovementState.Reached;
            OnReachedCallback?.Invoke();
        }

        return Reached;
    }

    protected void PerformMovement(Vector2 direction)
    {
        if (direction == Vector2.zero)
        {
            moveDirection = Vector2.zero;
            movementState = EMovementState.Aborted;
        }
        else
        {
            moveDirection = direction.normalized; // 
        }
    }
    protected void ApplyMovementForce()
    {
        rg2d.AddForce(moveDirection * moveSpeed, ForceMode2D.Force);
        //if (rg2d.linearVelocity.magnitude > maxVelocity)
        //{
        //    rg2d.linearVelocity = rg2d.linearVelocity.normalized * maxVelocity;
        //}
    }
}
