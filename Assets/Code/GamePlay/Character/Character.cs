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
    [SerializeField] protected Movement movement;
    
    protected Rigidbody2D rg2d;
    protected SpriteRenderer spriteRenderer;

    bool bFaceRight;

    private void Awake()
    {
        Debug.Assert(movement != null, "movement is not assigned");

        rg2d = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public bool IsFaceRight()
    {
        return bFaceRight;
    }
    public void OnInputAxis(Vector2 inputDirection)
    {
        FaceDirectionUpdate(inputDirection);
        movement.MoveToDirection(inputDirection);
    }
    private void FaceDirectionUpdate(Vector2 direction)
    {
        if(direction.x > 0f)
        {
            bFaceRight = true;
            spriteRenderer.flipX = false;
        }
        else if(direction.x < 0f)
        {
            bFaceRight = false;
            spriteRenderer.flipX = true;
        }
    }
}
