using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;


public class SurvivalGameRule
    : GameRule
{
    public string StageKey { get; private set; }
    public int CharacterID { get; private set; }
    private AsyncOperationHandle<GameObject> _characterHandle;

    [SerializeField]
    private SpawnerPool m_Spawner;
    private StageData m_StageData;

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

        _gameMode.ChangeGameState(EGameState.LoadGame);

        string[] parameters = Game.Payload.Split('#');

        StageKey = parameters[0];
        CharacterID = int.Parse(parameters[1]);

        Transform playerStart = _gameMode.PlayerStart;
        SpawnCharacter(playerStart.position);
        LoadSceneData();
    }

    protected override void Start()
    {
        base.Start();
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
            }
        };

        return true;
    }

    public void LoadSceneData()
    {
        m_StageData = Database.Instance.FindStageData(StageKey);
        if (m_StageData == null)
        {
            Debug.LogWarning("LoadStageData failed: invalid stage ID");
            Destroy(gameObject);
            return;
        }

        int loadedCount = 0;
        HashSet<int> uniqueEnemyIDs = m_StageData.GetSpawnableEnemyIDs();
        foreach(int ID in uniqueEnemyIDs)
        {
            AssetLoader.LoadAssetAsync<GameObject>(Database.Instance.FindCharacterAsset(ID).PrefabKey, () =>
            {
                loadedCount++;
                if (uniqueEnemyIDs.Count == loadedCount)
                {
                    m_Spawner.Initialize(m_StageData);
                    _gameMode.ChangeGameState(EGameState.ReadyGame);
                }
            });
        }
    }

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
