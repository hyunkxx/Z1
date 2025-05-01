using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefenceSpawner : SpawnController
{
    public Transform SpawnPos;
    public List<Transform> DestinationList = new List<Transform>();

    void Start()
    {
        StartCoroutine(Spawn("Defence_Orc", 10));
    }

    void Update()
    {
        
    }
     
    IEnumerator Spawn(string _type, int _count)
    {
        int curCount = 0;

        while (curCount < _count)
        {
            GameObject obj = base.Spawn(_type, SpawnPos.position);
            obj.GetComponent<DefenceAIController>().DestinationList = DestinationList;
            curCount++;

            yield return new WaitForSeconds(1f);
        }
    }
}
