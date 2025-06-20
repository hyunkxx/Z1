using AYellowpaper.SerializedCollections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]

[CreateAssetMenu(fileName = "DefenceRewardAssetData", menuName = "Scriptable Objects/Data/DefenceRewardAssetData")]
public class DefenceRewardAssetData : ScriptableObject
{
    public SerializedDictionary<string, int> _defenceRewardData;
}