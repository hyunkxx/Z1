using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PoolContainer : MonoBehaviour
{
    Dictionary<string, ObjectPool> poolContainer = new Dictionary<string, ObjectPool>();

    private void Awake()
    {
        ObjectPool[] pools = GetComponents<ObjectPool>();
        foreach (ObjectPool pool in pools)
        {
            RegisterPool(pool);
        }
    }

    public bool RegisterPool(ObjectPool pool)
    {
        if (pool == null || string.IsNullOrEmpty(pool.PoolName))
        {
            Debug.LogError("Pool or PoolName is null/empty");
            return false;
        }

        if (poolContainer.ContainsKey(pool.PoolName))
        {
            Debug.LogWarning($"Pool with name {pool.PoolName} already exists");
            return false;
        }

        poolContainer.Add(pool.PoolName, pool);
        return true;
    }
    public bool UnregisterPool(string poolName)
    {
        if (poolContainer.TryGetValue(poolName, out ObjectPool pool))
        {
            poolContainer.Remove(poolName);
            return true;
        }

        return false;
    }
    public void ClearContainer()
    {
        poolContainer.Clear();
    }
    public ObjectPool GetPool(string poolName)
    {
        return poolContainer.GetValueOrDefault(poolName);
    }
}