using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
    private SurvivalSpawner m_Spawner;
    private StageData m_StageData;

    private int jamCount = 0;
    public event Action<int> OnChangedJamCount;
    public event Action<int> OnChangedWave;

    /* Spawner Wave Property */
    const float waveCycle = 60f;
    const float spawnCycle = 5f;
    public float currentWaveTime { get; private set; }
    bool bWaveRestart = false;

    protected override void OnDestroy()
    {
        if (_characterHandle.IsValid() && _characterHandle.Result != null)
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

    protected override void LoadSceneData()
    {
        base.LoadSceneData();

        m_StageData = Database.Instance.FindStageData(StageKey);
        if (m_StageData == null)
        {
            Debug.LogWarning("LoadStageData failed: invalid stage ID");
            Destroy(gameObject);
            return;
        }

        int loadedCount = 0;

        /* DropObjeect_XP */
        AssetLoader.LoadAssetAsync<GameObject>("DropObject_XP", () =>
        {
            ObjectPool pool = gameObject.AddComponent<ObjectPool>();
            pool.InitializePool(AssetLoader.GetHandleInstance<GameObject>("DropObject_XP"), 200);
            PoolContainer.RegisterPool(pool);
        });

        /* Spawn Visualizer */
        AssetLoader.LoadAssetAsync<GameObject>("Effect_SpawnVisualizer", () =>
        {
            ObjectPool pool = gameObject.AddComponent<ObjectPool>();
            pool.InitializePool(AssetLoader.GetHandleInstance<GameObject>("Effect_SpawnVisualizer"), 50);
            PoolContainer.RegisterPool(pool);
        });

        /* Stage Enemy Pool */
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

                    UIManager.Instance.MessagePopup.Message($"Stage\n{StageKey}", () => 
                    {
                        StartCoroutine(WaveLifeCycle());
                    });
                }
            });
        }
    }

    public void AddJam(int amount)
    {
        jamCount += amount;
        OnChangedJamCount?.Invoke(jamCount);
    }

    IEnumerator WaveLifeCycle()
    {
        currentWaveTime = waveCycle;
        int currentWave = 0;
       
        OnChangedWave?.Invoke(currentWave);

        while (currentWave < m_StageData.TotalWaveCount())
        {
            bWaveRestart = false;

            float spawnTimer = 0f;
            while (currentWaveTime >= 0f)
            {
                currentWaveTime -= Time.deltaTime;
                spawnTimer -= Time.deltaTime;

                if (spawnTimer <= 0f)
                {
                    spawnTimer = spawnCycle;
                    StartCoroutine(m_Spawner.SpawnEnemy(currentWave));
                }

                yield return null;
            }

            currentWaveTime = waveCycle;
            currentWave++;

            OnChangedWave?.Invoke(currentWave);

            m_Spawner.AllKill();

            
            UIManager.Instance.MessagePopup.Message($"{currentWave + 1} Wave", () =>
            {
                bWaveRestart = true;
            });

            yield return new WaitUntil(() => bWaveRestart);
        }
    }
}
