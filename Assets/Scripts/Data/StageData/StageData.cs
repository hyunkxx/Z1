using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// @ StageID : ex) 1-1
[Serializable]
public struct WeveElement
{
    public List<CharacterDataAsset> m_SpawnEnemies;
}

[CreateAssetMenu(fileName = "StageData", menuName = "Scriptable Objects/Data/StageData")]
public class StageData : ScriptableObject
{
    [SerializeField] private string m_StageName;
    [SerializeField] private List<WeveElement> m_WaveInfos;

    public string StageName => m_StageName;
    public List<WeveElement> WaveInfo => m_WaveInfos;

    /* Unique IDs of all enemies spawned across all waves */
    [NonSerialized] private HashSet<int> uniqueEnemyIDs = new HashSet<int>();
    [NonSerialized] private bool bInitIDs;

    public int TotalWaveCount() { return m_WaveInfos.Count; }
    public HashSet<int> GetSpawnableEnemyIDs()
    {
        if(bInitIDs)
        {
            return uniqueEnemyIDs;
        }

        foreach(WeveElement element in m_WaveInfos)
        {
            foreach (CharacterDataAsset CDA in element.m_SpawnEnemies)
            {
                uniqueEnemyIDs.Add(CDA.CharacterID);
            }
        }

        bInitIDs = true;
        return uniqueEnemyIDs;
    }
}