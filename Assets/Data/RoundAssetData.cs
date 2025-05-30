using AYellowpaper.SerializedCollections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class RoundMonsterKeyElement
{
    public SerializedDictionary<string, int> _monsterSpawnData;
}

[CreateAssetMenu(fileName = "RoundAssetData", menuName = "Scriptable Objects/Data/RoundAssetData")]
public class RoundAssetData : ScriptableObject
{
    [SerializeField] private List<RoundMonsterKeyElement> _round;

    public List<RoundMonsterKeyElement> Round => _round;
}