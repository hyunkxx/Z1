using UnityEngine;
using UnityEngine.UI;

public class DefenceCraftingSlot : MonoBehaviour
{
    public enum ECraftingType
    {
        Base,
        Field,
    }

    CharacterDataAsset DataAsset;
    Animator animator;
    [SerializeField] GameObject CraftingParticle;
    [SerializeField] DefenceSpawner Spawner;

    [SerializeField] private Button CraftingButton;
    [SerializeField] private Image CharacterImage;
    [SerializeField] private int CraftCount = 1;

    private ECraftingType CraftingType = ECraftingType.Field;

    private UIParticleComponent[] Particles;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        Particles = CraftingParticle.transform.GetComponentsInChildren<UIParticleComponent>();
        foreach (var obj in Particles)
            obj.gameObject.SetActive(false);
    }

    void Start()
    {
        CraftingButton.onClick.AddListener(Craft);
    }

    public void SetSlotInfo(int _characterID, ECraftingType _craftingType)
    {
        CraftingType = _craftingType;
        DataAsset = Database.Instance.FindCharacterAsset(_characterID);
        CharacterImage.sprite = DataAsset.Sprite;
        SetSprite(DataAsset.Sprite);
    }

    void Craft()
    {
        Transform[] spawnPos = CraftingType == ECraftingType.Base ? Spawner.ComandSpanwPos : Spawner.CharacterSpawnPos;

        animator.SetTrigger("Click");
        StartCoroutine(Spawner.Spawn(DataAsset.PrefabKey, CraftCount, spawnPos));
        foreach (UIParticleComponent pComponent in Particles)
            pComponent.StartParticleEmission();
    }

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
