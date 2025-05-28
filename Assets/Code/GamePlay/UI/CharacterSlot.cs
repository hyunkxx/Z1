using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSlot : MonoBehaviour
{
    public int CharacterID = 0;
    public string CharacterName = "";
    public Image CharacterImage;
    public GameObject Focus;

    public Sprite GetSprite()
    {
        return CharacterImage.sprite;
    }


    public void SetSlotInfo(int _index)
    {
        CharacterDataAsset data = Database.Instance.FindCharacterAsset(1000 + _index);

        CharacterID = data.CharacterID;
        CharacterName = data.Name;
        CharacterImage.sprite = data.Sprite;
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