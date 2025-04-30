using UnityEngine;
using UnityEngine.UI;

public class CharacterSlot : MonoBehaviour
{
    public Image CharacterImage;
    public GameObject Focus;

    public Sprite GetSprite()
    {
        return CharacterImage.sprite;
    }

    public void ActiveFocus()
    {
        if (Focus.activeSelf)
        {
            Focus.SetActive(false);
        }
        else
        {
            Focus.SetActive(true);
        }
    }
}
