using System;
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
    [SerializeField]
    protected MovementComponent movement;
    
    protected Rigidbody2D rg2d;
    protected SpriteRenderer spriteRenderer;

    protected Damageable damageable;
    protected Animator animator;
    protected CharacterAnimationController animController;
    protected WeaponComponent weaponComponent;
    protected GhostEffect ghostEffect;

    //test
    public Effect2D testEffect;

    public MovementComponent Movement => movement;

    bool bFaceRight = true;
    public Action<bool> OnChangeFlip;

    protected override void Awake()
    {
        Debug.Assert(movement != null, "MovementComponent is not assigned");

        rg2d = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animController = GetComponent<CharacterAnimationController>();
        weaponComponent = GetComponent<WeaponComponent>();
        ghostEffect = GetComponent<GhostEffect>();

        damageable = GetComponent<Damageable>();
        damageable.OnDamageTaken += TakeDamage;
    }
    protected override void Start()
    {
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
            Dash(movement.MoveDirection);

        if (Input.GetKeyDown(KeyCode.I))
        {
            testEffect.ActivateEffect(gameObject);
        }

    }

    protected virtual void TakeDamage(DamageEvent info)
    {
        Debug.Log(info);
    }

    public bool IsRight()
    {
        return bFaceRight;
    }
    public void FaceDirectionUpdate(Vector2 direction)
    {
        if(direction.x > 0f)
        {
            bFaceRight = true;
            transform.localScale = new Vector3(1f, 1f, 1f);
        }
        else if(direction.x < 0f)
        {
            bFaceRight = false;
            transform.localScale = new Vector3(-1f, 1f, 1f);
        }
    }
    public void Dash(Vector2 direction)
    {
        const float dash = 10f;

        ghostEffect.ActivateEffect();
        if (movement.IsMove())
        {
            rg2d.AddForce(direction.normalized * dash, ForceMode2D.Impulse);
        }
        else
        {
            Vector2 dir = IsRight() ? Vector2.right : Vector2.left;
            rg2d.AddForce(dir * dash, ForceMode2D.Impulse);
        }
    }
}
