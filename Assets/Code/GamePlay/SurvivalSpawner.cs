using System.Collections;
using UnityEngine;

public class SurvivalSpawner : SpawnController
{
    public GameObject PlayerPrefabs;

    void Start()
    {
        StartCoroutine(Spawn("Orc", 10));
        StartCoroutine(Spawn("TestBossMonster", 10));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator Spawn(string _type, int _count)
    {
        int curCount = 0;

        while (curCount < _count)
        {
            GameObject obj = base.Spawn(_type);
            obj.GetComponent<MonsterStateMachine>().target = PlayerPrefabs;
            obj.GetComponent<SurvivalAIController>().target = PlayerPrefabs;
            curCount++;

            yield return new WaitForSeconds(1f);
        }
    }
}
