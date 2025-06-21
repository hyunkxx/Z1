using UnityEngine;
using UnityEngine.EventSystems;

public class CameraDragHandlerer : MonoBehaviour
{    
    bool isDragging = false;
    Vector2 lastTouchPos;
    Camera mainCamera;
    public float moveSpeed = 1f;
    public GameObject Target;

    private Vector3 prevTargetPos;

    ECameraMoveState CurState = ECameraMoveState.None;
    public enum ECameraMoveState
    {
        None,
        Targeting,
    }

    void Awake()
    {
        mainCamera = Camera.main;
    }

    void Start()
    {
        
    }

    void Update()
    {
        switch (CurState)
        {
            case ECameraMoveState.None:
                HandleMouseDrag();
                break;
            case ECameraMoveState.Targeting:
                TargetingObject();
                break;
        }
    }

    void HandleMouseDrag()
    {
        if (Input.GetMouseButtonDown(0))
        {
            isDragging = true;
            lastTouchPos = Input.mousePosition;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
        }

        if (isDragging)
        {
            Vector2 currentPosition = Input.mousePosition;

            Vector3 worldDelta = mainCamera.ScreenToWorldPoint(currentPosition) - mainCamera.ScreenToWorldPoint(lastTouchPos);

            Vector3 cameraPos = mainCamera.transform.position;
            Vector3 newPos = new Vector3((-worldDelta.x * moveSpeed) + cameraPos.x, cameraPos.y, cameraPos.z);

            mainCamera.transform.position = newPos;

            lastTouchPos = currentPosition;
        }
    }

    void TargetingObject()
    {
        //transform.position = new Vector3(Target.transform.position.x, Target.transform.position.y, transform.position.z);
        // 범위 내에서는 가만히 범위 밖으로 가면 카메라 이동

        if(isBecameVisible())
        {
            prevTargetPos = Target.transform.position;
            return;
        }

        Vector3 moveDistance = Target.transform.position - prevTargetPos;
        Camera.main.transform.position += new Vector3(moveDistance.x, moveDistance.y, 0f);
        prevTargetPos = Target.transform.position;
    }

    private bool isBecameVisible()
    {
        if (!Target) return false;

        Vector3 viewPos = Camera.main.WorldToViewportPoint(Target.transform.position);
        Debug.Log($"ViewPort Pos : {viewPos}");
        return viewPos.x >= 0 && viewPos.x <= 1 && viewPos.y >= 0 && viewPos.y <= 1;
    }

    public void ChangeState(ECameraMoveState state)
    {
        CurState = state;
        Target = null;
        isDragging = false;
        transform.position = new Vector3(transform.position.x, 0, transform.position.z);
    }

    public void ChangeState(ECameraMoveState state, GameObject target)
    {
        CurState = state;
        Target = target;
        isDragging = false;
        prevTargetPos = transform.position;
    }
}
