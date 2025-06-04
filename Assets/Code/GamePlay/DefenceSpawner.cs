using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class DefenceSpawner : SpawnController
{
    public Transform[] MonsterSpawnPos;
    public Transform[] CharacterSpawnPos;

    private RoundAssetData roundData;

    private GameMode gameMode;

    public void SetRoundData(RoundAssetData roundAssetData) { roundData = roundAssetData; }
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
                break;
        }
    }

    void Initialize()
    {

    }

    public void NextRoundSpawn(int _round)
    {
        foreach(var spawnData in roundData.Round[_round]._monsterSpawnData)
        {
            string prefabsKey = spawnData.Key;
            int size = spawnData.Value;

            StartCoroutine(Spawn(AssetLoader.GetHandleInstance<GameObject>(prefabsKey).name, size, MonsterSpawnPos));
        }
    }

    void Update()
    {
        
    }
     
    public IEnumerator Spawn(string _type, int _count, Transform[] _spanwRange)
    {
        int curCount = 0;

        while (curCount < _count)
        {
            GameObject obj = base.Spawn(_type, RandSpawnPos(_spanwRange));

            if(_type.Contains("Character"))
            {
                obj.GetComponent<BoxCollider2D>().enabled = true;
                obj.GetComponent<Character>().SetIsNPC(true);
                obj.GetComponent<Character>().InitializeNpcComponent();
            }

            curCount++;

            yield return new WaitForSeconds(1f);
        }
    }


    Vector3 RandSpawnPos(Transform[] _spanwRange)
    {
        float randYPos = Random.Range(_spanwRange[0].transform.position.y, _spanwRange[1].transform.position.y);
        Vector3 SpawnPos = Vector3.zero;

        SpawnPos.x = _spanwRange[0].transform.position.x;
        SpawnPos.y = randYPos;

        return SpawnPos;

    }

}
