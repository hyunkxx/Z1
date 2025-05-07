using UnityEngine;

public class SpawnController : MonoBehaviour
{
    public PoolContainer objectPools;

    public GameObject Spawn(string _type, Vector3 _spawnPos)
    {
        ObjectPool objectPool = objectPools.GetPool(_type);
        return objectPool.GetObject(_spawnPos, Quaternion.identity);
    }
}