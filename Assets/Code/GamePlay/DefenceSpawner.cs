using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefenceSpawner : SpawnController
{
    public Transform SpawnPos;
    public List<Transform> DestinationList = new List<Transform>();

    void Start()
    {
        //StartCoroutine(Spawn("Defence_Orc", 10));
        GameMode mode = GameManager.Instance.GameMode;
        mode.OnChangeGameState += OnChangeGameState;
        Initialize();

    }

    private void OnDestroy()
    {
        if (GameManager.IsValid())
        {
            GameMode mode = GameManager.Instance.GameMode;
            if (mode)
            {
                mode.OnChangeGameState -= OnChangeGameState;
            }
        }
    }
    private void OnChangeGameState(EGameState state)
    {
        switch (state)
        {
            case EGameState.ReadyGame:
                Initialize();
                break;
        }
    }

    void Initialize()
    {
        // Input Data
        AddPool("DefenceMonsterPrefabs/Defence_Orc", 10); // DataPath, Size

        objectPools.FindPools();

        foreach (var pool in objectPools.GetContainer())
        {
            StartCoroutine(Spawn(pool.Key, pool.Value.PoolSize));
        }

    }

    void AddPool(string _monsterPath, int _size)
    {
        ObjectPool pool = gameObject.AddComponent<ObjectPool>();
        pool.InitializePool(Resources.Load<GameObject>(_monsterPath), _size);
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
