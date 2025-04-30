//public class ObjectPool : Singleton<ObjectPool>
//{
//    public Dictionary<string, List<GameObject>> monsterPool = new Dictionary<string, List<GameObject>>();

//    void CreatePool(string _type, int _count)
//    {
//        GameObject poolObject = new GameObject($"{_type}_Pool");

//        if (!monsterPool.ContainsKey(_type))
//        {
//            monsterPool[_type] = new List<GameObject>();
//            poolObject.transform.parent = transform;
//        }

//        for (int i = 0; i < _count; ++i)
//        {
//            GameObject obj = Instantiate((GameObject)AssetDatabase.LoadAssetAtPath($"Assets/Level/Prefabs/{_type}.prefab", typeof(GameObject)));
//            obj.SetActive(false);
//            obj.transform.parent = poolObject.transform;
//            monsterPool[_type].Add(obj);
//        }
//    }

//    public GameObject PullObject(string _type)
//    {
//        if (!monsterPool.ContainsKey(_type)) { CreatePool(_type, 10); }

//        if (monsterPool.ContainsKey(_type))
//        {
//            var objectList = monsterPool[_type];

//            for (int i = 0; i < objectList.Count; ++i)
//            {
//                if (!objectList[i].activeSelf)
//                {
//                    objectList[i].SetActive(true);
//                    return objectList[i];

//                }
//            }

//            // ��� Ȱ��ȭ�� ���
//            int lastCount = objectList.Count;
//            CreatePool(_type, 10);

//            return objectList[lastCount];
//        }

//        return null;
//    }
//}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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