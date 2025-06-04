using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;


public class SurvivalGameRule
    : GameRule
{
    public int CharacterID { get; private set; }

    private AsyncOperationHandle<GameObject> _characterHandle;

    protected override void OnDestroy()
    {
        if (_characterHandle.IsValid())
        {
            Addressables.ReleaseInstance(_characterHandle);
        }

        if (GameManager.IsValid())
        {
            GameManager.Instance.RegisterGameMode(null);
        }

        base.OnDestroy();
    }

    protected override void Awake()
    {
        base.Awake();

        GameManager Game = GameManager.Instance;
        if(!Game.HasPayload())
        {
            Debug.LogWarning("need character id");
            Destroy(gameObject);
            return;
        }

        CharacterID = int.Parse(Game.Payload);

        Transform playerStart = _gameMode.PlayerStart;
        SpawnCharacter(playerStart.position);
    }

    protected override void Start()
    {
        base.Start();
        Debug.Log("Rule Start");
    }

    private void OnChangeGameState(EGameState state)
    {
        switch (state)
        {
            case EGameState.LoadGame:
                //LoadSceneData();
                break;
            case EGameState.ReadyGame:
                //Initialize();
                break;
            case EGameState.EnterGame:
                break;
        }
    }

    private bool SpawnCharacter(Vector3 location)
    {
        var AssetData = Database.Instance.FindCharacterAsset(CharacterID);
        if (!AssetData)
            return false;

        Addressables.InstantiateAsync(AssetData.PrefabKey, location, Quaternion.identity).Completed += (handle) =>
        {
            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                _characterHandle = handle;

                Character character = handle.Result.GetComponent<Character>();
                character.Initialize(CharacterID);

                CameraMovement cameraMovement = Camera.main.GetComponent<CameraMovement>();
                cameraMovement.SetViewTarget(character.gameObject);

                _gameMode.PlayerController.ConnectCharacter(character);
                _gameMode.ChangeGameState(EGameState.ReadyGame);
            }
        };

        return true;
    }

    //void LoadSceneData()
    //{
    //    roundData = Database.Instance.DefenseRoundAssetData.GetValueOrDefault(CurMode);
    //    Spawner.SetRoundData(roundData);

    //    for (int i = 0; i < Database.Instance.CharacterAssetCount; ++i)
    //    {
    //        MaxLoadingCount++;
    //        AssetLoader.LoadAssetAsync<GameObject>(Database.Instance.FindCharacterAsset(i + 1000).PrefabKey, () =>
    //        {
    //            CurLoadingCount++;
    //            if (MaxLoadingCount == CurLoadingCount)
    //                _gameMode.ChangeGameState(EGameState.ReadyGame);
    //        });
    //    }

    //    for (int i = 1; i <= roundData.Round.Count; ++i)
    //    {
    //        foreach (var data in roundData.Round[i - 1]._monsterSpawnData)
    //        {
    //            MaxLoadingCount++;
    //            AssetLoader.LoadAssetAsync<GameObject>(data.Key, () =>
    //            {
    //                CurLoadingCount++;
    //                if (MaxLoadingCount == CurLoadingCount)
    //                    _gameMode.ChangeGameState(EGameState.ReadyGame);
    //            });
    //        }
    //    }
    //}

    //void Initialize()
    //{
    //    for (int i = 1; i <= roundData.Round.Count; ++i)
    //    {
    //        foreach (var data in roundData.Round[i - 1]._monsterSpawnData)
    //        {
    //            ObjectPool pool = Spawner.objectPools.GetPool(data.Key);

    //            if (pool)
    //            {
    //                pool.ExpandPool(data.Value);
    //            }
    //            else
    //            {
    //                pool = Spawner.gameObject.AddComponent<ObjectPool>();
    //                pool.InitializePool(AssetLoader.GetHandleInstance<GameObject>(data.Key), data.Value);
    //            }

    //            Spawner.objectPools.RegisterPool(pool);
    //        }
    //    }
    //}

}
