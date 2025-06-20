using UnityEngine;
using System.Collections;
using Unity.VisualScripting;
using System.Collections.Generic;
using System;

public class DefenceGameRule
    : GameRule
{
    [SerializeField]
    public DefenceSpawner Spawner;

    public Action NextRoundAction;

    DefenceUI defenceUI;
    RoundAssetData roundData;
    DefenceRewardAssetData rewardData;

    public string CurMode = "Easy";

    private float ReadyTime = 5f;
    private float RoundTime = 10f;
    private int Round = 0;

    [SerializeField] private DefenceCastle PlayerTeamHP;
    [SerializeField] private DefenceCastle EnemyTeamHP;

    [SerializeField] static public int HaveGreenStoneCount = 0;
    public int HaveBlueStoneCount = 0;
    public int HaveRedStoneCount = 0;

    private int MaxLoadingCount = 0;
    private int CurLoadingCount = 0;

    public float round => Round;
    public float roundTime => RoundTime;

    public enum ERewardType
    {
        Lose,
        Win,
        Ads,
    }

    protected override void Awake()
    {
        base.Awake();
        CurMode = GameManager.Instance.Payload;

        _gameMode.OnChangeGameState += OnChangeGameState;
        _gameMode.ChangeGameState(EGameState.LoadGame);
    }

    // 골드, Exp, 특성 포인트(유저 특성), 스킬 포인트 (유저 스킬 강화)    

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
    }

    private void WinGame()
    {
        defenceUI.WinPanel.SetActive(true);
    }

    private void LoseGame()
    {
        defenceUI.LosePanel.SetActive(true);
    }

    public void GetReward(ERewardType rewardType)
    {
        switch(rewardType)
        {
            case ERewardType.Lose:
                HaveBlueStoneCount = rewardData._defenceRewardData["Gold"] / 2;
                HaveBlueStoneCount = rewardData._defenceRewardData["Exp"] / 2;
                break;

            case ERewardType.Win:
                HaveBlueStoneCount = rewardData._defenceRewardData["Gold"];
                HaveBlueStoneCount = rewardData._defenceRewardData["Exp"];
                break;

            case ERewardType.Ads:
                HaveBlueStoneCount = rewardData._defenceRewardData["Gold"] * 2;
                HaveBlueStoneCount = rewardData._defenceRewardData["Exp"] * 2;
                break;
        }

        GameManager.Instance.OpenScene("Lobby");
    }

    private IEnumerator RoundCheck()
    {
        while (true)
        {
            if (PlayerTeamHP.HP <= 0)
            {
                LoseGame();
                yield break;
            }

            if (roundData.Round.Count > round)
            {
                NextRoundAction.Invoke();
                Spawner.NextRoundSpawn(Round);
                ++Round;
            }

            yield return new WaitForSeconds(RoundTime);

            if (EnemyTeamHP.HP <= 0)
            {
                WinGame();
                yield break;
            }
        }
    }

    private void OnChangeGameState(EGameState state)
    {
        switch (state)
        {
            case EGameState.LoadGame:
                LoadSceneData();
                break;
            case EGameState.ReadyGame:
                Initialize();
                StartCoroutine(Timer(ReadyTime, () => _gameMode.ChangeGameState(EGameState.EnterGame)));
                break;
            case EGameState.EnterGame:
                StartCoroutine(RoundCheck());
                break;
        }
    }

    void LoadSceneData()
    {
        roundData = Database.Instance.DefenseRoundAssetData.GetValueOrDefault(CurMode);
        rewardData = Database.Instance.DefenseRewardAssetDatas.GetValueOrDefault(CurMode);
        Spawner.SetRoundData(roundData);

        /* temp count */
        for (int i = 0; i < 2; ++i)
        {
            MaxLoadingCount++;
            AssetLoader.LoadAssetAsync<GameObject>(Database.Instance.FindCharacterAsset(i + 1000).PrefabKey, () =>
            {
                CurLoadingCount++;
                if (MaxLoadingCount == CurLoadingCount)
                    _gameMode.ChangeGameState(EGameState.ReadyGame);
            });
        }

        for (int i = 1; i <= roundData.Round.Count; ++i)
        {
            foreach (var data in roundData.Round[i - 1]._monsterSpawnData)
            {
                MaxLoadingCount++;
                AssetLoader.LoadAssetAsync<GameObject>(data.Key, () => 
                { 
                    CurLoadingCount++; 
                    if (MaxLoadingCount == CurLoadingCount)
                        _gameMode.ChangeGameState(EGameState.ReadyGame); 
                });
            }
        }

        MaxLoadingCount++;
        AssetLoader.LoadAssetAsync<GameObject>(Database.Instance.FindCharacterAsset(2003).PrefabKey, () =>
        {
            CurLoadingCount++;
            if (MaxLoadingCount == CurLoadingCount)
                _gameMode.ChangeGameState(EGameState.ReadyGame);
        });
    }

    void Initialize()
    {
        /* temp count */
        for (int i = 0; i < 2; ++i)
        {
            ObjectPool characterPool = Spawner.objectPools.GetPool(Database.Instance.FindCharacterAsset(i + 1000).PrefabKey);

            if (characterPool)
            {
                characterPool.ExpandPool(10);
            }
            else
            {
                characterPool = Spawner.gameObject.AddComponent<ObjectPool>();
                characterPool.InitializePool(AssetLoader.GetHandleInstance<GameObject>(Database.Instance.FindCharacterAsset(i + 1000).PrefabKey), 10);
            }

            Spawner.objectPools.RegisterPool(characterPool);
        }

        for (int i = 1; i <= roundData.Round.Count; ++i)
        {
            foreach (var data in roundData.Round[i - 1]._monsterSpawnData)
            {
                ObjectPool pool = Spawner.objectPools.GetPool(data.Key);

                if (pool)
                {
                    pool.ExpandPool(data.Value);
                }
                else
                {
                    pool = Spawner.gameObject.AddComponent<ObjectPool>();
                    pool.InitializePool(AssetLoader.GetHandleInstance<GameObject>(data.Key), data.Value);
                }

                Spawner.objectPools.RegisterPool(pool);
            }
        }

        ObjectPool SCVPool = Spawner.objectPools.GetPool(Database.Instance.FindCharacterAsset(2003).PrefabKey);
        SCVPool = Spawner.gameObject.AddComponent<ObjectPool>();
        SCVPool.InitializePool(AssetLoader.GetHandleInstance<GameObject>(Database.Instance.FindCharacterAsset(2003).PrefabKey), 10);
        Spawner.objectPools.RegisterPool(SCVPool);
    }

    IEnumerator Timer(float _time, Action _action)
    {
        while (_time >= 0f)
        {
            _time -= 0.1f;

            if (_time < 0)
                _action?.Invoke();

            yield return new WaitForSeconds(0.1f);
        }
    }
}