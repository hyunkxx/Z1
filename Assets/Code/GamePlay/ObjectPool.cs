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
