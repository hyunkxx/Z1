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

    private TestItemData ItemData;

    void Start()
    {
        EquipButton.onClick.AddListener(Equip);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetItemInfo(TestItemData _itemData)
    {
        ItemData = _itemData;
        ItemImage.sprite = _itemData.sprite;
        ItemName.text = _itemData.Name;
    }

    public void Equip()
    {
        characterEquipUI.Equip(ItemData);
        gameObject.SetActive(false);
    }
}
