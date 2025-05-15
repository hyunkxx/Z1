using UnityEngine;


public class PoolManager : Singleton<PoolManager>
{
    public PoolContainer Container => container;
    private PoolContainer container;

    public ObjectPool GetPool(string poolName)
    {
        return container.GetPool(poolName);
    }

    protected override void Start()
    {
        base.Start();

        container = GetComponent<PoolContainer>();

        ObjectPool pool = gameObject.AddComponent<ObjectPool>();
        pool.InitializePool(Resources.Load<GameObject>("Common/UI/DamageFont"), 100);

        container.FindPools();
    }
}
