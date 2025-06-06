using UnityEngine;
using UnityEngine.UI;

public class DefenceCraftUI : MonoBehaviour
{
    [SerializeField] private GameObject Contents;
    [SerializeField] private Button CloseButton;
    //[SerializeField] private DefenceGameRule GameRule;

    private void Start()
    {
        CloseButton.onClick.AddListener(() => { this.gameObject.SetActive(false); });
        UpdateSlotInfo();
    }

    void UpdateSlotInfo()
    {
        for(int i = 0; i < Contents.transform.childCount; i++)
        {
            if (i > Database.Instance.CharacterAssetCount) return;

            Contents.transform.GetChild(i).GetComponent<DefenceCraftingSlot>().SetSlotInfo(i + 1000);
        }
    }
}
