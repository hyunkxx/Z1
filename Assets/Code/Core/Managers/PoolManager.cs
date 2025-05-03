using UnityEngine;


public class PoolManager : Singleton<PoolManager>
{
    public PoolContainer Container => container;
    private PoolContainer container;

    public ObjectPool GetPool(string poolName)
    {
        return container.GetPool(poolName);
    }

    public void Start()
    {
        container = GetComponent<PoolContainer>();
    }
}
