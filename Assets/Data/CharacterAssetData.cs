using UnityEngine;


[CreateAssetMenu(fileName = "CharacterAssetData", menuName = "Scriptable Objects/Data/CharacterAssetData")]
public class CharacterAssetData : ScriptableObject
{
    [SerializeField] private int _characterID;
    [SerializeField] private string _name;
    [SerializeField] private string _prefabKey;
    [SerializeField] private Sprite _sprite;

    public int CharacterID => _characterID;
    public string Name => _name;
    public string PrefabKey => _prefabKey;
    public Sprite Sprite => _sprite;
}   
