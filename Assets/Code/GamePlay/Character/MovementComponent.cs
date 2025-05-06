using System;
using UnityEngine;


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
    [SerializeField] private float moveSpeed = 10.0f;
    [SerializeField] private float maxVelocity = 2.0f;
    EMovementState movementState = EMovementState.None;

    private float ReachedOffset = 0.5f;
    private Vector2 moveDirection;
    private Vector2 goalLocation;

    private Rigidbody2D rg2d;

    public float MaxVelocity => maxVelocity;
    public Vector2 MoveDirection => moveDirection;
    public Vector2 GoalLocation => goalLocation;
    public Rigidbody2D MovementRigidBody => rg2d;

    public event Action<bool> OnSpriteFlipChanged;
    public event Action OnReachedLocation;
    private SpriteRenderer rootSprite;

    private Character character;

    private void Start()
    {
        rootSprite = transform.Find("Root").GetComponent<SpriteRenderer>();
        character = transform.root.GetComponent<Character>();

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
    public bool IsMove()
    {
        return !HelperLibrary.ApproximateEqual(moveDirection, Vector2.zero);
    }
    public void ResetMovement()
    {
        movementState = EMovementState.None;
        //moveDirection = Vector2.zero;
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
            OnReachedLocation?.Invoke();
        }

        return Reached;
    }

    protected void PerformMovement(Vector2 direction)
    {
        Vector2 position = gameObject.transform.position;
        if (HelperLibrary.ApproximateEqual(direction - position, Vector2.zero))
        {
            movementState = EMovementState.Aborted;
            Debug.Log("ApproximateEqual");
        }
        else
        {
            moveDirection = direction.normalized;
            Debug.Log("else ApproximateEqual");
        }
    }
    protected void ApplyMovementForce()
    {
        rg2d.AddForce(moveDirection * moveSpeed, ForceMode2D.Force);
        if (rootSprite)
        {
            UpdateRootSpriteFlip();
        }

        //if (rg2d.linearVelocity.magnitude > maxVelocity)
        //{
        //    rg2d.linearVelocity = rg2d.linearVelocity.normalized * maxVelocity;
        //}
    }
    protected void UpdateRootSpriteFlip()
    {
        /* aim move case */
        if (character && IsMove())
        {
            if(character.TargetingComponent.HasNearTarget())
            {
                Vector3 targetDir = character.TargetingComponent.GetTargetDirection();
                if (rootSprite.flipX && targetDir.x > 0f)
                {
                    rootSprite.flipX = false;
                    OnSpriteFlipChanged?.Invoke(false);
                }
                else if (!rootSprite.flipX && targetDir.x < 0f)
                {
                    rootSprite.flipX = true;
                    OnSpriteFlipChanged?.Invoke(true);
                }
            }
            else
            {
                if (rootSprite.flipX && moveDirection.x > 0f)
                {
                    rootSprite.flipX = false;
                    OnSpriteFlipChanged?.Invoke(false);
                }
                else if (!rootSprite.flipX &&  moveDirection.x < 0f)
                {
                    rootSprite.flipX = true;
                    OnSpriteFlipChanged?.Invoke(true);
                }
            }
        }
    }
}
