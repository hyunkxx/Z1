using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    [Header("ObjectPool Properties")]
    [SerializeField] private string poolName;
    [SerializeField] private int poolSize;
    [SerializeField] private GameObject sourcePrefab;

    public string PoolName => poolName;
    public int PoolSize => poolSize;
    public GameObject SourcePrefab => sourcePrefab;

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
        objectPool = new Queue<GameObject>();
        GameObject poolObject = new GameObject($"{poolName}_Pool");

        for (int i = 0; i < poolSize; ++i)
        {
            GameObject obj = Instantiate(sourcePrefab, transform.position, Quaternion.identity);
            obj.transform.parent = poolObject.transform;
            ReturnObject(obj);
        }
    }

    public GameObject GetObject(Vector3 position, Quaternion rotation)
    {
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
}