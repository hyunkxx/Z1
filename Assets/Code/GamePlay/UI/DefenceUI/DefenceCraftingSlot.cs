using UnityEngine;
using UnityEngine.UI;

public class DefenceCraftingSlot : MonoBehaviour
{
    CharacterDataAsset DataAsset;
    [SerializeField] DefenceSpawner Spawner;

    [SerializeField] private Button CraftingButton;
    [SerializeField] private Image CharacterImage;
    [SerializeField] private int CraftCount = 1;

    void Start()
    {
        CraftingButton.onClick.AddListener(Craft);
    }

    public void SetSlotInfo(int _characterID)
    {
        DataAsset = Database.Instance.FindCharacterAsset(_characterID);
        CharacterImage.sprite = DataAsset.Sprite;
        SetSprite(DataAsset.Sprite);
    }

    void Craft()
    {
        StartCoroutine(Spawner.Spawn(DataAsset.PrefabKey, CraftCount, Spawner.CharacterSpawnPos));
    }

    //void Craft()
    //{
    //    Debug.Log($"Craft : {DataAsset.CharacterID}");
    //    GridSlot waitingGrid = GridController.Instance.GetEmptyWaitingGrid();
    //    if (waitingGrid == null)
    //    {
    //        Debug.Log("¥Î±‚ø≠¿Ã ≤À √°Ω¿¥œ¥Ÿ");
    //        return;
    //    }

    //    waitingGrid.CurObject = Instantiate(AssetLoader.GetHandleInstance<GameObject>(DataAsset.PrefabKey), waitingGrid.transform.position, Quaternion.identity);
    //    waitingGrid.CurObject.AddComponent<CharacterDragHandler>().BaseSlot = waitingGrid;
    //    waitingGrid.CurObject.GetComponent<BoxCollider2D>().enabled = true;
    //    waitingGrid.CurObject.layer = LayerMask.NameToLayer("Character");
    //}

    public float targetVisualSize = 64f;

    public void SetSprite(Sprite newSprite)
    {
        Vector2 spriteSize = newSprite.rect.size;

        float maxAxis = Mathf.Max(spriteSize.x, spriteSize.y);
        float scale = targetVisualSize / maxAxis;

        Vector2 newSize = (spriteSize * scale) * 3f;
        CharacterImage.rectTransform.sizeDelta = new Vector2(newSize.x, newSize.x);
    }
}
