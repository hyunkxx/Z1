using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class DefenceSpawner : SpawnController
{
    public Transform SpawnPos;
    public List<Transform> DestinationList = new List<Transform>();
    private RoundAssetData roundData;

    private GameMode gameMode;

    void Start()
    {
        gameMode = GameManager.Instance.GameMode;
        gameMode.OnChangeGameState += OnChangeGameState;
    }

    private void OnDestroy()
    {
        if (GameManager.IsValid())
        {
            if (gameMode)
            {
                gameMode.OnChangeGameState -= OnChangeGameState;
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
            case EGameState.EnterGame:
                StartGame();
                break;
        }
    }

    void Initialize()
    {
        objectPools.FindPools();  
    }

    void StartGame()
    {
        foreach (var pool in objectPools.GetContainer())
        {
            StartCoroutine(Spawn(pool.Key, pool.Value.PoolSize));
        }

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
