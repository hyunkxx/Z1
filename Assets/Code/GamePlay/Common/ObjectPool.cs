using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class ObjectPool : MonoBehaviour
{
    [Header("ObjectPool Properties")]
    [SerializeField] private string poolName;
    [SerializeField] private int poolSize;
    [SerializeField] private GameObject sourcePrefab;

    public string PoolName => poolName;
    public int PoolSize => poolSize;
    public GameObject SourcePrefab => sourcePrefab;
    public Queue<GameObject> Pool => objectPool;
    private GameObject poolHolder;

    public int activateCount
    {
        get
        {
            return poolSize - objectPool.Count;
        }
    }

    private Queue<GameObject> objectPool;

    private void Awake()
    {

    }

    public void LoadPoolData(string _prefabKey, int _size, Action _loadedComplete)
    {
        Addressables.InstantiateAsync(_prefabKey, Vector3.zero, Quaternion.identity).Completed += (handle) =>
        {
            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                InitializePool(handle.Result.gameObject, _size);
                _loadedComplete?.Invoke();
            }
        };
    }

    public void InitializePool(GameObject _obj, int _count)
    {
        sourcePrefab = _obj;
        poolSize = _count;
        poolName = sourcePrefab.name;

        CreatePool();
    }

    public void CreatePool()
    {
        objectPool = new Queue<GameObject>();
        poolHolder = new GameObject($"{sourcePrefab.name}_Pool");

        for (int i = 0; i < poolSize; ++i)
        {
            GameObject obj = Instantiate(sourcePrefab, transform.position, Quaternion.identity);
            obj.transform.SetParent(poolHolder.transform);
            ReturnObject(obj);
        }
    }

    public GameObject GetObject(Vector3 position, Quaternion rotation)
    {
        if (activateCount >= poolSize)
        {
            //StartCoroutine(SpawnObjectsPerFrame());
            return null;
        }

        GameObject obj = objectPool.Dequeue();

        obj.transform.position = position;
        obj.transform.rotation = rotation;

        obj.gameObject.SetActive(true);

        return obj;
    }
    public void ReturnObject(GameObject obj)
    {
        obj.gameObject.SetActive(false);
        objectPool.Enqueue(obj);
    }

    private IEnumerator SpawnObjectsPerFrame()
    {
        int size = poolSize * 2;
        while(poolSize < size)
        {
            GameObject obj = Instantiate(sourcePrefab, transform.position, Quaternion.identity);
            obj.transform.SetParent(poolHolder.transform);
            ReturnObject(obj);
            poolSize++;

            yield return null;
        }
    }
}