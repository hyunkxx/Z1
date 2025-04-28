using System.Collections;
using UnityEngine;

public class SpawnController : MonoBehaviour
{
    public GameObject PlayerPrefabs;
    void Start()
    {
        StartCoroutine(Spawn("TestMonster", 10));
        StartCoroutine(Spawn("TestBossMonster", 10));
    }

    void Update()
    {
        
    }

    IEnumerator Spawn(string _type,int _count)
    {
        int curCount = 0;

        while(curCount < _count)
        {
            GameObject obj = ObjectPool.Instance.PullObject(_type);
            obj.GetComponent<MonsterStateMachine>().target = PlayerPrefabs;

            curCount++;

            yield return new WaitForSeconds(1f);              
        }
    }
}
