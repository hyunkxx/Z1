using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.TextCore.Text;


public enum EFlipType
{
    Sprite,
    Transform
}

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
    [SerializeField] private bool m_isNPC = false;
    [SerializeField] private EFlipType m_FlipType;

    [SerializeField, ShowIf("m_FlipType", EFlipType.Transform)]
    private Transform m_FlipRoot;

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

    [SerializeField] private AIStateSet aIStateSet;

    public event Action<bool> OnChangedFlip;
    //public event Action OnDieEvent;

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
            InitializeNpcComponent();
        }
    }

    public void Initialize(int characterID)
    {
        characterStats = new CharacterStats(characterID);
        //characterStats.JobType;
    }

    public void InitializeNpcComponent()
    {
        characterStats = new CharacterStats(2001);

        ETeam teamID = damageable.TeamID;
        AIType AI = teamID == ETeam.Player ? AIType.Character : AIType.Monster;

        AIBrain brain = gameObject.AddComponent<AIBrain>();
        gameObject.AddComponent<StateMachine>();
        brain.Initialize(this, AI, aIStateSet);
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
        Debug.Log(info.damage);
        if (characterStats == null)
            return;

        characterStats.ApplyDamage(info);
        Debug.Log($"{characterStats.GetStat(EStatType.CurHealth)} / {characterStats.GetStat(EStatType.MaxHealth)}");

        if(!IsAlive())
        {
            Kiil();
        }
    }

    public void Kiil()
    {
        Destroy(gameObject);
    }

    public bool IsAlive()
    {
        return characterStats.GetStat(EStatType.CurHealth) > 0f;
    }

    public bool IsRight()
    {
        return !spriteRenderer.flipX;
    }

    public void OnInputAxis(Vector2 InputDirection)
    {
        Movement.MoveToDirection(InputDirection);
        bool Input = InputDirection != Vector2.zero ? ActionComponent.TryExecute(EActionType.MOVE) : ActionComponent.TryExecute(EActionType.IDLE);
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
        Vector3 faceDirection = targetingComponent.HasNearTarget() ? targetingComponent.GetTargetDirection() : movementComponent.MoveDirection;

        switch (m_FlipType)
        {
            case EFlipType.Sprite:
                if (spriteRenderer.flipX && faceDirection.x > 0f)
                {
                    spriteRenderer.flipX = false;
                    OnChangedFlip?.Invoke(false);
                }
                else if (!spriteRenderer.flipX && faceDirection.x < 0f)
                {
                    spriteRenderer.flipX = true;
                    OnChangedFlip?.Invoke(true);
                }
                break;
            case EFlipType.Transform:
                if (m_FlipRoot == null)
                    return;

                Vector3 localScale = m_FlipRoot.localScale;
                if (localScale.x < 0f && faceDirection.x > 0f)
                {
                    localScale.x *= -1f;
                    m_FlipRoot.localScale = localScale;
                    OnChangedFlip?.Invoke(false);
                }
                else if (localScale.x > 0f && faceDirection.x < 0f)
                {
                    localScale.x *= -1f;
                    m_FlipRoot.localScale = localScale;
                    OnChangedFlip?.Invoke(true);
                }
                break;
        }
    }

    public void SetNPC(bool value)
    {
        m_isNPC = value;
    }

    public Character GetCharacter()
    {
        return this;
    }
}