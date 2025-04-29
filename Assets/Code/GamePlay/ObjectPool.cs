using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ObjectPool : Singleton<ObjectPool>
{
    public Dictionary<string, List<GameObject>> monsterPool = new Dictionary<string, List<GameObject>>();

    void CreatePool(string _type, int _count)
    {
        GameObject poolObject = new GameObject($"{_type}_Pool");

        if (!monsterPool.ContainsKey(_type))
        {
            monsterPool[_type] = new List<GameObject>();
            poolObject.transform.parent = transform;
        }
        

        for (int i = 0; i < _count; ++i)
        {
            GameObject obj = Instantiate((GameObject)AssetDatabase.LoadAssetAtPath($"Assets/Level/Prefabs/{_type}.prefab", typeof(GameObject)));
            obj.SetActive(false);
            obj.transform.parent = poolObject.transform;
            monsterPool[_type].Add(obj);
        }
    }

    public GameObject PullObject(string _type)
    {
        if (!monsterPool.ContainsKey(_type)) { CreatePool(_type, 10); }

        if (monsterPool.ContainsKey(_type))
        {
            var objectList = monsterPool[_type];

            for (int i = 0; i < objectList.Count; ++i)
            {
                if (!objectList[i].activeSelf)
                {
                    objectList[i].SetActive(true);
                    return objectList[i];

                }
            }

            // 모두 활성화일 경우
            int lastCount = objectList.Count;
            CreatePool(_type, 10);

            return objectList[lastCount];
        }

        return null;
    }

}


//using UnityEngine;
//using System.Collections;
//using System.Collections.Generic;

//public class ObjectPool : MonoBehaviour
//{
//    [Header("ObjectPool Properties")]
//    [SerializeField] private string poolName;
//    [SerializeField] private int poolSize;
//    [SerializeField] private GameObject sourcePrefab;

//    public string PoolName => poolName;
//    public int PoolSize => poolSize;
//    public GameObject SourcePrefab => sourcePrefab;

//    public int activateCount
//    {
//        get
//        {
//            return poolSize - objectPool.Count;
//        }
//    }

//    private Queue<GameObject> objectPool;

//    private void Awake()
//    {
//        objectPool = new Queue<GameObject>();

//        for (int i = 0; i < poolSize; ++i)
//        {
//            GameObject obj = Instantiate(sourcePrefab, transform.position, Quaternion.identity);
//            ReturnObject(obj);
//        }
//    }

//    public GameObject GetObject(Vector3 position, Quaternion rotation)
//    {
//        GameObject obj = objectPool.Dequeue();

//        obj.transform.position = position;
//        obj.transform.rotation = rotation;

//        obj.gameObject.SetActive(true);

//        return obj;
//    }

//    public void ReturnObject(GameObject obj)
//    {
//        obj.gameObject.SetActive(false);
//        objectPool.Enqueue(obj);
//    }
//}

//public class PoolContainer : MonoBehaviour
//{
//    Dictionary<string, ObjectPool> poolContainer = new Dictionary<string, ObjectPool>();

//    private void Awake()
//    {
//        ObjectPool[] pools = GetComponents<ObjectPool>();
//        foreach(ObjectPool pool in pools)
//        {
//            RegisterPool(pool);
//        }
//    }

//    public bool RegisterPool(ObjectPool pool)
//    {
//        if (pool == null || string.IsNullOrEmpty(pool.PoolName))
//        {
//            Debug.LogError("Pool or PoolName is null/empty");
//            return false;
//        }

//        if (poolContainer.ContainsKey(pool.PoolName))
//        {
//            Debug.LogWarning($"Pool with name {pool.PoolName} already exists");
//            return false;
//        }

//        poolContainer.Add(pool.PoolName, pool);
//        return true;
//    }
//    public bool UnregisterPool(string poolName)
//    {
//        if(poolContainer.TryGetValue(poolName, out ObjectPool pool))
//        {
//            poolContainer.Remove(poolName);
//            return true;
//        }

//        return false;
//    }
//    public void ClearContainer()
//    {
//        poolContainer.Clear();
//    }
//    public ObjectPool GetPool(string poolName)
//    {
//        return poolContainer.GetValueOrDefault(poolName);
//    }
//}