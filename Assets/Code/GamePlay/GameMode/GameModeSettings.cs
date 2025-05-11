using UnityEngine;

[CreateAssetMenu(fileName = "GameModeSettings", menuName = "Scriptable Objects/GameModeSettings")]
public class GameModeSettings : ScriptableObject
{
    public GameObject PlayerController;
    public GameObject GameRule;

}
