using UnityEngine;

public class CharacterDragHandler : Z1Behaviour
{
    private bool isSelect = false;
    private Vector3 offset;
    private Vector3 originalPosition;
    public GridSlot BaseSlot = null;

    protected override void Start()
    {
        base.Start();
    }
    protected override void Update()
    {
        base.Update();

        if (isSelect)
        {
            transform.position = GetMouseWorldPosition() + offset;
        }
    }

    public void SelectCharacter()
    {
        isSelect = true;
        offset = transform.position - GetMouseWorldPosition();
        originalPosition = transform.position;
        GridController.Instance.GridGroup.SetActive(true);
    }

    public void DeselectCharacter()
    {
        isSelect = false;

        Vector2 dropPos = transform.position;
        if (!GridController.Instance.isClampPos(dropPos))
        {
            transform.position = originalPosition;
            return;
        }

        GridSlot curCell = GridController.Instance.GetNearestCell(originalPosition);
        GridSlot nearestCell = GridController.Instance.GetNearestCell(dropPos);

        Vector2 snappedPos = GridController.Instance.SnapPos(nearestCell.transform.position.x, nearestCell.transform.position.y);
        transform.position = snappedPos;

        if (curCell == null)
        {
            nearestCell.isStay = true;
            nearestCell.CurObject = this.gameObject;
            nearestCell.CurObject.transform.position = nearestCell.transform.position;
            BaseSlot.CurObject = null;
        }
        else
        {
            GridController.Instance.SwapGrid(curCell, nearestCell);
        }

        originalPosition = transform.position;
        GridController.Instance.GridGroup.SetActive(false);
    }

    private Vector3 GetMouseWorldPosition()
    {
        return Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }
}
