using System;
using UnityEngine;


public abstract class GameRule
    : Z1Behaviour
{
    protected GameMode _gameMode;
    public PoolContainer PoolContainer { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        _gameMode = GameManager.Instance.GameMode;

        PoolContainer = gameObject.AddComponent<PoolContainer>();
    }

    /* Common LoadData */
    protected virtual void LoadSceneData()
    {
        /* Blood Particle */
        AssetLoader.LoadAssetAsync<GameObject>("Particle_Blood", () =>
        {
            ObjectPool pool = gameObject.AddComponent<ObjectPool>();
            pool.InitializePool(AssetLoader.GetHandleInstance<GameObject>("Particle_Blood"), 100);
            PoolContainer.RegisterPool(pool);
        });

        /* Default Hit Effect */
        AssetLoader.LoadAssetAsync<GameObject>("Effect_Hit00", () =>
        {
            ObjectPool pool = gameObject.AddComponent<ObjectPool>();
            pool.InitializePool(AssetLoader.GetHandleInstance<GameObject>("Effect_Hit00"), 100);
            PoolContainer.RegisterPool(pool);
        });
    }
}