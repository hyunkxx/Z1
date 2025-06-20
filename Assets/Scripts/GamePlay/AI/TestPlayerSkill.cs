using System.Collections.Generic;
using UnityEngine;

public class TestPlayerSkill : MonoBehaviour
{
    bool isDragging = false;
    Vector2 lastTouchPos;
    public float moveSpeed = 1f;

    [SerializeField] GameObject RangeSprite;
    [SerializeField] TargetingComponent targetingComponent;

    [SerializeField]
    GameObject abilityPrefab;

    [SerializeField, ShowIf("m_abilityTarget", EAbilityTarget.Multi)]
    int targetCount = 10;

    private void Awake()
    {
        transform.position = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, transform.position.z);
    }

    private void Start()
    {
        float size = targetingComponent.GetComponent<CircleCollider2D>().radius;
        RangeSprite.transform.localScale = new Vector2(size, size);
    }

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
            ActiveSkill();
            Destroy(gameObject);
        }

        if (isDragging)
        {
            Vector2 currentPosition = Input.mousePosition;

            Vector3 worldDelta = Camera.main.ScreenToWorldPoint(currentPosition) - Camera.main.ScreenToWorldPoint(lastTouchPos);

            Vector3 newPos = new Vector3((worldDelta.x * moveSpeed) + transform.position.x, (worldDelta.y * moveSpeed) + transform.position.y, transform.position.z);

            transform.position = newPos;

            lastTouchPos = currentPosition;
        }
    }

    void ActiveSkill()
    {
        Debug.Log("ÆÛÆÛÆÛÆÛÆã");
        InternalExecuteAction();
        Camera.main.GetComponent<CameraDragHandlerer>().ChangeState(CameraDragHandlerer.ECameraMoveState.None);
    }

    void InternalExecuteAction()
    {
        if (abilityPrefab == null)
            return;


        IReadOnlyList<TargetElement> targets = targetingComponent.GetTargetList();
        if (targets.Count < 1)
            return;

        GameObject targetObject = targetingComponent.GetNearestTarget();
        if (targetObject == null)
            return;

        for (int i = 0; i < targetCount; ++i)
        {
            if (targets.Count <= i)
                break;

            GameObject obj = Instantiate(abilityPrefab, targets[i].target.transform.position, Quaternion.identity);
            Ability ability = obj.GetComponent<Ability>();
            ability.Activate(gameObject, targets[i].target);
        }
    }
}
