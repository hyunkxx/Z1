using UnityEngine;

public class DefencePlayerController : PlayerController
{
    GameObject SelectObject = null;
    protected override void Start()
    {
        base.Awake();
    }
    protected override void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            LayerMask mask = 1 << LayerMask.NameToLayer("Character");

            RaycastHit2D[] hit = Physics2D.RaycastAll(mousePos, Vector2.zero, 0f, mask);
            
            for(int i = 0; i < hit.Length; ++i)
            {
                if (hit[i].collider != null)
                {
                    SelectObject = hit[i].collider.gameObject;
                    hit[i].collider.GetComponent<CharacterDragHandler>().SelectCharacter();
                }
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (SelectObject == null) return;

            SelectObject.GetComponent<CharacterDragHandler>().DeselectCharacter();
            SelectObject = null;
        }
    }
}