using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;


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

public class Character : Z1Behaviour
{
    [SerializeField] protected MovementComponent movementComponent;

    protected WeaponComponent weaponComponent;
    protected TargetingComponent targetingComponent;
    protected CharacterAnimationController animController;

    protected Rigidbody2D rg2d;
    protected SpriteRenderer spriteRenderer;

    protected CharacterStats characterStats;

    protected Damageable damageable;
    protected GhostEffect ghostEffect;

    //test
    public AttackAction testEffect;

    public TargetingComponent TargetingComponent => targetingComponent;
    public MovementComponent Movement => movementComponent;
    public CharacterStats Stats => characterStats;

    protected override void Start()
    {
        rg2d = GetComponent<Rigidbody2D>();
        spriteRenderer = transform.Find("Root").GetComponent<SpriteRenderer>();
        animController = GetComponent<CharacterAnimationController>();
        ghostEffect = GetComponent<GhostEffect>();
        characterStats = GetComponent<CharacterStats>();

        weaponComponent = GetComponent<WeaponComponent>();
        targetingComponent = GetComponentInChildren<TargetingComponent>();

        damageable = GetComponent<Damageable>();
        damageable.OnDamageTaken += TakeDamage;

        ghostEffect.Initialize(spriteRenderer);
    }
    protected override void OnDestroy()
    {
        damageable.OnDamageTaken -= TakeDamage;
    }
    protected override void Update()
    {
        /* temp */
        if (Input.GetKeyDown(KeyCode.LeftShift))
            Dash();

        if (Input.GetKeyDown(KeyCode.I))
        {
            testEffect.ExcuteAction();
        }
    }
    protected virtual void TakeDamage(DamageEvent info)
    {
        Debug.Log("Player HIT");
    }
    public bool IsRight()
    {
        return !spriteRenderer.flipX;
    }
    public void Dash()
    {
        const float dashPower = 10f;

        ghostEffect.ActivateEffect();
        Vector2 direction = GetCharacterDirection();
        rg2d.AddForce(direction * dashPower, ForceMode2D.Impulse);
    }
    public Vector2 GetCharacterDirection()
    {
        //if (movementComponent.IsMove())
        //    return movementComponent.MoveDirection.normalized;
        //else
        //    return IsRight() ? Vector2.right : Vector2.left;

        return movementComponent.MoveDirection.normalized;
    }
}