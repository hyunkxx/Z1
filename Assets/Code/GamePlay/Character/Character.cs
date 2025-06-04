using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.TextCore.Text;


public interface ICharacterQueryable
{
    public Character GetCharacter();
}

[System.Serializable]
public struct CharacterView
{
    public Transform m_weaponSocket;
    public Transform m_weaponEndSocket;
}

[System.Serializable]
public struct TransformData
{
    public Vector3 position;
    public Vector3 localPosition;
    public Quaternion rotation;

    public TransformData(Vector3 position, Vector3 localPosition, Quaternion rotation)
    {
        this.position = position;
        this.localPosition = localPosition;
        this.rotation = rotation;
    }

    public TransformData(Transform transform)
    {
        this.position = transform.position;
        this.localPosition = transform.localPosition;
        this.rotation = transform.rotation;
    }
}

[RequireComponent(typeof(Damageable))]
[RequireComponent(typeof(MovementComponent))]
public class Character 
    : Z1Behaviour
    , ICharacterQueryable
{
    [SerializeField] protected CharacterView characterView;
    [SerializeField] protected MovementComponent movementComponent;

    [SerializeField] protected bool m_isNPC;

    protected WeaponComponent weaponComponent;
    protected TargetingComponent targetingComponent;
    protected CharacterAnimationController animController;
    protected ActionComponent actionComponent;

    protected Rigidbody2D rg2d;
    protected SpriteRenderer spriteRenderer;
    protected Animator animator;

    protected CharacterStats characterStats;
    protected Damageable damageable;

    public bool IsNPC => m_isNPC;
    public CharacterView CharacterView => characterView;
    public TargetingComponent TargetingComponent => targetingComponent;
    public ActionComponent ActionComponent => actionComponent;
    public MovementComponent Movement => movementComponent;
    public CharacterStats Stats => characterStats;
    public Animator Animator => animator;

    public event Action<bool> OnChangedFlip;

    public void Initialize(int characterID)
    {
        characterStats = new CharacterStats(characterID);
        //characterStats.JobType;
    }

    protected override void Awake()
    {
        base.Awake();

        rg2d = GetComponent<Rigidbody2D>();
        spriteRenderer = transform.Find("Root").GetComponent<SpriteRenderer>();
        animator = transform.Find("Root").GetComponent<Animator>();

        actionComponent = GetComponent<ActionComponent>();
        animController = GetComponent<CharacterAnimationController>();

        weaponComponent = GetComponent<WeaponComponent>();
        targetingComponent = GetComponentInChildren<TargetingComponent>();

        damageable = GetComponent<Damageable>();
        damageable.OnDamageTaken += TakeDamage;

        if(IsNPC)
        {
            ETeam teamID = damageable.TeamID;
            AIType AI = teamID == ETeam.Player ? AIType.Character : AIType.Monster;

            AIBrain brain = gameObject.AddComponent<AIBrain>();
            gameObject.AddComponent<StateMachine>();
            brain.Initialize(this, AI);
        }
    }
    protected override void OnDestroy()
    {
        if (damageable)
        {
            damageable.OnDamageTaken -= TakeDamage;
        }
    }
    protected override void Update()
    {
        UpdateFlip();
    }
    protected virtual void TakeDamage(DamageEvent info)
    {
        Debug.Log("Player HIT");
    }
    public bool IsRight()
    {
        return !spriteRenderer.flipX;
    }

    public Vector2 GetCharacterDirection()
    {
        if (movementComponent.IsMove())
            return movementComponent.MoveDirection.normalized;
        else
            return IsRight() ? Vector2.right : Vector2.left;
    }
    public void UpdateFlip()
    {
        if(targetingComponent.HasNearTarget())
        {
            Vector3 targetDir = targetingComponent.GetTargetDirection();
            if (spriteRenderer.flipX && targetDir.x > 0f)
            {
                spriteRenderer.flipX = false;
                OnChangedFlip?.Invoke(false);
            }
            else if (!spriteRenderer.flipX && targetDir.x < 0f)
            {
                spriteRenderer.flipX = true;
                OnChangedFlip?.Invoke(true);
            }
        }
        else
        {
            Vector3 moveDirection = movementComponent.MoveDirection;
            if (spriteRenderer.flipX && moveDirection.x > 0f)
            {
                spriteRenderer.flipX = false;
                OnChangedFlip?.Invoke(false);
            }
            else if (!spriteRenderer.flipX && moveDirection.x < 0f)
            {
                spriteRenderer.flipX = true;
                OnChangedFlip?.Invoke(true);
            }
        }
    }

    public Character GetCharacter()
    {
        return this;
    }
}