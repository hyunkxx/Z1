using UnityEngine;
using UnityEngine.EventSystems;

public class CameraDragHandlerer : MonoBehaviour
{    
    bool isDragging = false;
    Vector2 lastTouchPos;
    Camera mainCamera;
    public float moveSpeed = 1f;

    void Awake()
    {
        mainCamera = Camera.main;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        HandleMouseDrag();
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
            Vector3 newPos = new Vector3((worldDelta.x * moveSpeed) + cameraPos.x, cameraPos.y, cameraPos.z);

            mainCamera.transform.position = newPos;

            lastTouchPos = currentPosition;
        }
    }

}
