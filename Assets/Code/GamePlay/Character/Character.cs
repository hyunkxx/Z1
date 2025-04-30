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

public class Character : MonoBehaviour
{
    [SerializeField]
    protected MovementComponent movement;
    
    protected Rigidbody2D rg2d;
    protected SpriteRenderer spriteRenderer;

    protected Animator animator;
    protected CharacterAnimationController animController;
    protected WeaponSystem weaponSystem;

    bool bFaceRight = true;

    public MovementComponent Movement => movement;

    private void Awake()
    {
        Debug.Assert(movement != null, "MovementComponent is not assigned");

        rg2d = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animController = GetComponent<CharacterAnimationController>();
        weaponSystem = GetComponent<WeaponSystem>();
    }

    public bool IsFaceRight()
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
}
