using AYellowpaper.SerializedCollections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RoundAssetData", menuName = "Scriptable Objects/Data/RoundAssetData")]

[System.Serializable]
public class RoundMonsterKeyElement
{
    public SerializedDictionary<string, int> _monsterSpawnData;
}

public class RoundAssetData : ScriptableObject
{
    [SerializeField] private List<RoundMonsterKeyElement> _round;

    public List<RoundMonsterKeyElement> Round => _round;
}