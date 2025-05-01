using System.Collections;
using UnityEngine;

public class SurvivalSpawner : SpawnController
{
    public GameObject PlayerPrefabs;

    void Start()
    {
        StartCoroutine(Spawn("Orc", 1));
        //StartCoroutine(Spawn("TestBossMonster", 10));
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
            bool isValidPos = false;
            float xPos = 0, yPos = 0;

            while (!isValidPos)
            {
                xPos = Random.Range(-10, 10);
                yPos = Random.Range(-6, 6);
                isValidPos = IsValidSpawnPosition(PlayerPrefabs.transform.position, new Vector3(xPos, yPos, 0));
            }

            transform.position = PlayerPrefabs.transform.position + new Vector3(xPos, yPos, 0);

            GameObject obj = base.Spawn(_type, new Vector3(xPos, yPos));
            obj.GetComponent<MonsterStateMachine>().target = PlayerPrefabs;
            obj.GetComponent<SurvivalAIController>().target = PlayerPrefabs;
            curCount++;

            yield return new WaitForSeconds(1f);
        }
    }

    bool IsValidSpawnPosition(Vector3 _targetPos, Vector3 _position)
    {
        if (Vector3.Distance(_position, _targetPos) > 9)
            return true;

        return false;
    }
}
