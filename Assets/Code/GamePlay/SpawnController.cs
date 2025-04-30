using UnityEngine;

public class SpawnController : MonoBehaviour
{
    public PoolContainer objectPools;
    
    void Start()
    {

    }

    void Update()
    {
        
    }

    public GameObject Spawn(string _type)
    {
        ObjectPool objectPool = objectPools.GetPool(_type);
        return objectPool.GetObject(Vector3.zero, Quaternion.identity);
    }
}