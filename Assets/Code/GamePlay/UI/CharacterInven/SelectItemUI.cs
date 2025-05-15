using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SelectItemUI : MonoBehaviour
{ 
    [SerializeField] private CharacterEquipUI characterEquipUI;
    [SerializeField] private Image ItemImage;
    [SerializeField] private TextMeshProUGUI ItemName;
    [SerializeField] private Button EquipButton;
    [SerializeField] private Button UpgradeButton;
    [SerializeField] private Button CollectButton;

    private TestItemData itemData;
    private int SlotIndex;

    void Start()
    {
        EquipButton.onClick.AddListener(Equip);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetItemInfo(int _invenIndex)
    {
        itemData = Database.Instance.TestInvenList[_invenIndex];
        ItemImage.sprite = Database.Instance.TestInvenList[_invenIndex].sprite;
        ItemName.text = Database.Instance.TestInvenList[_invenIndex].Name;
        SlotIndex = _invenIndex;

    }

    public void Equip()
    {
        characterEquipUI.Equip(itemData.ID, SlotIndex);
        gameObject.SetActive(false);
    }
}
